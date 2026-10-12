using System.IO;

namespace Ryujinx.HLE.SaveStates
{
    /// <summary>
    /// Serializes and restores the state of a paused emulation session.
    /// </summary>
    public interface ISaveStateBackend
    {
        /// <summary>
        /// Whether this backend can actually save and restore a running game.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Writes the current emulation state to <paramref name="output"/>. The emulation must be paused.
        /// </summary>
        bool TrySave(Stream output);

        /// <summary>
        /// Restores a state previously written by <see cref="TrySave"/>. The emulation must be paused.
        /// </summary>
        bool TryLoad(Stream input);
    }
}
