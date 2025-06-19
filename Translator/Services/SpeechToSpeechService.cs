using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorTelegramBot;

namespace Translator.Services
{
    public class SpeechToSpeechService
    {
        private readonly ISpeechToTextService _speechToTextService;
        private readonly ITextToSpeechService _textToSpeechService;
        private readonly TranslatorServiceBot _translatorService;

        public SpeechToSpeechService(
            ISpeechToTextService speechToTextService,
            ITextToSpeechService textToSpeechService,
            TranslatorServiceBot translatorService)
        {
            _speechToTextService = speechToTextService;
            _textToSpeechService = textToSpeechService;
            _translatorService = translatorService;
        }

        public async Task<Stream> TranslateSpeechAsync(Stream audioStream, string targetLanguage, string sourceLanguage = null)
        {
            var recognizedText = await _speechToTextService.SpeechToTextAsync(audioStream);

            if (string.IsNullOrWhiteSpace(recognizedText))
                throw new Exception("Speech not recognized.");

            var translatedText = await _translatorService.TranslateTextAsync(recognizedText, targetLanguage, sourceLanguage);
            var resultAudio = await _textToSpeechService.TextToSpeechAsync(translatedText, targetLanguage);

            return resultAudio;
        }
    }

}
