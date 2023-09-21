using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeExtensions.Attributes;

/// <summary>
/// Date range validation attribute
/// </summary>
public class DateRangeValidationAttribute : RangeAttribute
{
	/// <summary>
	/// Base
	/// </summary>
	public DateRangeValidationAttribute() : base(typeof(DateTime),
		DateTime.Now.AddYears(-108).ToShortDateString(),
		DateTime.Now.AddYears(-12).ToShortDateString())
	{ }
}
