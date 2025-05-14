using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using System;

namespace Ryujinx.Horizon.Sdk.Settings.System
{
    [Flags]
    enum ServiceDiscoveryControlSettings : uint
    {
        IsChangeEnvironmentIdentifierDisabled = 1 << 0,
    }
}
