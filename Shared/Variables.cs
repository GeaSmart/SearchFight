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

        public static string resultHtml;
        public static string resultText;

    }
}
