using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
namespace Ryujinx.Horizon.Bcat.Types
{
    enum BcatServicePermissionLevel
    {
        Admin = -1,
        User = 1,
        System = 2,
        Manager = 6,
    }
}
