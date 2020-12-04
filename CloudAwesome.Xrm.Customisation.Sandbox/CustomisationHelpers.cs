using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    public static class CustomisationHelpers
    {
        public static Label CreateLabelFromString(string displayString, int languageCode = 1033)
        {
            return new Label(displayString, languageCode);
        }

        public static string CreateLogicalNameFromDisplayName(string displayName, string publisherPrefix, bool isLookupAttribute = false)
        {
            var validNameChars = new Regex("[A-Z0-9]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var result = new StringBuilder();
            result.AppendFormat("{0}_", publisherPrefix);
            foreach (var match in validNameChars.Matches(displayName))
            {
                result.Append(match);
            }

            if (isLookupAttribute && (displayName.Substring(displayName.Length - 2) != "id"))
            {
                result.Append("id");
            }

            return result.ToString().ToLower().Trim();
        }

    }
}
