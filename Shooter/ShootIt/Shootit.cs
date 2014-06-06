using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShootIt
{
    public class Shootit
    {
       
        public static async Task<bool> GetPageAsImageAsync(string url, string path)
        {
            var tcs = new TaskCompletionSource<bool>();
            await TaskEx.Delay(1);

            var thread = new Thread(() =>
            {

                using (var browser = new WebBrowser())
                {

                    browser.Size = new Size(1280, 768);
                    browser.ScrollBarsEnabled = false;
                    WebBrowserDocumentCompletedEventHandler documentCompleted = null;
                    documentCompleted = async (o, s) =>
                    {
                        var loadedBrowser = (WebBrowser)o;
                        loadedBrowser.DocumentCompleted -= documentCompleted;
                        await TaskEx.Delay(2000); //Run JS a few seconds more

                        if (loadedBrowser.Document != null)
                        {
                            if (loadedBrowser.Document.Body != null)
                            {
                                loadedBrowser.Width = loadedBrowser.Document.Body.ScrollRectangle.Width;
                                loadedBrowser.Height = loadedBrowser.Document.Body.ScrollRectangle.Height;
                            }
                        }

                        using (var bitmap = new Bitmap(loadedBrowser.Width, loadedBrowser.Height))
                        {
                            loadedBrowser.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                            bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        System.Windows.Forms.Application.Exit();
                        tcs.TrySetResult(true);
                    };

                    browser.ScriptErrorsSuppressed = true;
                    browser.DocumentCompleted += documentCompleted;
                    browser.Navigate(url);
                    System.Windows.Forms.Application.Run();
                }
               
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            System.Threading.Thread.SpinWait(3000);
           
            return tcs.Task.Result;
        }

    }
}
