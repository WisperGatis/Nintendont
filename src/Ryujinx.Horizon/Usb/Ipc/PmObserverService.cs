using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using Ryujinx.Horizon.Sdk.Usb;

namespace Ryujinx.Horizon.Usb.Ipc
{
    partial class PmObserverService : IPmObserverService
    {
    public IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers()
    {
        return new Dictionary<int, CommandHandler>().AsReadOnly();
    }
    }
}
