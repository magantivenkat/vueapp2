using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GoRegister.ApplicationCore.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateRandomString(int passwordSize, bool useLowercase = true, bool useUppercase = true, bool useNumbers = true, bool useSpecial = true)
        {
            return GenerateRandomString("", passwordSize, useLowercase, useUppercase, useNumbers, useSpecial);
        }

        public static string GenerateRandomString(this string value,
            int passwordSize, bool useLowercase = true, bool useUppercase = true, bool useNumbers = true, bool useSpecial = true)
        {
            const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMBERS = "123456789";
            const string SPECIALS = @"!@$#";

            char[] _password = new char[passwordSize];
            string charSet = ""; // Initialise to blank
            System.Random _random = new Random();
            int counter;

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }
            var pass = String.Join(null, _password);
            var result = value += pass;

            return result; // ;
        }

        public static string CombineWithSlash(params string[] segments)
        {
            return string.Join("/", segments.Select(s => s.Trim('/')));
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}
