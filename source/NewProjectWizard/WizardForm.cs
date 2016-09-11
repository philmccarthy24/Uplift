using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uplift.VSExtensions
{
    public partial class WizardForm : Form
    {
        public string ServiceNamespace { get; private set; }
        public string ServiceInterfaceName { get; private set; }
        public string ServiceImplementationClassName { get; private set; }
        public int IISExpressPort { get; private set; }
        public string IISExpressServiceRoot { get; private set; }

        public WizardForm()
        {
            InitializeComponent();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }   

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                ServiceNamespace = serviceNamespace.Text;
                ServiceInterfaceName = serviceInterface.Text;
                ServiceImplementationClassName = serviceImplClass.Text;
                IISExpressPort = int.Parse(iisPort.Text);
                IISExpressServiceRoot = iisServiceRoot.Text;

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        bool Validate()
        {
            bool validated = true;
            // simple validation
            if (serviceNamespace.Text == string.Empty)
            {
                errorProvider.SetError(serviceNamespace, "Cannot be empty");
                validated = false;
            }
            if (serviceInterface.Text == string.Empty)
            {
                errorProvider.SetError(serviceInterface, "Cannot be empty");
                validated = false;
            }
            if (serviceImplClass.Text == string.Empty)
            {
                errorProvider.SetError(serviceImplClass, "Cannot be empty");
                validated = false;
            }
            int i = 0;
            bool canParse = int.TryParse(iisPort.Text, out i);
            if (!canParse || i == 0)
            {
                errorProvider.SetError(iisPort, "Must be a positive integer");
                validated = false;
            }
            if (iisServiceRoot.Text == string.Empty)
            {
                errorProvider.SetError(iisServiceRoot, "Cannot be empty");
                validated = false;
            }
            return validated;
        }
    
        public event PropertyChangedEventHandler PropertyChanged;

        private void ClearValidationErrors(object sender, KeyEventArgs e)
        {
            errorProvider.SetError((Control)sender, string.Empty);
        }
    }
}
