using Ryujinx.Horizon.Sdk.Sf;
using System.Collections.Generic;
using System;

namespace Ryujinx.Horizon.Sdk.Sf
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class CmifCommandAttribute : Attribute
    {
        public uint CommandId { get; }

        public CmifCommandAttribute(uint commandId)
        {
            CommandId = commandId;
        }
    }
}
