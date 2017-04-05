using System;
using Microsoft.Bot.Builder.FormFlow;

namespace FieldWorkerBot.Bot.Dialogs
{
    [Serializable]
    public class DiscrepancyFormDialogBuilder
    {
        public string DiscrepancyType;
        public string Comment;

        public static IForm<DiscrepancyFormDialogBuilder> BuildForm()
        {
            return new FormBuilder<DiscrepancyFormDialogBuilder>()
                .AddRemainingFields()
                .Build();
        }

        public static IFormDialog<DiscrepancyFormDialogBuilder> BuildFormDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildForm, options);
        }
    };
}