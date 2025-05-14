using System.Collections.Generic;

namespace Ryujinx.Horizon.Sdk.Sf
{
    interface IServiceObject
    {
        public IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers();
    }

    static class ServiceObjectUtils
    {
        public static IReadOnlyDictionary<int, CommandHandler> GetEmptyHandlers()
        {
            return new Dictionary<int, CommandHandler>().AsReadOnly();
        }
    }
}
