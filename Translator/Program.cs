using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TranslatorTelegramBot;

internal class Program
{
    // Replace with your credentials
   
    private static ITelegramBotClient botClient;
    private static TranslatorServiceBot translatorService;

    static async Task Main(string[] args)
    {
        try
        {
            // Validate credentials
            if (BotToken == "your-telegram-bot-token")
                throw new Exception("Please set your Telegram Bot Token.");
            if (TranslatorKey == "your-translator-key")
                throw new Exception("Please set TRANSLATOR_KEY in environment variables.");

            // Initialize bot and translator
            botClient = new TelegramBotClient(BotToken);
            translatorService = new TranslatorServiceBot(TranslatorKey, TranslatorRegion);

            // Get bot info
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Bot started: {me.Username}");

            // Start receiving updates
            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
            Console.WriteLine("Press Ctrl+C to stop the bot...");
            await Task.Delay(Timeout.Infinite);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Startup error: {ex.Message}");
            Console.ReadKey();
        }


        
            //Console.WriteLine("Enter text to translate:");
            //string inputText = Console.ReadLine();

            //Console.WriteLine("Enter target language (e.g., 'fr' for French):");
            //string targetLanguage = Console.ReadLine();

            //string translatedText = await TranslatorService.TranslateTextAsync(inputText, targetLanguage);
            //Console.WriteLine($"Translated text: {translatedText}");
            //Console.ReadKey();
        

    }
    static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message?.Text == null)
            return;

        var message = update.Message;
        var chatId = message.Chat.Id;
        var text = message.Text;

        try
        {
            if (text.StartsWith("/start"))
            {
                await bot.SendTextMessageAsync(
                    chatId,
                    "Welcome! Use /translate <source_lang> <target_lang> <text> to translate. " +
                    "E.g., '/translate en es Hello, how are you?' for English to Spanish. " +
                    "Omit <source_lang> for auto-detection, e.g., '/translate es Hello, how are you?'. " +
                    "Supported codes: es (Spanish), fr (French), de (German), etc. See https://aka.ms/translator-languages.",
                    cancellationToken: cancellationToken);
                return;
            }

            if (text.StartsWith("/translate"))
            {
                var segments = text.Split(new[] { ' ' }, 4);
                string inputText, targetLanguage, sourceLanguage = null;

                if (segments.Length < 3)
                {
                    await bot.SendTextMessageAsync(
                        chatId,
                        "Invalid format. Use:\n/translate <source_lang> <target_lang> <text>\nOR\n/translate <target_lang> <text>",
                        cancellationToken: cancellationToken);
                    return;
                }

                if (segments.Length == 3)
                {
                    // /translate <target_lang> <text>
                    targetLanguage = segments[1];
                    inputText = segments[2];
                }
                else
                {
                    // /translate <source_lang> <target_lang> <text>
                    sourceLanguage = segments[1];
                    targetLanguage = segments[2];
                    inputText = segments[3];
                }

                // Перевірка на ліміт
                if (inputText.Length > 10000)
                {
                    await bot.SendTextMessageAsync(
                        chatId,
                        "Error: Text exceeds 10,000 character limit. Please shorten your text.",
                        cancellationToken: cancellationToken);
                    return;
                }

                string translatedText = await translatorService.TranslateTextAsync(inputText, targetLanguage, sourceLanguage);

                await bot.SendTextMessageAsync(
                    chatId,
                    $"Original: {inputText}\n\nTranslated ({targetLanguage}): {translatedText}",
                    cancellationToken: cancellationToken);
            }
            else
            {
                await bot.SendTextMessageAsync(
                    chatId,
                    "Use /translate <source_lang> <target_lang> <text>\nOR /translate <target_lang> <text>.",
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            await bot.SendTextMessageAsync(
                chatId,
                $"Error: {ex.Message}",
                cancellationToken: cancellationToken);
        }
    }

    static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}
//internal class Program
//{
//    private static readonly string BotToken = "7812640284:AAFIceTmWZ6xu14CsDOyMaJX50_x5O9oeQk";
//    private static readonly string TranslatorKey = Environment.GetEnvironmentVariable("TranslatorKey") ?? "3nx3xwTnQ4OzPombwaSVwp68fGEmdjDgMnFqDQIAYT19bj9zHVQ9JQQJ99BFACi5YpzXJ3w3AAAbACOGIYbn";
//    private static readonly string TranslatorRegion = "northeurope";
//    private static ITelegramBotClient botClient;
//    private static TranslatorServiceBot translatorService;

//    static async Task Main(string[] args)
//    {
//        try
//        {
//            // Validate credentials
//            if (BotToken == "your-telegram-bot-token")
//                throw new Exception("Please set your Telegram Bot Token.");
//            if (TranslatorKey == "your-translator-key")
//                throw new Exception("Please set TRANSLATOR_KEY in environment variables.");

