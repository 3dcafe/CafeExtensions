using System.ComponentModel.DataAnnotations;

namespace CafeExtensions.Attributes;
/// <summary>
/// Attribute for validating a date within a specific range.
/// </summary>
public class DateRangeValidationAttribute : RangeAttribute
{
    // Constants for minimum and maximum allowed dates.
    private const string MinDate = "01/01/1915";
    private const string MaxDate = "01/01/2035";
    /// <summary>
    /// Initializes a new instance of the DateRangeValidationAttribute class.
    /// default dates min "01/01/1915" and max 01/01/2035
    /// </summary>
    /// <param name="errorMessage">Custom error message to be displayed when validation fails.</param>
    public DateRangeValidationAttribute(string? errorMessage) : base(typeof(DateTime), MinDate, MaxDate)
    {
        if (!string.IsNullOrEmpty(errorMessage))
            ErrorMessage = errorMessage;
    }
    /// <summary>
    /// Overrides the validation method to check if the value falls within the specified date range.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="validationContext">The validation context containing information about the object being validated.</param>
    /// <returns>
    /// A ValidationResult object representing the validation result. Returns ValidationResult.Success
    /// if the value falls within the specified date range or a new ValidationResult object with a custom
    /// error message otherwise.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var baseResult = base.IsValid(value, validationContext);
        if (baseResult == ValidationResult.Success)
            return ValidationResult.Success;
        var errorMessage = !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : baseResult?.ErrorMessage;
        return new ValidationResult(errorMessage);
    }
}
