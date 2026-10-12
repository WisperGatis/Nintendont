using System.IO;

namespace Ryujinx.HLE.SaveStates
{
    /// <summary>
    /// Placeholder backend used until a real snapshot implementation exists.
    /// </summary>
    public sealed class NullSaveStateBackend : ISaveStateBackend
    {
        public bool IsSupported => false;

        public bool TrySave(Stream output) => false;

        public bool TryLoad(Stream input) => false;
    }
}
