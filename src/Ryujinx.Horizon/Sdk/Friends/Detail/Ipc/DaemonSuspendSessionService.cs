using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
namespace Ryujinx.Horizon.Sdk.Friends.Detail.Ipc
{
    partial class DaemonSuspendSessionService : IDaemonSuspendSessionService
    {
        // NOTE: This service has no commands.
    public IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers()
    {
        return new Dictionary<int, CommandHandler>().AsReadOnly();
    }
    }
}
