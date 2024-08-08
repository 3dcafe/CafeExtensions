using CafeExtensions.Repositories;
using MorePayments.Payment.Tinkoff.Helpers;
using MorePayments.Payment.Tinkoff.Models;
using System;
using System.Threading.Tasks;

namespace MorePayments.Payment.Tinkoff
{
    public class TinkoffService
    {
        private readonly string TERMINAL_KEY;
        private readonly string TERMINAL_PASS;
        private const string PAY_URL = "https://securepay.tinkoff.ru/v2/Init";

        public TinkoffService(string key, string password)
        {
            TERMINAL_KEY = key;
            TERMINAL_PASS = password;
        }

        public async Task<TinkoffResponse?> InitAsync(uint price, string description, string phoneNumber, string userId)
        {
            TinkoffRequestBody body = new()
            {
                OrderId = Guid.NewGuid().ToString(),
                Amount = (int)price * 100,
                TerminalKey = TERMINAL_KEY,
                Description = description,
                Data = new(),
                Receipt = new()
                {
                    Taxation = "osn",
                    Phone = phoneNumber,
                    Items = new()
                    {
                        new ()
                        {
                            Name = description,
                            Price = (int)price * 100,
                            Quantity = 1,
                            Tax = "none"
                        }
                    }
                },
                NotificationURL = "https://qird.ru",
                FailURL = $"https://api.qird.ru/api/WebHook/confirm-record?userId={userId}",
                SuccessURL = $"https://api.qird.ru/api/WebHook/confirm-record?userId={userId}",
            };

            body.Token = TokenGenerationHelper.GenerateToken(body, TERMINAL_PASS);

            var http = new HttpSimpleClientRepository();

            var res = await http.PostAsync<TinkoffResponse>(PAY_URL, body);

            if (res == null || res.Response == null) return null;

            return res.Response;
        }
    }
}
