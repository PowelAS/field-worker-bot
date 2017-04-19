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
    internal class FieldWorkerDialog : LuisDialog<object>
    {
        private IAwaitable<IMessageActivity> _messageActivity;
        private Random _random = new Random();
        private InspectionDialog _inspectionDialog;
        
        private string _substationName = null;
        private List<DiscrepancyFormDialogBuilder> _discrepancyFormBuilders = new List<DiscrepancyFormDialogBuilder>();

        public FieldWorkerDialog(ILuisService service) : base(service)
        {
            _inspectionDialog = new InspectionDialog(service);
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

        [LuisIntent("greet")]
        public async Task Greet(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, I'm André. How may I assist you today?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm here to help you register discrepancies found in substations during inspections. I also have a very soothing voice for you to enjoy.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("angry")]
        public async Task Angry(IDialogContext context, LuisResult result)
        {
            var responses = new[]
            {
                "Hey, be careful there!",
                "I'm only a bot you know.",
                "Do you kiss your grandma with that mouth?",
                "Is that all you've got? I've heard toddlers throwing worse insults than that!"
            };
            var message = responses[_random.Next(responses.Length)];
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("happy")]
        public async Task Happy(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("That's so kind of you!");
            context.Wait(MessageReceived);
        }

        [LuisIntent("startInspection")]
        public async Task StartInspection(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Inspection is started.");
            context.Call(_inspectionDialog, null);
        }


    }
}