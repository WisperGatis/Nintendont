using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;

namespace Ryujinx.Horizon.Sdk.Sf
{
    /// <summary>
    /// Provides extension methods for IServiceObject implementations.
    /// </summary>
    internal static class ServiceObjectExtensions
    {
        /// <summary>
        /// Gets an empty command handler dictionary.
        /// </summary>
        /// <param name="serviceObject">The service object.</param>
        /// <returns>An empty command handler dictionary.</returns>
        internal static IReadOnlyDictionary<int, CommandHandler> GetEmptyHandlers(this IServiceObject serviceObject)
        {
            return new Dictionary<int, CommandHandler>().AsReadOnly();
        }
    }
} 
