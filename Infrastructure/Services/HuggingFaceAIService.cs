using Application.DTOs.AI;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Infrastructure.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public class HuggingFaceAIService : IAIService
{
    private readonly IHttpClientFactory _factory;
    private readonly IApplicationDbContext _db;
    private readonly IPaymentService _paymentService;
    private readonly HuggingFaceSettings _settings;

    public HuggingFaceAIService(
        IHttpClientFactory factory,
        IApplicationDbContext db,
        IPaymentService paymentService,
        IOptions<HuggingFaceSettings> options)
    {
        _factory = factory;
        _db = db;
        _paymentService = paymentService;
        _settings = options.Value;
    }

    public async Task<string> GetResponseAsync(Guid userId, string message)
    {
        if (string.IsNullOrEmpty(_settings.ApiKey))
            return "Missing API key";

        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new NotFoundException("User not found");

        var customerId = user.StripeCustomerId;

        var cards = await _paymentService.GetCardsAsync(customerId);

        var history =  await _paymentService.GetHistoryAsync(customerId);

        var cardsText = string.Join(", ", cards.Select(c =>
                $"{c.Brand} ****{c.Last4} exp {c.ExpMonth}/{c.ExpYear}"
            ));

        var historyText = string.Join(", ", history.Select(h =>
            $"{h.Amount} {h.Currency} {h.Status} {h.Description}"
        ));

        var userContext = $@"
            User: {user.FullName}
            Email: {user.Email}
            Cards: {cardsText}
            Transactions: {historyText}";

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _settings.ApiKey);

        var body = PromptBuilder.Build(userContext, message);

        var response = await client.PostAsync("https://router.huggingface.co/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        );

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<AiResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result == null || result.Choices.Count == 0)
        {
            return "No AI response";
        }

        var responseMessage = result.Choices[0]
            .Message
            .Content ?? "Empty response";

        return responseMessage;
    }
}