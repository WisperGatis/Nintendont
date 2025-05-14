using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using Ryujinx.Horizon.Sdk.Wlan;

namespace Ryujinx.Horizon.Wlan.Ipc
{
    partial class GeneralServiceCreator : IGeneralServiceCreator
    {
    public IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers()
    {
        return new Dictionary<int, CommandHandler>().AsReadOnly();
    }
    }
}
