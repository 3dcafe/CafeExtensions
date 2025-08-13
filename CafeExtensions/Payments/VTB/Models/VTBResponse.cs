namespace CafeExtensions.Payments.VTB.Models;

public class VTBResponse
{
    public string? OrderId { get; set; }
    public string? FormUrl { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}
