using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TranslatorTelegramBot;

namespace Translator.Handlers
{
    public class CallbackQueryHandler
    {
        private readonly TranslatorServiceBot _translatorService;

        public CallbackQueryHandler(TranslatorServiceBot translatorService)
        {
            _translatorService = translatorService;
        }

        public async Task HandleCallbackAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            string targetLanguage = callbackQuery.Data?.Replace("lang_", "");

            if (!string.IsNullOrEmpty(targetLanguage))
            {
                await botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    $"You selected target language: {targetLanguage.ToUpper()}\nNow send me the text to translate.",
                    cancellationToken: cancellationToken
                );

                // You could store the selected language per-user using a dictionary or a small state machine
            }
        }
    }

}
