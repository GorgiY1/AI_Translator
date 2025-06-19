using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Translator.Handlers;
using Translator.Services;
using TranslatorTelegramBot;

namespace Translator.Bot
{
    public class BotService
    {
        private readonly IEnumerable<ICommandHandler> _handlers;
        private readonly TranslatorServiceBot _translatorService;
        private static readonly Dictionary<long, string> userTargetLanguages = new Dictionary<long, string>();

        public BotService(IEnumerable<ICommandHandler> handlers, TranslatorServiceBot translatorService)
        {
            _handlers = handlers;
            _translatorService = translatorService;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            // ✅ Обробка натискань кнопок (CallbackQuery)
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
            {
                var callback = update.CallbackQuery;
                var chatId = callback.Message.Chat.Id;
                var data = callback.Data;

                if (data.StartsWith("lang_"))
                {
                    string langCode = data.Replace("lang_", "");
                    userTargetLanguages[chatId] = langCode;

                    await bot.SendTextMessageAsync(chatId,
                        $"✅ Language set to {langCode.ToUpper()}.\nNow send text to translate.",
                        cancellationToken: token);

                    return;
                }
            }

            // ✅ Обробка повідомлень (Message)
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;
                var text = message.Text;

                // Якщо це команда, шукаємо хендлер
                if (text.StartsWith("/"))
                {
                    var handler = _handlers.FirstOrDefault(h => h.CanHandle(text));
                    if (handler != null)
                    {
                        await handler.HandleAsync(message, bot, token);
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(chatId,
                            "Unknown command. Try /start.",
                            cancellationToken: token);
                    }

                    return;
                }

                // Якщо це просто текст — намагаємось перекласти
                if (userTargetLanguages.TryGetValue(chatId, out string targetLang))
                {
                    string translated = await _translatorService.TranslateTextAsync(text, targetLang);
                    await bot.SendTextMessageAsync(chatId,
                        $"Translated to {targetLang.ToUpper()}:\n{translated}",
                        cancellationToken: token);
                }
                else
                {
                    await bot.SendTextMessageAsync(chatId,
                        "❗ Please choose a target language first using /menu.",
                        cancellationToken: token);
                }
            }
        }

    }

}
