using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
namespace Ryujinx.Horizon.MmNv
{
    class MmNvMain : IService
    {
        public static void Main(ServiceTable serviceTable)
        {
            MmNvIpcServer ipcServer = new();

            ipcServer.Initialize();

            serviceTable.SignalServiceReady();

            ipcServer.ServiceRequests();
            ipcServer.Shutdown();
        }
    }
}
