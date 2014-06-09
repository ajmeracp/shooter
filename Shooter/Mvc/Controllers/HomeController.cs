using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            for (int i = 1; i < 12; i++)
            {
                //ShootIt(@"C:\Users\Shubh\Desktop\html\Rateboard" + i + @"\index.html",
                //    @"C:\Users\Shubh\Desktop\html\ScreenShots\" + Guid.NewGuid() + ".png").Wait();
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"E:\Shooter Work\shooter\Shooter\ShootIt\bin\Debug\ShootIt.exe",
                    Arguments =
                        @"Url=C:\Users\Shubh\Desktop\html\Rateboard" + i + @"\index.html" +
                        @" ImagePath=C:\Users\Shubh\Desktop\html\ScreenShots\" + Guid.NewGuid() + ".png" +
                        " JsDelay=2000"
                };
                var process = Process.Start(startInfo);
                if (process != null) process.WaitForExit();
            }
            return View();
        }

        private async Task ShootIt(string htmlPath, string imagePath)
        {
            var result = await Shooter.Shootit.GetPageAsImageAsync(htmlPath, imagePath);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
