using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Extensions
{
    public static class FragmentStringExtensions
    {
        public static FragmentString ToFragmentString(this string fragment)
        {
            if (string.IsNullOrWhiteSpace(fragment)) return default(FragmentString);
            return new FragmentString($"#{fragment}");
        }
    }
}
