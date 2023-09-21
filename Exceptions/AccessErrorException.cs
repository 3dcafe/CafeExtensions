namespace CafeExtensions.Exceptions;
/// <summary>
/// Access Error Exception
/// </summary>
[Serializable]
public class AccessErrorException : Exception
{
    /// <summary>
    /// Base with text
    /// </summary>
    /// <param name="message"></param>
    public AccessErrorException(string message) : base(message) { }
}