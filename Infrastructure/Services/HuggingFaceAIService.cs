using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public interface IAIService
{
    Task<string> GetResponseAsync(Guid userId, string message);
}

public class HuggingFaceAIService : IAIService
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _config;
    private readonly IApplicationDbContext _db;
    private readonly IPaymentService _paymentService;

    public HuggingFaceAIService(
        IHttpClientFactory factory,
        IConfiguration config,
        IApplicationDbContext db,
        IPaymentService paymentService)
    {
        _factory = factory;
        _config = config;
        _db = db;
        _paymentService = paymentService;
    }

    public async Task<string> GetResponseAsync(Guid userId, string message)
    {
        var apiKey = _config["HuggingFace:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            return "Missing API key";

        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return "User not found";

        var customerId = user.StripeCustomerId.ToString();

        var cards = await _paymentService.GetCards(customerId);

        var history =  await _paymentService.GetHistory(customerId);

        var cardsText = string.Join(", ", cards.Select(c =>
                $"{c.Brand} ****{c.Last4} exp {c.ExpMonth}/{c.ExpYear}"
            ));

        var historyText = string.Join(", ", history.Select(h =>
            $"{h.Amount} {h.Currency} {h.Status}"
        ));

        var userContext = $@"
            User: {user.FullName}
            Email: {user.Email}
            Cards: {cardsText}
            Transactions: {historyText}";

        var systemPrompt =
            "You are a financial AI assistant inside FinancesManager. Help users understand transactions, cards, and spending.";

        var appContext =
            "Rules: Only financial answers. Be concise. If unknown data, say you don't know.";

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            model = "meta-llama/Llama-3.1-8B-Instruct",
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "system", content = appContext },
                new { role = "system", content = userContext },
                new { role = "user", content = message }
            }
        };

        var response = await client.PostAsync(
            "https://router.huggingface.co/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        );

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("choices", out var choices) ||
            choices.GetArrayLength() == 0)
        {
            return "No AI response";
        }

        return choices[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "Empty response";
    }
}