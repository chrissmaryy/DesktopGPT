using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DesktopGPT.Data;

namespace DesktopGPT.Classes
{
    internal class Chat
    {
        private readonly DesktopGPTMain desktopGPTMain;
        private readonly HttpClient httpClient;

        public Chat(DesktopGPTMain desktopGPTMain)
        {
            this.desktopGPTMain = desktopGPTMain;
            string api_key = UserRepository.GetAPIKey()[0]["api_key"].ToString();

            httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {api_key}");
        }

        public async Task StreamResponseAsync(List<Dictionary<string, object>> messages, int chat_id, string model)
        {
            string endpoint = "https://api.openai.com/v1/chat/completions";
            string temperature = UserRepository.GetTemperature()[0]["temperature"].ToString();

            var requestBody = new
            {
                model = model,
                messages = messages,
                temperature = temperature,
                stream = true
            };

            using HttpRequestMessage request = new(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Request failed with status code {response.StatusCode}. Make sure the settings are correct.";
                desktopGPTMain.ErrorCallback(errorMessage);
                return;
            }

            using Stream stream = await response.Content.ReadAsStreamAsync();
            using StreamReader reader = new(stream);

            await Task.Run(async () =>
            {
                StringBuilder answerStringBuilder = new();
                StringBuilder chunkStringBuilder = new();
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("data: "))
                    {
                        string jsonStr = line["data: ".Length..].Trim();
                        if (jsonStr == "[DONE]")
                        {
                            if (chunkStringBuilder.Length > 0)
                            {
                                await desktopGPTMain.StreamResponseCallback(answerStringBuilder.ToString());
                            }
                            MessageRepository.InsertMessage(chat_id, model, "assistant", answerStringBuilder.ToString(), DateTime.Now);
                            break;
                        }

                        string chunk = GetContentFromJsonStr(jsonStr);
                        if (!string.IsNullOrEmpty(chunk))
                        {
                            chunkStringBuilder.Append(chunk);
                            answerStringBuilder.Append(chunk);
                            if (chunkStringBuilder.Length > 30)
                            {
                                await desktopGPTMain.StreamResponseCallback(answerStringBuilder.ToString());
                                chunkStringBuilder = new StringBuilder();
                            }
                        }
                    }
                }
            });
        }

        private string GetContentFromJsonStr(string jsonStr)
        {
            try
            {
                JObject json = JObject.Parse(jsonStr);
                JToken? delta = json["choices"]?[0]?["delta"];
                JToken? content = delta?["content"];
                if (delta != null && content != null)
                {
                    string chunk = content.ToString();
                    return chunk;
                }
                return "";
            }
            catch (JsonReaderException)
            {
                return "";
            }
        }

    }
}
