using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TranslatorService
{
    private static readonly string subscriptionKey = "";
    private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";
    private static readonly string location = "norwayeast";
    private string translatorKey;
    private string translatorRegion;

    public TranslatorService(string translatorKey, string translatorRegion)
    {
        this.translatorKey = translatorKey;
        this.translatorRegion = translatorRegion;
    }

    public static async Task<string> TranslateTextAsync(string inputText, string targetLanguage)
    {
        // Validate inputs
        if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(targetLanguage))
        {
            throw new ArgumentException("Input text and target language must not be empty.");
        }

        string route = $"/translate?api-version=3.0&to={targetLanguage}";
        object[] body = new object[] { new { Text = inputText } };
        var requestBody = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", location);

            try
            {
                HttpResponseMessage response = await client.PostAsync(endpoint + route, requestBody);
                response.EnsureSuccessStatusCode(); // Throws if response is not successful

                string result = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                JArray jsonResponse = JArray.Parse(result);

                // Check if the array is empty or the expected structure is missing
                if (jsonResponse.Count == 0 || jsonResponse[0]["translations"] == null || jsonResponse[0]["translations"][0] == null)
                {
                    throw new Exception("Translation response is empty or invalid.");
                }

                // Extract the translated text
                string translatedText = jsonResponse[0]["translations"][0]["text"]?.ToString();
                if (string.IsNullOrEmpty(translatedText))
                {
                    throw new Exception("Translated text not found in response.");
                }

                return translatedText;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return null;
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return null;
            }
        }
    }
}