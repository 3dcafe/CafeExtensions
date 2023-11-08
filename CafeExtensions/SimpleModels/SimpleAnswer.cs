namespace CafeExtensions.SimpleModels;
/// <summary>
/// Simple answer response
/// </summary>
public class SimpleAnswer
{
    /// <summary>
    /// Base contructor
    /// </summary>
    public SimpleAnswer() { }
    /// <summary>
    /// SimpleAnswer
    /// </summary>
    /// <param name="state"></param>
    /// <param name="error"></param>
    public SimpleAnswer(bool state, string error)
    {
        State = state;
        Error = error;
    }
    /// <summary>
    /// State
    /// </summary>
    public bool State { get; set; } = false;
    /// <summary>
    /// Error text
    /// </summary>
    public string Error { get; set; } = string.Empty;
}