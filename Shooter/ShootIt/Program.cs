using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShootIt {
    class Program {
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();
        static void Main(string[] args) {

            #region Extract Command Line Parameters
            Regex cmdRegEx = new Regex(@"/(?<name>.+?)=(?<val>.+)");

            Dictionary<string, string> cmdArgs = new Dictionary<string, string>();
            foreach (string s in args) {
                Console.WriteLine(s);
                Match m = cmdRegEx.Match(s);
                if (m.Success) {
                    cmdArgs.Add(m.Groups[1].Value, m.Groups[2].Value);
                }
            }
            #endregion

            if (cmdArgs.ContainsKey(CommandArgumentParam.Url.ToString())
                && cmdArgs.ContainsKey(CommandArgumentParam.ImagePath.ToString())
                && cmdArgs.ContainsKey(CommandArgumentParam.JsDelay.ToString())) {
                int jsDelay;
                if (int.TryParse(cmdArgs.FirstOrDefault(x => x.Key == CommandArgumentParam.JsDelay.ToString()).Value, out jsDelay)) {
                    // shoot the image
                    ShootIt(cmdArgs.FirstOrDefault(x => x.Key == CommandArgumentParam.Url.ToString()).Value, cmdArgs.FirstOrDefault(x => x.Key == CommandArgumentParam.ImagePath.ToString()).Value, jsDelay).Wait();
                } else {
                    throw new Exception("parameter JsDelay is invalid.");
                }
            } else {
                throw new Exception("command line parametres are not valid or they are not provided.");
            }

            IntPtr pHandle = GetCurrentProcess();
            SetProcessWorkingSetSize(pHandle, -1, -1);
        }

        private static async Task ShootIt(string htmlPath, string imagePath, int jsDelay) {
            var result = await Shootit.GetPageAsImageAsync(htmlPath, imagePath, jsDelay);
        }
    }
}