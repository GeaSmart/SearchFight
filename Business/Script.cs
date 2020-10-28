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
        public void CallServerSideCode() //Utilizado por el motor de búsquedas Google debido a que no se puede obtener el número de resultados desde el html inicial directamente obtenido sino del html post-ejcucion de codigo JS.
        {
            var doc = Shared.Variables.htmlGoogle;            
            Shared.Variables.resultNumberGoogle = Convert.ToInt64(Regex.Match(doc.GetElementsByTagName("div")["result-stats"].OuterHtml, Shared.Variables.regexGoogle).Value.Replace(",", ""));            
        }

        public void CallHtmlCode() //Utilizado por Bing, quien desde el primer html obtenido ya nos da la información que necsitamos
        {
            var doc = Shared.Variables.htmlBing;            
            Shared.Variables.resultNumberBing = Convert.ToInt64(Regex.Match(doc.GetElementsByTagName("div")["b_tween"].OuterHtml.Replace(".", ""), Shared.Variables.regexBing).Value);            
        }
    }
}
