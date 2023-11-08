using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeExtensions.Validators;

public static class EmailValidator
{
	public static bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;

		try
		{
			MailAddress m = new(email);
		}
		catch (FormatException)
		{
			return false;
		}

		try
		{
			email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
				RegexOptions.None, TimeSpan.FromMilliseconds(200));
		}
		catch
		{
			return false;
		}

		try
		{
			return Regex.IsMatch(email,
				@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
				RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
		}
		catch
		{
			return false;
		}
	}
	private static string DomainMapper(Match match)
	{
		var idn = new IdnMapping();
		string domainName = idn.GetAscii(match.Groups[2].Value);
		return match.Groups[1].Value + domainName;
	}
}