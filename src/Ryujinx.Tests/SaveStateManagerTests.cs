using NUnit.Framework;
using Ryujinx.HLE.SaveStates;
using Ryujinx.UI.App.Common;
using System;
using System.IO;
using System.Linq;

namespace Ryujinx.Tests
{
    public class SaveStateManagerTests
    {
        private const string TitleId = "0100000000001000";

        private sealed class FakeBackend : ISaveStateBackend
        {
            public byte[] State = { 1, 2, 3, 4 };
            public byte[] Loaded;
            public bool FailSave;

            public bool IsSupported => true;

            public bool TrySave(Stream output)
            {
                if (FailSave)
                {
                    output.WriteByte(0xFF);

                    return false;
                }

                output.Write(State);

                return true;
            }

            public bool TryLoad(Stream input)
            {
                using MemoryStream copy = new();
                input.CopyTo(copy);
                Loaded = copy.ToArray();

                return true;
            }
        }

        private string _dir;
        private FakeBackend _backend;

        [SetUp]
        public void Setup()
        {
            _dir = Path.Combine(Path.GetTempPath(), "ryu-savestate-" + Guid.NewGuid());
            _backend = new FakeBackend();
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_dir))
            {
                Directory.Delete(_dir, true);
            }
        }

        private SaveStateManager Create(string version = "1.0") => new(_dir, version, _backend);

        [Test]
        public void SaveLoad_RoundTripsPayloadAndMetadata()
        {
            SaveStateManager manager = Create();

            Assert.IsTrue(manager.Save(TitleId, 3, TimeSpan.FromMinutes(7), new byte[] { 9, 9 }));
            Assert.IsTrue(manager.Load(TitleId, 3));
            Assert.AreEqual(_backend.State, _backend.Loaded);

            SaveStateInfo info = manager.List(TitleId).Single();

            Assert.AreEqual(3, info.Slot);
            Assert.AreEqual(TimeSpan.FromMinutes(7), info.PlayTime);
            Assert.IsNotNull(info.ThumbnailPath);
        }

        [Test]
        public void Load_RejectsDifferentEmulatorVersion()
        {
            Assert.IsTrue(Create("1.0").Save(TitleId, 1, TimeSpan.Zero, null));

            SaveStateManager other = Create("2.0");

            Assert.IsFalse(other.Load(TitleId, 1));
            Assert.IsEmpty(other.List(TitleId));
        }

        [Test]
        public void FailedSave_KeepsPreviousSlot()
        {
            SaveStateManager manager = Create();

            Assert.IsTrue(manager.Save(TitleId, 1, TimeSpan.FromMinutes(1), null));

            _backend.FailSave = true;

            Assert.IsFalse(manager.Save(TitleId, 1, TimeSpan.FromMinutes(2), null));
            Assert.AreEqual(TimeSpan.FromMinutes(1), manager.List(TitleId).Single().PlayTime);
            Assert.IsEmpty(Directory.GetFiles(Path.Combine(_dir, TitleId, "savestates"), "*.tmp"));
        }

        [Test]
        public void List_SkipsCorruptFilesAndAllowsMissingThumbnail()
        {
            SaveStateManager manager = Create();

            manager.Save(TitleId, 2, TimeSpan.Zero, null);
            File.WriteAllBytes(Path.Combine(_dir, TitleId, "savestates", "slot4.rsv"), new byte[] { 1, 2, 3 });

            SaveStateInfo info = manager.List(TitleId).Single();

            Assert.AreEqual(2, info.Slot);
            Assert.IsNull(info.ThumbnailPath);
        }

        [Test]
        public void Delete_RemovesSlotAndThumbnail()
        {
            SaveStateManager manager = Create();

            manager.Save(TitleId, 5, TimeSpan.Zero, new byte[] { 1 });

            Assert.IsTrue(manager.Delete(TitleId, 5));
            Assert.IsEmpty(manager.List(TitleId));
            Assert.IsFalse(manager.Delete(TitleId, 5));
        }

        [Test]
        public void NullBackend_IsUnsupportedAndRefusesEverything()
        {
            SaveStateManager manager = new(_dir, "1.0", new NullSaveStateBackend());

            Assert.IsFalse(manager.IsSupported);
            Assert.IsFalse(manager.Save(TitleId, 1, TimeSpan.Zero, null));
            Assert.IsFalse(manager.Load(TitleId, 1));
        }

        [Test]
        public void InvalidSlot_IsRejected()
        {
            SaveStateManager manager = Create();

            Assert.IsFalse(manager.Save(TitleId, 10, TimeSpan.Zero, null));
            Assert.IsFalse(manager.Save(TitleId, -1, TimeSpan.Zero, null));
        }
    }
}
