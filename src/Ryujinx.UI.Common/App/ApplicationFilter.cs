using System;
using System.Globalization;
using System.Linq;

namespace Ryujinx.UI.App.Common
{
    public enum QuickFilter
    {
        All,
        Favorites,
        Recent,
        NeverPlayed,
    }

    public static class ApplicationFilter
    {
        public static readonly TimeSpan RecentWindow = TimeSpan.FromDays(14);

        /// <summary>
        /// Whether <paramref name="app"/> passes the search text, the quick filter and the optional tag filter.
        /// </summary>
        public static bool Matches(ApplicationData app, string searchText, QuickFilter filter, string tag, DateTime nowUtc)
        {
            if (app == null)
            {
                return false;
            }

            bool quickMatch = filter switch
            {
                QuickFilter.Favorites => app.Favorite,
                QuickFilter.Recent => app.LastPlayed.HasValue && nowUtc - app.LastPlayed.Value <= RecentWindow,
                QuickFilter.NeverPlayed => !app.LastPlayed.HasValue,
                _ => true,
            };

            if (!quickMatch)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(tag) && !HasTag(app, tag))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
            const CompareOptions Options = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;

            return compareInfo.IndexOf(app.Name ?? string.Empty, searchText, Options) >= 0 ||
                   (app.Notes != null && compareInfo.IndexOf(app.Notes, searchText, Options) >= 0) ||
                   (app.Tags?.Any(appTag => compareInfo.IndexOf(appTag, searchText, Options) >= 0) ?? false);
        }

        private static bool HasTag(ApplicationData app, string tag) =>
            app.Tags?.Any(appTag => string.Equals(appTag, tag, StringComparison.OrdinalIgnoreCase)) ?? false;
    }
}
