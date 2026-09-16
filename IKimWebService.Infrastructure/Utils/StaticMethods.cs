using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.Utils
{
    public class StaticMethods
    {
        public static string[] SplitString(string inputtedString, char separation)
        {
            if (inputtedString.IsNullOrEmpty())
                return null;

            return inputtedString.Trim().Split(separation);
        }

        public static string GeneratePassword(int length = 12, bool useUppercase = true, bool useLowercase = true, bool useNumbers = true, bool useSpecial = true)
        {
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

            StringBuilder charSet = new StringBuilder();
            if (useUppercase) charSet.Append(uppercase);
            if (useLowercase) charSet.Append(lowercase);
            if (useNumbers) charSet.Append(numbers);
            if (useSpecial) charSet.Append(special);

            if (charSet.Length == 0)
                throw new ArgumentException("At least one character set must be enabled.");

            Random rng = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => charSet[rng.Next(charSet.Length)])
                .ToArray());
        }
    }
}
