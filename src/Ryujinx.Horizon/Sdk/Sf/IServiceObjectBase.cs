using System.Collections.Generic;

namespace Ryujinx.Horizon.Sdk.Sf
{
    /// <summary>
    /// Base class for service objects that provides a default implementation of IServiceObject.
    /// </summary>
    internal class ServiceObjectBase : IServiceObject
    {
        /// <summary>
        /// Gets the command handlers for this service object.
        /// </summary>
        /// <returns>An empty dictionary of command handlers by default.</returns>
        public virtual IReadOnlyDictionary<int, CommandHandler> GetCommandHandlers()
        {
            return ServiceObjectUtils.GetEmptyHandlers();
        }
    }
} 
