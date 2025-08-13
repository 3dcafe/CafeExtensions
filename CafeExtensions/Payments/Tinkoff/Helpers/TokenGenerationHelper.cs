using MorePayments.Payment.Tinkoff.Attributes;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace MorePayments.Payment.Tinkoff.Helpers
{
    public class TokenGenerationHelper
    {
        public static string GenerateToken<T>(T model, string password) where T : class
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password is empty");
            }
            var properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var pairs = new Dictionary<string, object>() {{ "Password", password }};

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<IgnoreTokenCalculateAttribute>();
                if (attribute != null)
                {
                    continue;
                }
                var value = property.GetValue(model);
                var type = property.PropertyType;
                object? defaultValue = null;
                if (object.Equals(value, defaultValue))
                {
                    continue;
                }
                var propValue = property.GetValue(model);
                if (type == typeof(bool) || type == typeof(bool?))
                {
                    propValue = (propValue + "").ToLower();
                }
                pairs[property.Name] = propValue ?? string.Empty;
            }

            var strValues = pairs.OrderBy(x => x.Key).Select(x => x.Value).Aggregate((x, y) => x + "" + y) + "";
            return CalculateHash256(strValues);
        }

        public static string GenerateToken(Dictionary<string, string> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var strValues = model.OrderBy(x => x.Key).Select(x => x.Value).Aggregate((x, y) => x + "" + y) + "";
            return CalculateHash256(strValues);
        }

        public static string GenerateTokenBase64(Dictionary<string, string> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var strValues = model.OrderBy(x => x.Key).Select(x => x.Value).Aggregate((x, y) => x + "" + y) + "";
            var bytes = Encoding.UTF8.GetBytes(strValues);
            using (SHA256 mySHA256 = SHA256.Create())
            {
                var hashBytes = mySHA256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private static string CalculateHash256(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            using (SHA256 mySHA256 = SHA256.Create())
            {
                var hashBytes = mySHA256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes, 0).Replace("-", "").ToLower();
            }
        }
    }
}
