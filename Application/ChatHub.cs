using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Application
{
    public class ChatHub : Hub
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatHub(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            var hfApiKey = "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"; // make sure it has inference access
            if (string.IsNullOrEmpty(hfApiKey))
            {
                await Clients.All.SendAsync("ReceiveMessage", "AI Bot", "Hugging Face API key not found.");
                return;
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hfApiKey);

            var requestBody = new
            {
                model = "meta-llama/Llama-3.1-8B-Instruct",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = message
                    }
                }
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://router.huggingface.co/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                string aiMessage = "AI couldn't respond.";

                try
                {
                    using var jsonDoc = JsonDocument.Parse(responseString);

                    if (jsonDoc.RootElement.TryGetProperty("choices", out var choices))
                    {
                        aiMessage = choices[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();
                    }
                }
                catch
                {
                    aiMessage = "Failed to parse AI response.";
                }

                await Clients.All.SendAsync("ReceiveMessage", "AI Bot", aiMessage);
            }
            catch (Exception ex)
            {
                await Clients.All.SendAsync("ReceiveMessage", "AI Bot", $"Request failed: {ex.Message}");
            }
        }
    }
}