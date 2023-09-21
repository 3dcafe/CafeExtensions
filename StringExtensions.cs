namespace CafeExtensions
{
	/// <summary>
	/// Расширения для работы со строками
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Проверка на null или пустоту
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsEmpty(this string s) => s == null || s.Trim().Length == 0;
		/// <summary>
		/// Проверка нахождение на null или пустоту
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsNotEmpty(this string s) => s != null && s.Trim().Length > 0;
		/// <summary>
		/// Конвертация в enum
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="s"></param>
		/// <returns></returns>
		public static object? ToEnum<TResult>(this string s) where TResult : System.Enum
		{
			if (s.IsNotEmpty() && System.Enum.TryParse(typeof(TResult), s, true, out object? value))
			{
				if (value == null)
					return null;
				if (System.Enum.IsDefined(typeof(TResult), value))
				{
					return value;
				}
			}
			return null;
		}
		/// <summary>
		/// Text to base 64
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string Base64Encode(this string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		/// <summary>
		/// Decode text from base 64
		/// </summary>
		/// <param name="base64EncodedData"></param>
		/// <returns></returns>
		public static string Base64Decode(this string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
		/// <summary>
		/// To Pascal Words to string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToPascalWords(this string value)
		{
			char[] array = value.ToCharArray();
			// Handle the first letter in the string.
			if (array.Length >= 1)
			{
				if (char.IsLower(array[0]))
				{
					array[0] = char.ToUpper(array[0]);
				}
			}
			// Scan through the letters, checking for spaces.
			// Uppercase the lowercase letters following spaces.
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i - 1] == ' ')
				{
					if (char.IsLower(array[i]))
					{
						array[i] = char.ToUpper(array[i]);
					}
				}
			}
			return new string(array);
		}

        /// <summary>
        /// Convert text to Base64
		/// this method using for 1C system
        /// </summary>
        /// <param name="base64Url"></param>
        /// <returns></returns>
        public static byte[] ConvertToBase64(this string base64Url)
        {
            string padded = base64Url.Length % 4 == 0
                ? base64Url
                : base64Url + "===="[(base64Url.Length % 4)..];
            string base64 = padded.Replace("_", "/")
                                  .Replace("-", "+");

            return Convert.FromBase64String(base64);
        }

        public static string CreateMD5(string input)
		{
			using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
			{
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);
				return Convert.ToHexString(hashBytes); 
			}
		}
	}
}
