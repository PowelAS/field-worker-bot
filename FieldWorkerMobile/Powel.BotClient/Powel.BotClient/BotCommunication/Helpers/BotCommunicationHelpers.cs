using System;
using System.Collections.Generic;
using System.Linq;
using Powel.BotClient.BotCommunication.Models;

namespace Powel.BotClient.BotCommunication.Helpers
{
    public class BotCommunicationHelpers
    {
        public static int GetLastUserMessageIndex(IEnumerable<Message> botConversationMessages, IEnumerable<string> localConversationMessages)
        {
            var conversationList = botConversationMessages.ToList();
            var lastUserInputIndex =
                conversationList.ToList().FindLastIndex(m => String.Equals(m.Text, localConversationMessages.Last()));

            return lastUserInputIndex;
        }

        public static bool IsBotResponseMultiline(IEnumerable<Message> botConversationMessages, IEnumerable<string> localConversationMessages)
        {
            var botMessages = botConversationMessages.ToList();
            var lastUserMessageIndex = GetLastUserMessageIndex(botMessages, localConversationMessages);
            return botMessages.ToList().Count - 1 - lastUserMessageIndex > 1;
        }

        public static string FormatMultilineBotResponse(IEnumerable<Message> botConversationMessages, IEnumerable<string> localConversationMessages)
        {
            var botMessages = botConversationMessages.ToList();
            var lastUserMessageIndex = GetLastUserMessageIndex(botMessages, localConversationMessages);
            botMessages.Reverse();
            var multilineMessage = botMessages
                .Take(botMessages.Count - 1 - lastUserMessageIndex)
                .ToList();
            multilineMessage.Reverse();

            return String.Join(Environment.NewLine, multilineMessage.Select(m => m.Text));
        }
    }
}