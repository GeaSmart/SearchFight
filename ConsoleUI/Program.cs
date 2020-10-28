using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using Business;
using System.Net;
using System.Text.RegularExpressions;

namespace ConsoleUI
{
    class Program
    {
        static string url = "";
        static void Main(string[] args)
        {            
            Console.WriteLine("SEARCH FIGHT");
            Console.WriteLine("============");

            Console.WriteLine("Ingrese las palabras a buscar:");
            string cadena = Console.ReadLine();
            
            foreach (string palabra in Utils.getSplittedStrings(cadena))
            {
                url = string.Format("{0}{1}", Shared.Variables.BingQueryUrl, palabra);
                
                Program obj = new Program();
                Thread thr1 = new Thread(new ThreadStart(obj.runBingThread)); //select google or bing thread
                thr1.SetApartmentState(ApartmentState.STA);
                thr1.Start();

                thr1.Join(); //added

                //Thread.Sleep(3000);
                Console.WriteLine(palabra + " -> " + Shared.Variables.resultNumber.ToString()); //select number o text as result                
            }

            //waiting for read all
            Console.WriteLine("\r\nPulse una tecla para salir");
            if (Console.ReadLine() == "x")
                Application.Exit();
        }

        //private void executeSearchEngine()
        //{
        //    url = string.Format("{0}{1}", Shared.Variables.BingQueryUrl, palabra);

        //    Program obj = new Program();
        //    Thread thr1 = new Thread(new ThreadStart(obj.runBingThread)); //select google or bing thread
        //    thr1.SetApartmentState(ApartmentState.STA);
        //    thr1.Start();

        //    thr1.Join(); //added

        //    //Thread.Sleep(3000);
        //    Console.WriteLine(palabra + " -> " + Shared.Variables.resultNumber.ToString()); //select number o text as result    
        //}

        private void runGoogleThread()
        {
            var th = new Thread(() =>
            {
                var br = new WebBrowser();
                br.DocumentCompleted += browser_DocumentCompleted;
                br.Navigate(url);
                Application.Run();
            });            

            th.SetApartmentState(ApartmentState.STA);
            th.Start();                       
        }

        private void runBingThread()
        {
            using (WebClient client = new WebClient())
            {
                Shared.Variables.resultHtml = client.DownloadString(url);
            }

            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.DocumentText = Shared.Variables.resultHtml;
            browser.Document.OpenNew(true);
            browser.Document.Write(Shared.Variables.resultHtml);
            browser.Refresh();
            Shared.Variables.doc = browser.Document;

            Script oScript = new Script();
            oScript.CallHtmlCode();
        }

        private static void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var br = sender as WebBrowser;                
                br.Navigate("javascript: window.external.CallServerSideCode();");
                Shared.Variables.doc = br.Document;

                Script oScript = new Script();
                br.ObjectForScripting = oScript;       
            }
            catch
            {

            }
        }
    }
}
