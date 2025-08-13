using CafeExtensions.Interfaces;
using CafeExtensions.Payments.VTB.Models;
using MorePayments.Payment.Tinkoff.Helpers;
using System;
using System.Net.Http.Json;

namespace CafeExtensions.Payments.VTB;

public class VTBService
{
    private readonly string USERNAME;
    private readonly string PASSWORD;

    private readonly string _successUrl;
    private readonly string _failUrl;

    private const string PAY_URL = "https://vtb.rbsuat.com/payment/rest/register.do";

    public VTBService(string login, string password, string successUrl, string failUrl)
    {
        USERNAME = login;
        PASSWORD = password;
        _successUrl = successUrl;
        _failUrl = failUrl;
    }

    public async Task<VTBResponse?> InitAsync(uint price, string description, string phoneNumber, string? email, string orderId)
    {
        var dict = new Dictionary<string, string>()
        {
            {"amount", $"{price}" },
            {"orderNumber", orderId },
            {"returnUrl", _successUrl },
            {"failUrl", _failUrl },
            {"description", description },
            {"userName", USERNAME },
            {"password", PASSWORD }
        };

        if (!string.IsNullOrEmpty(email)) dict.Add("email", email);

        var token = TokenGenerationHelper.GenerateTokenBase64(dict);

        using var client = new HttpClient();

        //client.DefaultRequestHeaders.Add("X-Hash", token);

        using var req = new HttpRequestMessage(HttpMethod.Post, PAY_URL) { Content = new FormUrlEncodedContent(dict) };
        using var res = await client.SendAsync(req);
        var response = await res.Content.ReadFromJsonAsync<VTBResponse?>();

        return response;
    }
}
