namespace CafeExtensions.Interfaces;
/// <summary>
/// Base interface user
/// </summary>
public interface IUser
{
    public string? Id { get; set; }
    /// <summary>
    /// Surname user
    /// </summary>
    public string? Surname { get; set; }
    /// <summary>
    /// FirstName
    /// </summary>
    public string? FirstName { get; set; }
    /// <summary>
    /// Patronymic
    /// </summary>
    public string? Patronymic { get; set; }
    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime? DateBirth { get; set; }
    /// <summary>
    /// Phone Number
    /// </summary>
    public string? PhoneNumber { get; set; }
}