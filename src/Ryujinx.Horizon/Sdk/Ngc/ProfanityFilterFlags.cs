using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using System;

namespace Ryujinx.Horizon.Sdk.Ngc
{
    [Flags]
    enum ProfanityFilterFlags
    {
        None = 0,
        MatchNormalizedFormKC = 1 << 0,
        MatchSimilarForm = 1 << 1,
    }
}
