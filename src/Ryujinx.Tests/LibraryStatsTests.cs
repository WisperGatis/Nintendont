using NUnit.Framework;
using Ryujinx.UI.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ryujinx.Tests
{
    public class LibraryStatsTests
    {
        private static readonly DateTime _now = new(2026, 10, 13, 12, 0, 0, DateTimeKind.Utc);

        private static ApplicationData App(string name, int hours = 0, int daysAgo = -1, params string[] tags) => new()
        {
            Name = name,
            TimePlayed = TimeSpan.FromHours(hours),
            LastPlayed = daysAgo >= 0 ? _now - TimeSpan.FromDays(daysAgo) : null,
            Tags = tags.ToList(),
        };

        [Test]
        public void EmptyLibrary_IsAllZeros()
        {
            LibraryStats stats = LibraryStats.Compute(new List<ApplicationData>());

            Assert.AreEqual(0, stats.GameCount);
            Assert.AreEqual(TimeSpan.Zero, stats.AveragePlayTime);
            Assert.IsEmpty(stats.MostPlayed);
            Assert.IsNull(stats.TopTag);
        }

        [Test]
        public void CountsTotalsAndAverage()
        {
            LibraryStats stats = LibraryStats.Compute(new[] { App("A", 10, 1), App("B", 20, 2), App("C") });

            Assert.AreEqual(3, stats.GameCount);
            Assert.AreEqual(2, stats.PlayedCount);
            Assert.AreEqual(TimeSpan.FromHours(30), stats.TotalPlayTime);
            Assert.AreEqual(TimeSpan.FromHours(15), stats.AveragePlayTime);
        }

        [Test]
        public void MostPlayed_OrdersByTimeThenName_AndLimits()
        {
            LibraryStats stats = LibraryStats.Compute(new[] { App("B", 5, 1), App("A", 5, 1), App("C", 9, 1), App("D", 1, 1) }, 3);

            Assert.AreEqual(new[] { "C", "A", "B" }, stats.MostPlayed.Select(app => app.Name).ToArray());
        }

        [Test]
        public void RecentlyPlayed_NewestFirst()
        {
            LibraryStats stats = LibraryStats.Compute(new[] { App("Old", 1, 10), App("New", 1, 1), App("Never") });

            Assert.AreEqual(new[] { "New", "Old" }, stats.RecentlyPlayed.Select(app => app.Name).ToArray());
        }

        [Test]
        public void TopTag_IsCaseInsensitive()
        {
            LibraryStats stats = LibraryStats.Compute(new[] { App("A", tags: "RPG"), App("B", tags: "rpg"), App("C", tags: "Racing") });

            Assert.AreEqual("rpg", stats.TopTag.ToLowerInvariant());
        }

        [Test]
        public void FormatTotal_UsesTotalHours()
        {
            Assert.AreEqual("50h 05m", LibraryStats.FormatTotal(TimeSpan.FromMinutes(50 * 60 + 5)));
            Assert.AreEqual("0h 00m", LibraryStats.FormatTotal(TimeSpan.Zero));
        }
    }
}
