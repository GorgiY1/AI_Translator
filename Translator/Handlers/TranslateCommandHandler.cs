using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Translator.Services;

namespace Translator.Handlers
{
    public class TranslateCommandHandler : ICommandHandler
    {
        private readonly ITranslatorService _translator;

        public TranslateCommandHandler(ITranslatorService translator)
        {
            _translator = translator;
        }

        public bool CanHandle(string command) => command.StartsWith("/translate");

        public async Task HandleAsync(Message message, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var text = message.Text;
            var segments = text.Split(new[] { ' ' }, 4);
            string inputText, targetLanguage, sourceLanguage = null;

            if (segments.Length < 3)
            {
                await bot.SendTextMessageAsync(chatId,
                    "Use:\n/translate <source_lang> <target_lang> <text>\nOR\n/translate <target_lang> <text>",
                    cancellationToken: cancellationToken);
                return;
            }

            if (segments.Length == 3)
            {
                targetLanguage = segments[1];
                inputText = segments[2];
            }
            else
            {
                sourceLanguage = segments[1];
                targetLanguage = segments[2];
                inputText = segments[3];
            }

            var translated = await _translator.TranslateTextAsync(inputText, targetLanguage, sourceLanguage);

            await bot.SendTextMessageAsync(chatId,
                $"Original: {inputText}\n\nTranslated ({targetLanguage}): {translated}",
                cancellationToken: cancellationToken);
        }
    }

}
