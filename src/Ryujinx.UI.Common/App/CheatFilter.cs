using System.Globalization;

namespace Ryujinx.UI.App.Common
{
    public static class CheatFilter
    {
        /// <summary>
        /// Whether a cheat name matches the search text (case and accent insensitive). Empty search matches everything.
        /// </summary>
        public static bool Matches(string cheatName, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(
                cheatName ?? string.Empty,
                searchText.Trim(),
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }
    }
}
