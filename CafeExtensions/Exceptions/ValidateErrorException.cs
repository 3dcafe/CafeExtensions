namespace CafeExtensions.Exceptions;
/// <summary>
/// Validate Error Exception
/// </summary>
[Serializable]
public class ValidateErrorException : Exception
{
    /// <summary>
    /// Base with text
    /// </summary>
    /// <param name="message"></param>
    public ValidateErrorException(string message) : base(message) { }
}
