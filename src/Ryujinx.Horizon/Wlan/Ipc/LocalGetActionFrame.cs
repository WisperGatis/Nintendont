using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using Ryujinx.Horizon.Sdk.Wlan;

namespace Ryujinx.Horizon.Wlan.Ipc
{
    partial class LocalGetActionFrame : ILocalGetActionFrame
    {
    public IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers()
    {
        return new Dictionary<int, CommandHandler>().AsReadOnly();
    }
    }
}
