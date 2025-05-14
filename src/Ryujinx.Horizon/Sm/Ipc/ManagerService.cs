using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using Ryujinx.Horizon.Sdk.Sm;

namespace Ryujinx.Horizon.Sm.Ipc
{
    partial class ManagerService : IManagerService
    {
    public IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers()
    {
        return new Dictionary<int, CommandHandler>().AsReadOnly();
    }
    }
}
