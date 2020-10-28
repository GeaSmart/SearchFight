using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using Business;
using Entity;
using System.Net;
using System.Text.RegularExpressions;

namespace ConsoleUI
{
    class Program
    {
        static string urlGoogle = "";
        static string urlBing = "";
        static List<eResults> listado = new List<eResults>();

        static void Main(string[] args)
        {            
            Console.WriteLine("SEARCH FIGHT");
            Console.WriteLine("============");

            Console.WriteLine("Ingrese las palabras a buscar:");
            string cadena = Console.ReadLine();

            
            
            foreach (string palabra in Utils.getSplittedStrings(cadena))
            {
                executeSearchEngine(palabra);
            }


            showResults();

            //waiting for read all
            Console.WriteLine("\r\nPulse una tecla para salir");
            if (Console.ReadLine() == "x")
                Application.Exit();
        }

        private static void executeSearchEngine(string searchPhrase)
        {
            urlGoogle = string.Format("{0}{1}", Shared.Variables.googleQueryUrl, searchPhrase);
            urlBing = string.Format("{0}{1}", Shared.Variables.BingQueryUrl, searchPhrase);

            Program obj = new Program();

            Thread thr1 = new Thread(new ThreadStart(obj.runGoogleThread));
            Thread thr2 = new Thread(new ThreadStart(obj.runBingThread));

            thr1.SetApartmentState(ApartmentState.STA);
            thr1.Start();

            thr2.SetApartmentState(ApartmentState.STA);
            thr2.Start();

            thr2.Join();

            Thread.Sleep(3000);

            Console.WriteLine(string.Format("{0}\t\t Google:{1}\tBing:{2}",searchPhrase, Shared.Variables.resultNumber.ToString(), Shared.Variables.resultNumber_2.ToString()));
            eResults res = new eResults();
            res.searchProvider = "Google";
            res.wordSearched = searchPhrase;
            res.numberResults = Shared.Variables.resultNumber;
            listado.Add(res);

            eResults res2 = new eResults();
            res2.searchProvider = "Bing";
            res2.wordSearched = searchPhrase;
            res2.numberResults = Shared.Variables.resultNumber_2;
            listado.Add(res2);
        }

        private void runGoogleThread()
        {
            var th = new Thread(() =>
            {
                var br = new WebBrowser();
                br.DocumentCompleted += browser_DocumentCompleted;
                br.Navigate(urlGoogle);
                Application.Run();
            });            

            th.SetApartmentState(ApartmentState.STA);
            th.Start();                       
        }

        private void runBingThread()
        {
            using (WebClient client = new WebClient())
            {
                Shared.Variables.resultHtml_2 = client.DownloadString(urlBing);
            }

            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.DocumentText = Shared.Variables.resultHtml_2;
            browser.Document.OpenNew(true);
            browser.Document.Write(Shared.Variables.resultHtml_2);
            browser.Refresh();
            Shared.Variables.doc_2 = browser.Document;

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

        private static void showResults()
        {
            Console.WriteLine("\r\n");
            Console.WriteLine("***RESULTS***");
            Console.WriteLine("=============");

            string googleWinner = (from l in listado where l.searchProvider.Equals("Google") orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();
            string bingWinner = (from l in listado where l.searchProvider.Equals("Bing") orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();

            string totalWinner = (from l in listado orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();

            Console.WriteLine("Google winner:\t" + googleWinner);
            Console.WriteLine("Bing winner:\t"  + bingWinner);
            Console.WriteLine("Total winner:\t" + totalWinner);
        }
    }
}
