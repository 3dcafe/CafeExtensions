namespace MorePayments.Payment.Tinkoff.Models
{
    public class TinkoffResponse
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        /// <summary>
        /// Краткое описание ошибки
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Подробное описание ошибки	
        /// </summary>
        public string? Details { get; set; }
        public string TerminalKey { get; set; }
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public int Amount { get; set; }
        public string PaymentURL { get; set; }
    }
}
