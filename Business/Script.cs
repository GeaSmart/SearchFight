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
        

        public void CallServerSideCode()
        {
            var doc = Shared.Variables.doc;
            //MessageBox.Show(doc.GetElementsByTagName("div")["result-stats"].OuterHtml);

            resultText = doc.GetElementsByTagName("div")["result-stats"].OuterHtml;
            resultText = Regex.Match(resultText, @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)").Value;

            Shared.Variables.resultNumber = Convert.ToInt64(resultText.Replace(",", ""));

            
        }
    }
}
