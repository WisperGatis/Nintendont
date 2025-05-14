using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
namespace Ryujinx.Horizon.Sdk.OsTypes
{
    class MultiWaitHolder : MultiWaitHolderBase
    {
        public object UserData { get; set; }

        public void UnlinkFromMultiWaitHolder()
        {
            DebugUtil.Assert(IsLinked);

            MultiWait.UnlinkMultiWaitHolder(this);

            SetMultiWait(null);
        }
    }
}
