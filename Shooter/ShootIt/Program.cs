using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShootIt
{
    class Program
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();
        static void Main(string[] args)
        {
            Console.WriteLine("Arg 1 : " + args[0]);
            Console.WriteLine("Arg 2 : " + args[1]);
            ShootIt(args[0], args[1]).Wait();

            IntPtr pHandle = GetCurrentProcess();
            SetProcessWorkingSetSize(pHandle, -1, -1);
        }

        private static async Task ShootIt(string htmlPath, string imagePath)
        {
            var result = await Shootit.GetPageAsImageAsync(htmlPath, imagePath);
        }
    }
}
