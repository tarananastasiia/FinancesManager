using Application.DTOs.ResponseModel.Payment;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public interface IPaymentService
    {
        Task<List<CardResponse>> GetCards(string customerId);
        Task<List<PaymentHistoryResponse>> GetHistory(string customerId);
    }

    public class PaymentService : IPaymentService
    {
        public async Task<List<CardResponse>> GetCards(string customerId)
        {
            var service = new PaymentMethodService();

            var methods = await service.ListAsync(new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card"
            });

            return methods.Data.Select(pm => new CardResponse
            {
                Brand = pm.Card.Brand,
                Last4 = pm.Card.Last4,
                ExpMonth = pm.Card.ExpMonth,
                ExpYear = pm.Card.ExpYear
            }).ToList();
        }

        public async Task<List<PaymentHistoryResponse>> GetHistory(string customerId)
        {
            var service = new PaymentIntentService();

            var payments = await service.ListAsync(new PaymentIntentListOptions
            {
                Customer = customerId,
                Limit = 20
            });

            return payments.Data.Select(p => new PaymentHistoryResponse
            {
                Id = p.Id,
                Amount = p.Amount / 100.0,
                Currency = p.Currency,
                Status = p.Status,
                Date = DateTimeOffset.FromUnixTimeSeconds(p.Created.Second).UtcDateTime
            }).ToList();
        }
    }
}
