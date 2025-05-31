using Ryujinx.Cpu;
using Ryujinx.Horizon.Common;

namespace Ryujinx.HLE.HOS.Kernel.SupervisorCall
{
    static partial class SyscallDispatch
    {
        public static void Dispatch64(ISyscallApi syscall, IExecutionContext context, int id)
        {
            // The actual syscall dispatch is handled by the SvcHandler attribute system
            // This is just a placeholder to satisfy the call in SyscallHandler.cs
            // The real implementation would use reflection to find the appropriate method based on the id
            // and call it with the parameters from the registers
            
            // For now, we'll just log that the syscall was attempted
            System.Console.WriteLine($"Attempted to dispatch 64-bit syscall with id {id}");
        }

        public static void Dispatch32(ISyscallApi syscall, IExecutionContext context, int id)
        {
            // The actual syscall dispatch is handled by the SvcHandler attribute system
            // This is just a placeholder to satisfy the call in SyscallHandler.cs
            // The real implementation would use reflection to find the appropriate method based on the id
            // and call it with the parameters from the registers
            
            // For now, we'll just log that the syscall was attempted
            System.Console.WriteLine($"Attempted to dispatch 32-bit syscall with id {id}");
        }
    }
}
