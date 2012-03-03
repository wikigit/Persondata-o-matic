using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Persondata_o_matic
{
    public partial class LoadingDialog : Form
    {
        public LoadingDialog()
        {
            InitializeComponent();
        }

        public string Message
        {
            set
            {
                labelLoading.Text = value;
            }
        }
    }
}
