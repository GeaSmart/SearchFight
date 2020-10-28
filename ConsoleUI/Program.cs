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
            Utils.PrintToUser("SEARCH FIGHT STARRING GOOGLE vs BING! [By Gazabache]", Variables.HEADER_COLOR);            
            Initialize();
        }

        private static void Initialize()
        {
            Utils.PrintToUser("=====================================================");
            Utils.PrintToUser("Ingrese las palabras a buscar:");
            string cadena = Console.ReadLine().Trim();  //Nos aseguramos que no hayan espacios en blanco al principio o final

            foreach (string palabra in Utils.getSplittedStrings(cadena))
            {
                executeSearchEngine(palabra);
            }
            showResults();
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

            Utils.PrintToUser(string.Format("{0}\t\t Google:{1}\tBing:{2}",searchPhrase, Shared.Variables.resultNumber.ToString(), Shared.Variables.resultNumber_2.ToString()));
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
            Utils.PrintToUser("\r\n");
            Utils.PrintToUser("***RESULTS***", Variables.HEADER_COLOR);
            //Utils.PrintToUser("=============");

            string googleWinner = (from l in listado where l.searchProvider.Equals("Google") orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();
            string bingWinner = (from l in listado where l.searchProvider.Equals("Bing") orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();

            string totalWinner = (from l in listado orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();

            Utils.PrintToUser("Google winner:\t" + googleWinner);
            Utils.PrintToUser("Bing winner:\t"  + bingWinner);
            Utils.PrintToUser("Total winner:\t" + totalWinner,Variables.RESULT_COLOR);
                     
            Utils.PrintToUser("\r\n¿Desea realizar otra búsqueda?(S/N)",Variables.QUESTION_COLOR);
            if (Console.ReadLine().ToUpper() == "S")
            {
                Initialize();
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
