using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Azure;
using Azure.AI.Translation.Text;
using Translator.Services;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Linq;

namespace TranslatorTelegramBot
{
    public class TranslatorServiceBot : ITranslatorService
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

        public async Task<Dictionary<string, string>> GetSupportedLanguagesAsync()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation");
            var json = JObject.Parse(response)["translation"] as JObject;

            return json.Properties()
                       .ToDictionary(
                           p => p.Name,                               // напр. "fr"
                           p => (string)p.Value["name"]);             // напр. "French"
        }


    }
}