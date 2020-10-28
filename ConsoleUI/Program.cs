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
using System.Configuration;

namespace ConsoleUI
{
    class Program
    {
        static string urlGoogle = "";
        static string urlBing = "";
        static List<eResults> listadoResultados = new List<eResults>();

        static void Main(string[] args)
        {
            //obtenemos valores del programa desde el App.config
            Utils.PrintToUser(string.Format("Welcome to {0} {1} starring Google vs Bing! [By {2}]",ConfigurationSettings.AppSettings["AppName"], ConfigurationSettings.AppSettings["version"], ConfigurationSettings.AppSettings["author"]), Variables.HEADER_COLOR);            
            Initialize();
        }

        private static void Initialize()
        {
            listadoResultados.Clear();
            Utils.PrintToUser("====================================================================");
            Utils.PrintToUser("Ingrese las palabras a buscar:");
            string cadena = Console.ReadLine().Trim();  //Nos aseguramos que no hayan espacios en blanco al principio o final
            List<string> listadoSegregado = Utils.getSplittedStrings(cadena);
            Utils.PrintToUser(string.Format("procesando {0} elementos, espere...", listadoSegregado.Count));

            if (listadoSegregado.Count > 0)
            {
                foreach (string palabra in listadoSegregado)
                {
                    executeSearchEngine(palabra);
                }
                showResults();
            }
            else
            {
                Utils.PrintToUser("No se pudieron detectar palabras, por favor intente nuevamente", Variables.DANGER_COLOR);
                Initialize();
            }
        }

        private static void executeSearchEngine(string searchPhrase)
        {
            try
            {
                //Utlizamos dos variables, debido a que la ejecucion del algoritmo emplea dos hilos distintos y queremos evitar conflictos de asignacion de variables por cocurrencia
                urlGoogle = string.Format("{0}{1}", Shared.Variables.googleQueryUrl, searchPhrase);
                urlBing = string.Format("{0}{1}", Shared.Variables.BingQueryUrl, searchPhrase);

                Program obj = new Program();

                Thread thr1 = new Thread(new ThreadStart(obj.runGoogleThread));
                Thread thr2 = new Thread(new ThreadStart(obj.runBingThread));

                thr1.SetApartmentState(ApartmentState.STA);
                thr1.Start();

                thr2.SetApartmentState(ApartmentState.STA);
                thr2.Start();

                thr2.Join();//Aseguramos que culmine el hilo 2 antes de continuar.

                Thread.Sleep(4000);//el hilo 1 de google necesita que múltiples scripts js de la página se terminen de ejecutar, y al no recibir respusta de culminación controlamos la ejecucion de tiempo, sin embargo podría optimizarse.

                Utils.PrintToUser(string.Format("{0}\t\t\t Google:{1:n0}\tBing:{2:n0}", searchPhrase, Shared.Variables.resultNumberGoogle, Shared.Variables.resultNumberBing));

                eResults res = new eResults();
                res.searchProvider = "Google";
                res.wordSearched = searchPhrase;
                res.numberResults = Shared.Variables.resultNumberGoogle;
                listadoResultados.Add(res);

                eResults res2 = new eResults();
                res2.searchProvider = "Bing";
                res2.wordSearched = searchPhrase;
                res2.numberResults = Shared.Variables.resultNumberBing;
                listadoResultados.Add(res2);
            }
            catch(Exception ex)
            {
                Utils.PrintToUser("Ocurrió un error, se reiniciará el programa", Variables.DANGER_COLOR);
                Initialize();
            }
        }

        private void runGoogleThread()
        {
            var th = new Thread(() =>
            {
                WebBrowser browser = new WebBrowser();
                browser.DocumentCompleted += browser_DocumentCompleted;
                browser.Navigate(urlGoogle);
                Application.Run();
            });            

            th.SetApartmentState(ApartmentState.STA);
            th.Start();                       
        }

        private void runBingThread()
        {
            string document;
            using (WebClient client = new WebClient())
            {
                document = client.DownloadString(urlBing);
            }

            WebBrowser browser = new WebBrowser();
            browser.Navigate(urlBing);
            browser.ScriptErrorsSuppressed = true;
            browser.DocumentText = document;
            browser.Document.OpenNew(true);
            browser.Document.Write(document);
            browser.Refresh();
            Shared.Variables.htmlBing = browser.Document;

            Script oScript = new Script();
            Thread.Sleep(1);//delay para que cargue completamente el dom
            oScript.CallHtmlCode();
        }

        private static void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var br = sender as WebBrowser;                
                br.Navigate("javascript: window.external.CallServerSideCode();");
                Shared.Variables.htmlGoogle = br.Document;

                Script oScript = new Script();
                br.ObjectForScripting = oScript;       
            }
            catch (Exception ex)
            {
                Utils.PrintToUser("Ocurrió un error al ejecutar código del lado del servidor o el servidor no responde, se reiniciará el programa",Variables.DANGER_COLOR);
                Initialize();
            }
        }

        private static void showResults()
        {
            try
            {
                Utils.PrintToUser("\r\n***RESULTS***", Variables.HEADER_COLOR);

                string googleWinner = (from l in listadoResultados where l.searchProvider.Equals("Google") orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();
                string bingWinner = (from l in listadoResultados where l.searchProvider.Equals("Bing") orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();

                string totalWinner = (from l in listadoResultados orderby l.numberResults descending select l.wordSearched).FirstOrDefault().ToString();

                Utils.PrintToUser("Google winner:\t" + googleWinner);
                Utils.PrintToUser("Bing winner:\t" + bingWinner);
                Utils.PrintToUser("Total winner:\t" + totalWinner, Variables.RESULT_COLOR);

                ContinuePrompt();
            }
            catch (Exception exception)
            {
                Utils.PrintToUser("Ocurrió un error al ejecutar código del lado del servidor o el servidor no responde, se reiniciará el programa", Variables.DANGER_COLOR);
                Initialize();
            }
        }

        private static void ContinuePrompt()
        {
            Utils.PrintToUser("\r\n¿Desea realizar otra búsqueda?(S/N)", Variables.QUESTION_COLOR);
            string response = Console.ReadLine().ToUpper();

            if (response.Equals("S"))
            {
                Initialize();
            }
            if (response.Equals("N"))
            {
                Application.Exit();
            }
            else
            {
                Utils.PrintToUser("\r\nComando no válido", Variables.DANGER_COLOR);
                ContinuePrompt();//ejecucion recursiva del prompt
            }
        }
    }
}
