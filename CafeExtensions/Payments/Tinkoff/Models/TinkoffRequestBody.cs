using MorePayments.Payment.Tinkoff.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MorePayments.Payment.Tinkoff.Models
{
    public class TinkoffRequestBody
    {
        [Required]
        [JsonPropertyName("TerminalKey")]
        public string TerminalKey { get; set; }

        [JsonPropertyName("Amount")]
        public int Amount { get; set; }

        [JsonPropertyName("OrderId")]
        [Required]
        public string OrderId { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("PayType")]
        public string PayType { get; set; } = "O";

        [JsonPropertyName("Token")]
        [IgnoreTokenCalculate]
        public string Token { get; set; }

        [IgnoreTokenCalculate]
        [JsonPropertyName("DATA")]
        public Dictionary<string, string> Data { get; set; }

        [JsonPropertyName("Receipt")]
        [IgnoreTokenCalculate]
        public Receipt Receipt { get; set; }

        [JsonPropertyName("NotificationURL")]
        public string NotificationURL { get; set; }
        [JsonPropertyName("SuccessURL")]
        public string SuccessURL { get; set; }
        [JsonPropertyName("FailURL")]
        public string FailURL { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Price")]
        public int Price { get; set; }

        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("Amount")]
        public int Amount => Price * Quantity;

        [JsonPropertyName("Tax")]
        public string? Tax { get; set; }

        [JsonPropertyName("Ean13")]
        public string? Ean13 { get; set; }
    }

    public class Receipt
    {
        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("Phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("Taxation")]
        public string Taxation { get; set; }

        [JsonPropertyName("Items")]
        public List<Item> Items { get; set; }
    }
}
