using System;
using System.Collections.Generic;
using System.Linq;

namespace Ryujinx.UI.App.Common
{
    public sealed record LibraryStats(
        int GameCount,
        int PlayedCount,
        TimeSpan TotalPlayTime,
        TimeSpan AveragePlayTime,
        IReadOnlyList<ApplicationData> MostPlayed,
        IReadOnlyList<ApplicationData> RecentlyPlayed,
        string TopTag)
    {
        public static LibraryStats Compute(IEnumerable<ApplicationData> apps, int topCount = 5)
        {
            List<ApplicationData> all = apps?.Where(app => app != null).ToList() ?? new List<ApplicationData>();
            List<ApplicationData> played = all.Where(app => app.TimePlayed > TimeSpan.Zero || app.LastPlayed.HasValue).ToList();

            TimeSpan total = TimeSpan.FromTicks(played.Sum(app => app.TimePlayed.Ticks));
            TimeSpan average = played.Count > 0 ? TimeSpan.FromTicks(total.Ticks / played.Count) : TimeSpan.Zero;

            List<ApplicationData> mostPlayed = played
                .Where(app => app.TimePlayed > TimeSpan.Zero)
                .OrderByDescending(app => app.TimePlayed)
                .ThenBy(app => app.Name, StringComparer.CurrentCultureIgnoreCase)
                .Take(topCount)
                .ToList();

            List<ApplicationData> recent = played
                .Where(app => app.LastPlayed.HasValue)
                .OrderByDescending(app => app.LastPlayed.Value)
                .ThenBy(app => app.Name, StringComparer.CurrentCultureIgnoreCase)
                .Take(topCount)
                .ToList();

            string topTag = all
                .SelectMany(app => (app.Tags ?? new List<string>()).Distinct(StringComparer.OrdinalIgnoreCase))
                .GroupBy(tag => tag, StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key, StringComparer.CurrentCultureIgnoreCase)
                .Select(group => group.Key)
                .FirstOrDefault();

            return new LibraryStats(all.Count, played.Count, total, average, mostPlayed, recent, topTag);
        }

        /// <summary>
        /// Formats a duration as total hours and minutes, e.g. <c>50h 05m</c>.
        /// </summary>
        public static string FormatTotal(TimeSpan duration) =>
            $"{(int)duration.TotalHours}h {duration.Minutes:D2}m";
    }
}
