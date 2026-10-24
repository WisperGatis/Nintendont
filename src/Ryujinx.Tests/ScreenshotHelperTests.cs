using NUnit.Framework;
using Ryujinx.UI.Common.Helper;
using System;

namespace Ryujinx.Tests
{
    public class ScreenshotHelperTests
    {
        [Test]
        public void BuildFileName_FormatsTimestamp()
        {
            Assert.AreEqual("Game_2026-03-04_05-06-07.png", ScreenshotHelper.BuildFileName("Game", new DateTime(2026, 3, 4, 5, 6, 7)));
        }

        [Test]
        public void BuildFileName_RemovesIllegalCharacters()
        {
            string name = ScreenshotHelper.BuildFileName("Zelda: Breath/of*the?Wild", new DateTime(2026, 1, 1));

            Assert.That(name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()), Is.EqualTo(-1));
            Assert.That(name, Does.EndWith("_2026-01-01_00-00-00.png"));
        }
    }
}
