using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OSHVisualGui
{
    public partial class CodeForm : Form
    {
        private GuiControls.Form form;

        public CodeForm(GuiControls.Form form)
        {
            this.form = form;

            InitializeComponent();
        }

        private void CodeForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
