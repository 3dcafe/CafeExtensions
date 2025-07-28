namespace CafeExtensions.SimpleModels;
/// <summary>
/// Type register user
/// </summary>
public enum TypeRegister
{
    /// <summary>
    /// Phone number
    /// </summary>
    Phone = 0,
    /// <summary>
    /// Email
    /// </summary>
    Email = 1,
    /// <summary>
    /// Social media
    /// </summary>
    Social = 2,
    /// <summary>
    /// Login and password from an external or local system
    /// </summary>
    LoginAndPassword = 3,
    /// <summary>
    /// Login from web qird widget
    /// </summary>
    WebWidget = 4
}