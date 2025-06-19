using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Newtonsoft.Json.Linq;
using Translator.Services;
using TranslatorTelegramBot;

namespace Translator.Handlers
{
    public class MenuCommandHandler : ICommandHandler
    {
        private TranslatorServiceBot _translatorService;
        public MenuCommandHandler(TranslatorServiceBot translatorService)
        {
            _translatorService = translatorService;
        }
        public bool CanHandle(string command) => command.Equals("/menu", StringComparison.OrdinalIgnoreCase);

        public async Task HandleAsync(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            
            var langs = await _translatorService.GetSupportedLanguagesAsync();
            var buttons = langs
                .Select(kv => InlineKeyboardButton.WithCallbackData(kv.Value, $"lang_{kv.Key}"))
                .Take(200)
                .ChunkBy(6)
                .ToArray();

            var keyboard = new InlineKeyboardMarkup(buttons);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "🗣️ Choose target language:",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }

}
