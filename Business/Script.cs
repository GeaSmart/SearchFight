using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Business
{
    [ComVisible(true)]
    public class Script
    {
        string resultText = "";
        string resultText_2 = "";


        public void CallServerSideCode()
        {
            var doc = Shared.Variables.doc;            
            resultText = doc.GetElementsByTagName("div")["result-stats"].OuterHtml;            
            resultText = Regex.Match(resultText, @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)").Value;
            Shared.Variables.resultNumber = Convert.ToInt64(resultText.Replace(",", ""));            
        }

        public void CallHtmlCode()
        {
            var doc = Shared.Variables.doc_2;                        
            resultText_2 = doc.GetElementsByTagName("div")["b_tween"].OuterHtml;
            resultText_2 = resultText_2.Replace(".", "");
            resultText_2 = Regex.Match(resultText_2, @"\d+").Value;
            Shared.Variables.resultNumber_2 = Convert.ToInt64(resultText_2);            
        }
    }
}
