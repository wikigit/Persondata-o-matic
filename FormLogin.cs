using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Persondata_o_matic
{
    public partial class FormLogin : Form
    {
        public string Username;
        public string Password;
        public string Category;
        public int MaxLoadAheadPages;

        private static string settingsFileName = "settings.dat";

        public FormLogin()
        {
            InitializeComponent();

            if (File.Exists(settingsFileName))
            {
                using (StreamReader settingsFile = new StreamReader(settingsFileName))
                {
                    textBoxUsername.Text = settingsFile.ReadLine();
                    textBoxPassword.Text = settingsFile.ReadLine();
                    string category = settingsFile.ReadLine();
                    if (category == "Persondata templates without name parameter")
                    {
                        listBoxCategory.SelectedIndex = 0;
                    }
                    else if (category == "Persondata templates without short description parameter")
                    {
                        listBoxCategory.SelectedIndex = 1;
                    }
                    numericUpDownMaximumLoadAheadPages.Value = int.Parse(settingsFile.ReadLine());
                }
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == "")
            {
                MessageBox.Show("You must enter a username.");
                return;
            }
            else if (textBoxPassword.Text == "")
            {
                MessageBox.Show("You must enter a password.");
                return;
            }
            Username = textBoxUsername.Text;
            Password = textBoxPassword.Text;

            if (listBoxCategory.SelectedIndex == 0)
            {
                Category = "Persondata templates without name parameter";
            }
            else if (listBoxCategory.SelectedIndex == 1)
            {
                Category = "Persondata templates without short description parameter";
            }
            else
            {
                MessageBox.Show("You must select a type of persondata to work on.");
                return;
            }
            MaxLoadAheadPages = (int)numericUpDownMaximumLoadAheadPages.Value;

            using (StreamWriter settingsFile = new StreamWriter(settingsFileName, /*append*/false))
            {
                settingsFile.WriteLine(Username);
                settingsFile.WriteLine(Password);
                settingsFile.WriteLine(Category);
                settingsFile.WriteLine(MaxLoadAheadPages);
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