//            // Initialize bot and translator
//            botClient = new TelegramBotClient(BotToken);
//            translatorService = new TranslatorServiceBot(TranslatorKey, TranslatorRegion);

//            // Get bot info
//            var me = await botClient.GetMeAsync();
//            Console.WriteLine($"Bot started: {me.Username}");

//            // Start local HTTP server to receive updates from Azure Function
//            StartHttpServer();
//            Console.WriteLine("Local HTTP server started at http://localhost:8080. Press Ctrl+C to stop...");
//            await Task.Delay(Timeout.Infinite);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Startup error: {ex.Message}");
//            Console.ReadKey();
//        }
//    }

//    static void StartHttpServer()
//    {
//        HttpListener listener = new HttpListener();
//        listener.Prefixes.Add("http://localhost:8080/");
//        listener.Start();

//        Task.Run(async () =>
//        {
//            while (true)
//            {
//                try
//                {
//                    var context = await listener.GetContextAsync();
//                    var request = context.Request;
//                    var response = context.Response;

//                    if (request.HttpMethod == "POST")
//                    {
//                        using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
//                        {
//                            string requestBody = await reader.ReadToEndAsync();
//                            Console.WriteLine($"Received update: {requestBody}");

//                            try
//                            {
//                                var update = JsonConvert.DeserializeObject<Update>(requestBody);
//                                await HandleUpdateAsync(botClient, update, CancellationToken.None);
//                            }
//                            catch (Exception ex)
//                            {
//                                Console.WriteLine($"Error processing update: {ex.Message}");
//                            }
//                        }
//                    }

//                    response.StatusCode = 200;
//                    response.Close();
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Server error: {ex.Message}");
//                }
//            }
//        });
//    }

//    static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
//    {
//        if (update.Type != UpdateType.Message || update.Message?.Text == null)
//            return;

//        var message = update.Message;
//        var chatId = message.Chat.Id;
//        var text = message.Text;

//        try
//        {
//            if (text.StartsWith("/start"))
//            {
//                Console.WriteLine("Received /start command");
//                await bot.SendTextMessageAsync(
//                    chatId,
//                    "Welcome! Use /translate <source_lang> <target_lang> <text> to translate. " +
//                    "E.g., '/translate en es Hello, how are you?' for English to Spanish. " +
//                    "Omit <source_lang> for auto-detection, e.g., '/translate es Hello, how are you?'. " +
//                    "Supported codes: es (Spanish), fr (French), de (German), etc. See https://aka.ms/translator-languages.",
//                    cancellationToken: cancellationToken);
//                return;
//            }

//            if (text.StartsWith("/translate"))
//            {
//                var segments = text.Split(new[] { ' ' }, 4);
//                string inputText, targetLanguage, sourceLanguage = null;

//                if (segments.Length < 3)
//                {
//                    await bot.SendTextMessageAsync(
//                        chatId,
//                        "Invalid format. Use:\n/translate <source_lang> <target_lang> <text>\nOR\n/translate <target_lang> <text>",
//                        cancellationToken: cancellationToken);
//                    return;
//                }

//                if (segments.Length == 3)
//                {
//                    targetLanguage = segments[1];
//                    inputText = segments[2];
//                }
//                else
//                {
//                    sourceLanguage = segments[1];
//                    targetLanguage = segments[2];
//                    inputText = segments[3];
//                }

//                if (inputText.Length > 10000)
//                {
//                    await bot.SendTextMessageAsync(
//                        chatId,
//                        "Error: Text exceeds 10,000 character limit. Please shorten your text.",
//                        cancellationToken: cancellationToken);
//                    return;
//                }

//                string translatedText = await translatorService.TranslateTextAsync(inputText, targetLanguage, sourceLanguage);
//                await bot.SendTextMessageAsync(
//                    chatId,
//                    $"Original: {inputText}\n\nTranslated ({targetLanguage}): {translatedText}",
//                    cancellationToken: cancellationToken);
//            }
//            else
//            {
//                await bot.SendTextMessageAsync(
//                    chatId,
//                    "Use /translate <source_lang> <target_lang> <text>\nOR /translate <target_lang> <text>.",
//                    cancellationToken: cancellationToken);
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"HandleUpdate error: {ex.Message}");
//            await bot.SendTextMessageAsync(chatId, $"Error: {ex.Message}", cancellationToken: cancellationToken);
//        }
//    }

//    static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
//    {
//        Console.WriteLine($"Error: {exception.Message}");
//        return Task.CompletedTask;
//    }
//}
