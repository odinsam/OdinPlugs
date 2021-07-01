using System.Text.RegularExpressions;

namespace OdinPlugs.OdinExtensions.RegexExtensions
{
    public static class OdinRegexExtensions
    {
        public static string GetIp4(this string str)
        {
            string pattern = @"\b(([01]?\d?\d|2[0-4]\d|25[0-5])\.){3}([01]?\d?\d|2[0-4]\d|25[0-5])\b";
            Match match = Regex.Match(str, pattern);
            return match.Value;
        }
    }
}