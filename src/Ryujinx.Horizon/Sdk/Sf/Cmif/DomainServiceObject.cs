using System.Collections.Generic;

namespace Ryujinx.Horizon.Sdk.Sf.Cmif
{
    abstract partial class DomainServiceObject : ServerDomainBase, IServiceObject
    {
        public abstract ServerDomainBase GetServerDomain();

        public IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers()
        {
            return new Dictionary<int, CommandHandler>().AsReadOnly();
        }
    }
}
