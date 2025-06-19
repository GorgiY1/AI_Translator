using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Translator.Services;

namespace Translator.Handlers
{
    public class SpeechTranslateCommandHandler : ICommandHandler
    {
        private readonly SpeechToSpeechService _speechToSpeechService;

        public SpeechTranslateCommandHandler(SpeechToSpeechService speechToSpeechService)
        {
            _speechToSpeechService = speechToSpeechService;
        }

        public bool CanHandle(string command) => command.StartsWith("/s2s");

        public async Task HandleAsync(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            if (message.Voice == null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    "❗ Please send a voice message after using /s2s.",
                    cancellationToken: cancellationToken);
                return;
            }

            var targetLang = "uk"; // Example: default target lang
            var fileStream = new MemoryStream();

            var file = await botClient.GetFileAsync(message.Voice.FileId, cancellationToken);
            await botClient.DownloadFileAsync(file.FilePath, fileStream, cancellationToken);
            fileStream.Position = 0;

            try
            {
                var translatedAudio = await _speechToSpeechService.TranslateSpeechAsync(fileStream, targetLang);
                await botClient.SendVoiceAsync(message.Chat.Id, InputFile.FromStream(translatedAudio), cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"❗ Error: {ex.Message}", cancellationToken: cancellationToken);
            }
        }
    }

}
