namespace CafeExtensions.Interfaces;
/// <summary>
/// Base interface for all db models EF
/// </summary>
public interface IBase
{
    public DateTimeOffset DateAdd { get; set; }
    public DateTimeOffset DateUpdate { get; set; }
    public bool IsDeleted { get; set; }
}