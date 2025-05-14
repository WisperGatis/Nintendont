using Ryujinx.Horizon.Common;
using Ryujinx.Horizon.Sdk.Arp;
using Ryujinx.Horizon.Sdk.Arp.Detail;
using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;

namespace Ryujinx.Horizon.Arp.Ipc
{
    partial class UnregistrationNotifier : ServiceObjectBase, IUnregistrationNotifier
    {
        private readonly ApplicationInstanceManager _applicationInstanceManager;

        public UnregistrationNotifier(ApplicationInstanceManager applicationInstanceManager)
        {
            _applicationInstanceManager = applicationInstanceManager;
        }

        [CmifCommand(0)]
        public Result GetReadableHandle([CopyHandle] out int readableHandle)
        {
            readableHandle = _applicationInstanceManager.EventHandle;

            return Result.Success;
        }


    }
}
