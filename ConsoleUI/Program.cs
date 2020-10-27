using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using Business;

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

            foreach(var palabra in cadena.Split(' '))
            {
                url = string.Format("https://www.google.com/search?q={0}", palabra);

                Program obj = new Program();
                Thread thr1 = new Thread(new ThreadStart(obj.runBrowserThread));
                thr1.SetApartmentState(ApartmentState.STA);
                thr1.Start();
                Thread.Sleep(3000);
                Console.WriteLine(palabra + " -> " + Shared.Variables.resultNumber.ToString());
            }

            //waiting for read all
            Console.WriteLine("\r\nPulse una tecla para salir");
            if (Console.ReadLine() == "x")
                Application.Exit();
        }

        private void runBrowserThread()
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
