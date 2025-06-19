using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Translator.Handlers
{
    public class StartCommandHandler : ICommandHandler
    {
        public bool CanHandle(string command) => command.StartsWith("/start");

        public async Task HandleAsync(Message message, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            string helpMessage =
    "🤖 Welcome to the Translator Bot!\n\n" +
    "📌 Available Commands:\n" +
    "/start – Show welcome message\n" +
    "/menu – 🌍 Choose a language for translation from a list\n" +
    "/translate <from> <to> <text> – Translate manually (e.g. /translate en fr Hello)\n" +
    "/help – Show this help message\n\n" +
    "📝 After choosing a target language via /menu, just send any text and I’ll translate it automatically!\n\n" +
    "🌐 Example:\n" +
    "/translate en uk How are you?\n" +
    "→ Як справи?\n\n" +
    "💡 Tip: You can use language codes like en (English), es (Spanish), de (German), etc.\n\n" +
    "🔤 Supported Languages:\n" +
    "af — Afrikaans\n" +
    "sq — Albanian\n" +
    "am — Amharic\n" +
    "ar — Arabic\n" +
    "hy — Armenian\n" +
    "as — Assamese\n" +
    "az — Azerbaijani\n" +
    "bn — Bangla\n" +
    "ba — Bashkir\n" +
    "eu — Basque\n" +
    "be — Belarusian\n" +
    "bs — Bosnian\n" +
    "bg — Bulgarian\n" +
    "my — Burmese\n" +
    "ca — Catalan\n" +
    "zh-Hans — Chinese (Simplified)\n" +
    "zh-Hant — Chinese (Traditional)\n" +
    "hr — Croatian\n" +
    "cs — Czech\n" +
    "da — Danish\n" +
    "nl — Dutch\n" +
    "en — English\n" +
    "et — Estonian\n" +
    "fi — Finnish\n" +
    "fr — French\n" +
    "gl — Galician\n" +
    "ka — Georgian\n" +
    "de — German\n" +
    "el — Greek\n" +
    "gu — Gujarati\n" +
    "ht — Haitian Creole\n" +
    "he — Hebrew\n" +
    "hi — Hindi\n" +
    "hu — Hungarian\n" +
    "is — Icelandic\n" +
    "id — Indonesian\n" +
    "ga — Irish\n" +
    "it — Italian\n" +
    "ja — Japanese\n" +
    "kn — Kannada\n" +
    "kk — Kazakh\n" +
    "km — Khmer\n" +
    "ko — Korean\n" +
    "ky — Kyrgyz\n" +
    "lo — Lao\n" +
    "lv — Latvian\n" +
    "lt — Lithuanian\n" +
    "mk — Macedonian\n" +
    "mg — Malagasy\n" +
    "ms — Malay\n" +
    "ml — Malayalam\n" +
    "mt — Maltese\n" +
    "mi — Maori\n" +
    "mr — Marathi\n" +
    "mn — Mongolian\n" +
    "ne — Nepali\n" +
    "no — Norwegian\n" +
    "or — Odia\n" +
    "ps — Pashto\n" +
    "fa — Persian\n" +
    "pl — Polish\n" +
    "pt — Portuguese\n" +
    "pa — Punjabi\n" +
    "ro — Romanian\n" +
    "ru — Russian\n" +
    "sr — Serbian\n" +
    "si — Sinhala\n" +
    "sk — Slovak\n" +
    "sl — Slovenian\n" +
    "so — Somali\n" +
    "es — Spanish\n" +
    "sw — Swahili\n" +
    "sv — Swedish\n" +
    "tl — Tagalog\n" +
    "ta — Tamil\n" +
    "te — Telugu\n" +
    "th — Thai\n" +
    "bo — Tibetan\n" +
    "tr — Turkish\n" +
    "tk — Turkmen\n" +
    "uk — Ukrainian\n" +
    "ur — Urdu\n" +
    "uz — Uzbek\n" +
    "vi — Vietnamese\n" +
    "cy — Welsh\n" +
    "yi — Yiddish\n" +
    "yo — Yoruba\n" +
    "zu — Zulu\n";



            await bot.SendTextMessageAsync(chatId, helpMessage, cancellationToken: cancellationToken);
        }
    }

}
