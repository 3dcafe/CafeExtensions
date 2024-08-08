using CafeExtensions.Repositories;
using MorePayments.Payment.Tinkoff.Helpers;
using MorePayments.Payment.Tinkoff.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MorePayments.Payment.Tinkoff
{
    public class TinkoffService
    {
        private readonly string TERMINAL_KEY;
        private readonly string TERMINAL_PASS;

        private readonly string _successUrl;
        private readonly string _failUrl;
        private readonly string _notificationUrl;

        private const string PAY_URL = "https://securepay.tinkoff.ru/v2/Init";

        public TinkoffService(string key, string password, string successUrl, string failUrl, string notificationUrl)
        {
            TERMINAL_KEY = key;
            TERMINAL_PASS = password;
            _successUrl = successUrl;
            _failUrl = failUrl;
            _notificationUrl = notificationUrl;
        }

        public async Task<TinkoffResponse?> InitAsync(uint price, string description, string phoneNumber, string orderId)
        {
            TinkoffRequestBody body = new()
            {
                OrderId = orderId,
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
                NotificationURL = _notificationUrl,
                FailURL = _failUrl,
                SuccessURL = _successUrl,
            };

            body.Token = TokenGenerationHelper.GenerateToken(body, TERMINAL_PASS);

            var http = new HttpSimpleClientRepository();

            var res = await http.PostAsync<TinkoffResponse>(PAY_URL, body);

            if (res == null || res.Response == null) return null;

            return res.Response;
        }
    }
}
