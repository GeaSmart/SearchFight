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
        public static string googleQueryUrl = @"https://www.google.com/search?q=";
        public static string BingQueryUrl = @"https://www.bing.com/search?q=";

        public static HtmlDocument doc;
        public static Int64 resultNumber;

        public static HtmlDocument doc_2;
        public static Int64 resultNumber_2;

        public static string resultHtml_2;
        public static string resultText_2;

        public const ConsoleColor HEADER_COLOR = ConsoleColor.Cyan;
        public const ConsoleColor RESULT_COLOR = ConsoleColor.Green;
        public const ConsoleColor QUESTION_COLOR = ConsoleColor.Gray;
        
    }
}
