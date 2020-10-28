using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shared
{
    public static class Variables
    {
        /* Armado de URLs */
        public static string googleQueryUrl = @"https://www.google.com/search?q=";
        public static string BingQueryUrl = @"https://www.bing.com/search?q=";

        /* Manejo de estructuras HTML */
        public static HtmlDocument htmlGoogle;        
        public static HtmlDocument htmlBing;

        /* Resultados por motor de búsqueda */
        public static Int64 resultNumberGoogle;
        public static Int64 resultNumberBing;
                
        /* Colores para la consola | Console colors */
        public const ConsoleColor HEADER_COLOR = ConsoleColor.Cyan;
        public const ConsoleColor RESULT_COLOR = ConsoleColor.Green;
        public const ConsoleColor QUESTION_COLOR = ConsoleColor.Gray;
        public const ConsoleColor DANGER_COLOR = ConsoleColor.Red;

        /* Reglas Regex */
        public static string regexGoogle = @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)";
        public static string regexBing = @"\d+";
        
    }
}
