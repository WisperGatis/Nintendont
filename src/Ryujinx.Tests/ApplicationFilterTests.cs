using NUnit.Framework;
using Ryujinx.UI.App.Common;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ryujinx.Tests
{
    public class ApplicationFilterTests
    {
        private static readonly DateTime _now = new(2026, 10, 13, 12, 0, 0, DateTimeKind.Utc);

        private static ApplicationData App(string name, bool favorite = false, DateTime? lastPlayed = null, params string[] tags) => new()
        {
            Name = name,
            Favorite = favorite,
            LastPlayed = lastPlayed,
            Tags = new List<string>(tags),
        };

        [Test]
        public void Search_IsCaseAndAccentInsensitive()
        {
            Assert.IsTrue(ApplicationFilter.Matches(App("Pokémon"), "pokemon", QuickFilter.All, null, _now));
            Assert.IsFalse(ApplicationFilter.Matches(App("Zelda"), "mario", QuickFilter.All, null, _now));
        }

        [Test]
        public void Search_AlsoMatchesTags()
        {
            Assert.IsTrue(ApplicationFilter.Matches(App("Game", tags: "Co-op"), "co-", QuickFilter.All, null, _now));
        }

        [Test]
        public void Search_AlsoMatchesNotes()
        {
            ApplicationData app = App("Game");
            app.Notes = "Needs the Vulkan backend";

            Assert.IsTrue(ApplicationFilter.Matches(app, "vulkan", QuickFilter.All, null, _now));
            Assert.IsFalse(ApplicationFilter.Matches(App("Game"), "vulkan", QuickFilter.All, null, _now));
        }

        [Test]
        public void SetNotes_TrimsEmptiesAndCaps()
        {
            ApplicationMetadata metadata = new();

            metadata.SetNotes("  hello\nworld  ");
            Assert.AreEqual("hello\nworld", metadata.Notes);

            metadata.SetNotes(new string('x', 5000));
            Assert.AreEqual(ApplicationMetadata.MaxNotesLength, metadata.Notes.Length);

            metadata.SetNotes("   ");
            Assert.IsNull(metadata.Notes);
        }

        [Test]
        public void Notes_OmittedFromJsonWhenNull()
        {
            Assert.IsFalse(JsonSerializer.Serialize(new ApplicationMetadata()).Contains("\"notes\""));
            Assert.IsNull(JsonSerializer.Deserialize<ApplicationMetadata>("{}").Notes);
        }

        [Test]
        public void EmptySearch_MatchesEverything()
        {
            Assert.IsTrue(ApplicationFilter.Matches(App("Game"), "  ", QuickFilter.All, null, _now));
        }

        [Test]
        public void Favorites_OnlyFavorites()
        {
            Assert.IsTrue(ApplicationFilter.Matches(App("A", favorite: true), null, QuickFilter.Favorites, null, _now));
            Assert.IsFalse(ApplicationFilter.Matches(App("A"), null, QuickFilter.Favorites, null, _now));
        }

        [Test]
        public void Recent_UsesFourteenDayWindow()
        {
            Assert.IsTrue(ApplicationFilter.Matches(App("A", lastPlayed: _now - TimeSpan.FromDays(14)), null, QuickFilter.Recent, null, _now));
            Assert.IsFalse(ApplicationFilter.Matches(App("A", lastPlayed: _now - TimeSpan.FromDays(15)), null, QuickFilter.Recent, null, _now));
            Assert.IsFalse(ApplicationFilter.Matches(App("A"), null, QuickFilter.Recent, null, _now));
        }

        [Test]
        public void NeverPlayed_OnlyWithoutLastPlayed()
        {
            Assert.IsTrue(ApplicationFilter.Matches(App("A"), null, QuickFilter.NeverPlayed, null, _now));
            Assert.IsFalse(ApplicationFilter.Matches(App("A", lastPlayed: _now), null, QuickFilter.NeverPlayed, null, _now));
        }

        [Test]
        public void TagFilter_IsExactAndCaseInsensitive()
        {
            Assert.IsTrue(ApplicationFilter.Matches(App("A", tags: "Racing"), null, QuickFilter.All, "racing", _now));
            Assert.IsFalse(ApplicationFilter.Matches(App("A", tags: "Racing"), null, QuickFilter.All, "rac", _now));
        }

        [Test]
        public void SetTags_NormalizesAndOmitsWhenEmpty()
        {
            ApplicationMetadata metadata = new();

            metadata.SetTags(new[] { " Co-op ", "co-op", "", "  ", "RPG", new string('x', 100) });

            Assert.AreEqual(3, metadata.Tags.Count);
            Assert.AreEqual("Co-op", metadata.Tags[0]);
            Assert.AreEqual(ApplicationMetadata.MaxTagLength, metadata.Tags[2].Length);

            metadata.SetTags(new[] { " " });

            Assert.IsNull(metadata.Tags);
        }

        [Test]
        public void Metadata_WithoutTags_StillDeserializes()
        {
            ApplicationMetadata metadata = JsonSerializer.Deserialize<ApplicationMetadata>("{\"favorite\":true}");

            Assert.IsNull(metadata.Tags);
        }
    }
}
