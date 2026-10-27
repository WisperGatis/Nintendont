using NUnit.Framework;
using Ryujinx.UI.App.Common;

namespace Ryujinx.Tests
{
    public class CheatFilterTests
    {
        [Test]
        public void EmptySearch_MatchesEverything()
        {
            Assert.IsTrue(CheatFilter.Matches("Infinite Health", ""));
            Assert.IsTrue(CheatFilter.Matches("Infinite Health", "   "));
            Assert.IsTrue(CheatFilter.Matches("Infinite Health", null));
        }

        [Test]
        public void Search_IsCaseAndAccentInsensitive()
        {
            Assert.IsTrue(CheatFilter.Matches("Infinite Health", "HEALTH"));
            Assert.IsTrue(CheatFilter.Matches("Pokémon Always Shiny", "pokemon"));
        }

        [Test]
        public void NoMatch_ReturnsFalse()
        {
            Assert.IsFalse(CheatFilter.Matches("Infinite Health", "ammo"));
            Assert.IsFalse(CheatFilter.Matches(null, "ammo"));
        }
    }
}
