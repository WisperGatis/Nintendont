using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
namespace Ryujinx.Horizon.Sdk.Ncm
{
    public enum StorageId : byte
    {
        None,
        Host,
        GameCard,
        BuiltInSystem,
        BuiltInUser,
        SdCard,
        Any,
    }
}
