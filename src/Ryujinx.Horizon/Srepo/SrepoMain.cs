using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
namespace Ryujinx.Horizon.Srepo
{
    class SrepoMain : IService
    {
        public static void Main(ServiceTable serviceTable)
        {
            SrepoIpcServer ipcServer = new();

            ipcServer.Initialize();

            serviceTable.SignalServiceReady();

            ipcServer.ServiceRequests();
            ipcServer.Shutdown();
        }
    }
}
