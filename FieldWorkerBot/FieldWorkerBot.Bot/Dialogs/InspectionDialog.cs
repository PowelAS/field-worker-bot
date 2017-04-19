using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FieldWorkerBot.Services.Interfaces;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace FieldWorkerBot.Bot.Dialogs
{
    [Serializable]
    internal class InspectionDialog : LuisDialog<object>
    {
        private IAwaitable<IMessageActivity> _messageActivity;
        private Random _random = new Random();
        
        private string _substationName = null;
        private List<DiscrepancyFormDialogBuilder> _discrepancyFormBuilders = new List<DiscrepancyFormDialogBuilder>();

        public InspectionDialog(ILuisService service) : base(service)
        {
        }

        protected override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> messageActivity)
        {
            _messageActivity = messageActivity;
            return base.MessageReceived(context, messageActivity);
        }


        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry, I didn't understand that.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("getDiscrepancyType")]
        public async Task SelectControlPoints(IDialogContext context, LuisResult result)
        {
            if (String.IsNullOrEmpty(_substationName))
            {
                await context.PostAsync("You need to tell what substation you want to select.");
                context.Wait(MessageReceived);
            }
            else
            {
                var discrepancyService =
                    WebApiApplication.Container.Resolve<IDiscrepancyTypeService>();
                var assetService = WebApiApplication.Container.Resolve<IAssetService>();
                var inspectionService = WebApiApplication.Container.Resolve<IInspectionService>();
                var asset = await assetService.GetByName(_substationName);
                var inspection = await inspectionService.GetNotPerformedByAssetId(asset.ObjectId);
                var discrepancyTypes = await discrepancyService.Get(asset.ObjectId, inspection.Id);

                await context.PostAsync($"You have your discrepancy types here:\n {JsonConvert.SerializeObject(discrepancyTypes)}");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("assetAllGood")]
        public async Task AssetAllGood(IDialogContext context, LuisResult result)
        {
            if (String.IsNullOrEmpty(_substationName))
            {
                await context.PostAsync($"You need to tell what substation you want to select.");
                context.Wait(MessageReceived);
            }
            else
            {
                PromptDialog.Confirm(context, AfterConfirmAsync, "Are you sure that you want to finish this inspection?");
                await Task.FromResult<Object>(null);
            }
        }

        [LuisIntent("reportDiscrepancy")]
        public async Task ReportDiscrepancy(IDialogContext context, LuisResult result)
        {
            EntityRecommendation discrepancyTypeEntity;
            var discrepancyDialog = new DiscrepancyFormDialogBuilder();

            if (result.TryFindEntity("ControlPoint", out discrepancyTypeEntity))
            {
                var discrepancyType = discrepancyTypeEntity.Entity.ToUpper();
                discrepancyDialog = new DiscrepancyFormDialogBuilder { DiscrepancyType = discrepancyType };
            }

            var form = new FormDialog<DiscrepancyFormDialogBuilder>(
                    discrepancyDialog,
                    DiscrepancyFormDialogBuilder.BuildForm,
                    FormOptions.PromptInStart,
                    result.Entities);

            context.Call(form, AfterDiscrepancyAsync);
        }

        [LuisIntent("commitReport")]
        public async Task CommitReport(IDialogContext context, LuisResult result)
        {
            if (String.IsNullOrEmpty(_substationName))
            {
                await context.PostAsync($"You need to tell what substation you want to select.");
            }
            else
            {
                await context.PostAsync($"Committing inspection on substation {_substationName} with {_discrepancyFormBuilders.Count} discrepancies.");
                _discrepancyFormBuilders.Clear();
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("selectSubstation")]
        public async Task SelectSubstation(IDialogContext context, LuisResult result)
        {
            string extractedName;
            string message = "I'm sorry, I didn't catch the name of the substation.";

            if (TryExtractingSubstationName(result, out extractedName))
            {
                var asset = await WebApiApplication.Container.Resolve<IAssetService>().GetByName(extractedName);
                _substationName = asset != null ? extractedName : null;
                message = asset != null ? $"The substation {_substationName} has been chosen." : $"Sorry, I didn't find a substation called '{extractedName}'.";
            }

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("checkMyReport")]
        public async Task CheckMyReport(IDialogContext context, LuisResult result)
        {
            if (!String.IsNullOrEmpty(_substationName))
            {
                await context.PostAsync($"You have selected substation {_substationName}.");
            }

            await context.PostAsync($"You have logged {_discrepancyFormBuilders.Count} discrepancies.");

            foreach (var discrepancy in _discrepancyFormBuilders)
                await context.PostAsync($"Type {discrepancy.DiscrepancyType}. Comment: \"{discrepancy.Comment}\"");

            context.Wait(MessageReceived);
        }

        public async Task AfterConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                var assetService = WebApiApplication.Container.Resolve<IAssetService>();
                var inspectionService = WebApiApplication.Container.Resolve<IInspectionService>();
                var asset = await assetService.GetByName(_substationName);
                var inspection = await inspectionService.GetNotPerformedByAssetId(asset.ObjectId);
                await inspectionService.CommitOk(inspection);

                await context.PostAsync($"Committing inspection on substation {_substationName} as everything is ok");
            }
            else
            {
                await context.PostAsync($"Committing inspection on substation {_substationName} is canceled");
            }
            context.Done(true);
        }

        private async Task AfterDiscrepancyAsync(IDialogContext context, IAwaitable<DiscrepancyFormDialogBuilder> result)
        {
            var discrepancy = await result;
            _discrepancyFormBuilders.Add(discrepancy);
            await context.PostAsync($"OK, registering a discrepancy of type {discrepancy.DiscrepancyType} with the following comment: {discrepancy.Comment}");
            context.Done(true);
        }

        bool TryExtractingSubstationName(LuisResult result, out string substationName)
        {
            EntityRecommendation substationEntity;

            if (result.TryFindEntity("SubstationName", out substationEntity))
            {
                substationName = substationEntity.Entity.ToUpper().Replace(" ", "");

                if (ContainsDigits(substationName))
                    return true;
            }

            if (TryExtractingSubstationName(result.Query, out substationName))
            {
                substationName = substationName.Replace(" ", "");

                if (ContainsDigits(substationName))
                    return true;
            }

            substationName = "";
            return false;
        }

        private bool TryExtractingSubstationName(string query, out string substationName)
        {
            substationName = "";
            var matches = Regex.Matches(query, "\\w{1,5}\\s?\\d{1,5}");
            if (matches.Count == 0) return false;
            substationName = matches[0].Value;
            return true;
        }

        private bool ContainsDigits(string s)
        {
            foreach (char c in s)
                if (char.IsDigit(c))
                    return true;

            return false;
        }

    }
}