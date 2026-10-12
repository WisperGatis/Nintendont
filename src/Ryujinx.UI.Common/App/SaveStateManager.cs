using Ryujinx.Common.Logging;
using Ryujinx.HLE.SaveStates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ryujinx.UI.App.Common
{
    public sealed record SaveStateInfo(int Slot, DateTime SavedUtc, TimeSpan PlayTime, string EmulatorVersion, string ThumbnailPath);

    /// <summary>
    /// Manages per-game save state slots on disk: <c>slot&lt;N&gt;.rsv</c> plus an optional <c>slot&lt;N&gt;.png</c> thumbnail.
    /// Slot 0 is reserved for the automatic slot, slots 1 to <see cref="SlotCount"/> are manual.
    /// </summary>
    public sealed class SaveStateManager
    {
        public const int SlotCount = 9;
        public const int AutoSlot = 0;

        private const uint Magic = 0x53565352; // "RSVS"
        private const uint FormatVersion = 1;

        private readonly string _gamesDirectory;
        private readonly string _emulatorVersion;
        private readonly ISaveStateBackend _backend;

        public SaveStateManager(string gamesDirectory, string emulatorVersion, ISaveStateBackend backend)
        {
            _gamesDirectory = gamesDirectory;
            _emulatorVersion = emulatorVersion;
            _backend = backend ?? new NullSaveStateBackend();
        }

        public bool IsSupported => _backend.IsSupported;

        private string SlotPath(string titleId, int slot, string extension) =>
            Path.Combine(_gamesDirectory, titleId, "savestates", $"slot{slot}{extension}");

        private static bool IsValidSlot(int slot) => slot is >= AutoSlot and <= SlotCount;

        public bool Save(string titleId, int slot, TimeSpan playTime, byte[] thumbnailPng)
        {
            if (!IsSupported || !IsValidSlot(slot))
            {
                return false;
            }

            string path = SlotPath(titleId, slot, ".rsv");
            string tempPath = path + ".tmp";

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using (FileStream stream = new(tempPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    long lengthPosition;

                    using (BinaryWriter writer = new(stream, Encoding.UTF8, leaveOpen: true))
                    {
                        writer.Write(Magic);
                        writer.Write(FormatVersion);
                        writer.Write(_emulatorVersion);
                        writer.Write(titleId);
                        writer.Write(DateTime.UtcNow.Ticks);
                        writer.Write(playTime.Ticks);
                        lengthPosition = stream.Position;
                        writer.Write(0L);
                    }

                    long payloadStart = stream.Position;

                    if (!_backend.TrySave(stream))
                    {
                        stream.Dispose();
                        File.Delete(tempPath);

                        return false;
                    }

                    long payloadLength = stream.Position - payloadStart;

                    stream.Position = lengthPosition;
                    new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true).Write(payloadLength);
                }

                File.Move(tempPath, path, overwrite: true);

                string thumbnailPath = SlotPath(titleId, slot, ".png");

                if (thumbnailPng is { Length: > 0 })
                {
                    File.WriteAllBytes(thumbnailPath + ".tmp", thumbnailPng);
                    File.Move(thumbnailPath + ".tmp", thumbnailPath, overwrite: true);
                }
                else if (File.Exists(thumbnailPath))
                {
                    File.Delete(thumbnailPath);
                }

                return true;
            }
            catch (Exception exception)
            {
                Logger.Warning?.Print(LogClass.Application, $"Failed to save state for {titleId} slot {slot}: {exception.Message}");

                TryDelete(tempPath);

                return false;
            }
        }

        public bool Load(string titleId, int slot)
        {
            if (!IsSupported || !IsValidSlot(slot))
            {
                return false;
            }

            try
            {
                using FileStream stream = File.OpenRead(SlotPath(titleId, slot, ".rsv"));

                if (ReadHeader(stream, titleId, slot) == null)
                {
                    return false;
                }

                return _backend.TryLoad(stream);
            }
            catch (Exception exception)
            {
                Logger.Warning?.Print(LogClass.Application, $"Failed to load state for {titleId} slot {slot}: {exception.Message}");

                return false;
            }
        }

        public bool Delete(string titleId, int slot)
        {
            if (!IsValidSlot(slot))
            {
                return false;
            }

            string path = SlotPath(titleId, slot, ".rsv");
            bool existed = File.Exists(path);

            TryDelete(path);
            TryDelete(SlotPath(titleId, slot, ".png"));

            return existed;
        }

        /// <summary>
        /// Lists the valid slots for a game. Corrupt or incompatible files are skipped.
        /// </summary>
        public IReadOnlyList<SaveStateInfo> List(string titleId)
        {
            List<SaveStateInfo> result = new();

            for (int slot = AutoSlot; slot <= SlotCount; slot++)
            {
                string path = SlotPath(titleId, slot, ".rsv");

                if (!File.Exists(path))
                {
                    continue;
                }

                try
                {
                    using FileStream stream = File.OpenRead(path);

                    if (ReadHeader(stream, titleId, slot) is { } info)
                    {
                        result.Add(info);
                    }
                }
                catch (Exception)
                {
                    // Unreadable slot, ignore it.
                }
            }

            return result;
        }

        private SaveStateInfo ReadHeader(FileStream stream, string titleId, int slot)
        {
            using BinaryReader reader = new(stream, Encoding.UTF8, leaveOpen: true);

            if (reader.ReadUInt32() != Magic || reader.ReadUInt32() != FormatVersion)
            {
                return null;
            }

            string emulatorVersion = reader.ReadString();
            string storedTitleId = reader.ReadString();
            DateTime saved = new(reader.ReadInt64(), DateTimeKind.Utc);
            TimeSpan playTime = TimeSpan.FromTicks(reader.ReadInt64());
            long payloadLength = reader.ReadInt64();

            if (storedTitleId != titleId || emulatorVersion != _emulatorVersion || payloadLength != stream.Length - stream.Position)
            {
                return null;
            }

            string thumbnail = SlotPath(titleId, slot, ".png");

            return new SaveStateInfo(slot, saved, playTime, emulatorVersion, File.Exists(thumbnail) ? thumbnail : null);
        }

        private static void TryDelete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // Best effort.
            }
        }
    }
}
