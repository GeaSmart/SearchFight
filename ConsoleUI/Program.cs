using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SEARCH FIGHT");
            Console.WriteLine("============");

            Console.WriteLine("Ingrese las palabras a buscar:");
            string cadena = Console.ReadLine();

            Console.WriteLine("La cadena ingesada es: " + cadena);

            //waiting for read all
            Console.WriteLine("\r\nPulse una tecla para salir");
            Console.ReadKey();
        }

        
    }
}
