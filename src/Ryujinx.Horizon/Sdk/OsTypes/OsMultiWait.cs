using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
namespace Ryujinx.Horizon.Sdk.OsTypes
{
    static partial class Os
    {
        public static void FinalizeMultiWaitHolder(MultiWaitHolderBase holder)
        {
            DebugUtil.Assert(!holder.IsLinked);
        }
    }
}
