using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;

namespace Challenge
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }
        
        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.wbrBrowser.Navigate(string.Format("https://www.google.com/search?q={0}", this.textBox1.Text));
            

            //Script oScript = new Script();
            

            

            //Script oScript = new Script();
            //MessageBox.Show(oScript.CallServerSideCode().ToString());
        }

        private void wbrBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.wbrBrowser.Navigate("javascript: window.external.CallServerSideCode();");
            Shared.Variables.doc = this.wbrBrowser.Document;

            Script oScript = new Script();
            this.wbrBrowser.ObjectForScripting = oScript;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Shared.Variables.resultNumber.ToString());
        }
    }
}
