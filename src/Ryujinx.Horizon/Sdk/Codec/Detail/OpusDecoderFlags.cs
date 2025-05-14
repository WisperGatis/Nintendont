using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using System;

namespace Ryujinx.Horizon.Sdk.Codec.Detail
{
    [Flags]
    enum OpusDecoderFlags : uint
    {
        None,
        LargeFrameSize = 1 << 0,
    }
}
