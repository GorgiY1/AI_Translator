using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Azure;
using Azure.AI.Translation.Text;

namespace TranslatorTelegramBot
{
    public class TranslatorServiceBot
    {
        private readonly TextTranslationClient _client;

        public TranslatorServiceBot(string key, string region)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(region))
                throw new ArgumentException("Key and region must not be empty.");
            AzureKeyCredential credential = new AzureKeyCredential(key);
            _client = new TextTranslationClient(credential, region);
        }

        public async Task<string> TranslateTextAsync(string text, string toLanguage, string fromLanguage = null)
        {
            try
            {
                if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(toLanguage))
                    throw new ArgumentException("Text and target language must not be empty.");
                if (text.Length > 10000)
                    throw new ArgumentException("Text exceeds 10,000 character limit.");

                var result = await _client.TranslateAsync(toLanguage, text, fromLanguage);
                return result.Value[0].Translations[0].Text;
            }
            catch (RequestFailedException ex)
            {
                return $"Error: {ex.ErrorCode} - {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}