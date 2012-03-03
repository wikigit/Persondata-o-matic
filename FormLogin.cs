using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Persondata_o_matic
{
    public enum PageListSource { category, file }

    public partial class FormLogin : Form
    {
        public string Username;
        public string Password;
        public PageListSource PageListSource;
        public string PageListSourceValue; // previously "Category"
        public int MaxLoadAheadPages;
        public int MaxCategoryPages;
        public bool RandomizeOrder;

        private static string settingsFileName = "settingsv2.dat";
        private static int settingsMinorVersion = 1; // so we don't have to rename the settings file each time and can still load old versions that may need backwards compatible stuff

        public FormLogin()
        {
            InitializeComponent();

            if (File.Exists(settingsFileName))
            {
                using (StreamReader settingsFile = new StreamReader(settingsFileName))
                {
                    if (int.Parse(settingsFile.ReadLine()) >= settingsMinorVersion) // only load the file if it's of current version or newer
                    {
                        textBoxUsername.Text = settingsFile.ReadLine();
                        textBoxPassword.Text = settingsFile.ReadLine();
                        PageListSource = StringToPageListSource(settingsFile.ReadLine());
                        if (PageListSource == PageListSource.category) radioButtonCategory.Checked = true;
                        if (PageListSource == PageListSource.file) radioButtonFile.Checked = true;
                        comboBoxPageListSourceCategory.Text = settingsFile.ReadLine();
                        textBoxPageListSourceFileName.Text = settingsFile.ReadLine();
                        numericUpDownMaximumLoadAheadPages.Value = int.Parse(settingsFile.ReadLine());
                        numericUpDownMaximumCategoryPages.Value = int.Parse(settingsFile.ReadLine());
                        checkBoxRandomOrder.Checked = settingsFile.ReadLine() == "true";
                    }
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

            if (radioButtonCategory.Checked)
            {
                if (string.IsNullOrEmpty(comboBoxPageListSourceCategory.Text))
                {
                    MessageBox.Show("You must choose the category name to work on.");
                    return;
                }

                if (!comboBoxPageListSourceCategory.Text.StartsWith("Category:"))
                {
                    MessageBox.Show("Category name must begin with \"Category:\".");
                    return;
                }

                PageListSource = PageListSource.category;
                PageListSourceValue = comboBoxPageListSourceCategory.Text;
            }
            if (radioButtonFile.Checked)
            {
                if (string.IsNullOrEmpty(textBoxPageListSourceFileName.Text))
                {
                    MessageBox.Show("You must choose the file name to work on.");
                    return;
                }

                if (!File.Exists(textBoxPageListSourceFileName.Text))
                {
                    MessageBox.Show("The file name you selected does not seem to exist.");
                    return;
                }

                PageListSource = PageListSource.file;
                PageListSourceValue = textBoxPageListSourceFileName.Text;
            }

            MaxLoadAheadPages = (int)numericUpDownMaximumLoadAheadPages.Value;
            MaxCategoryPages = (int)numericUpDownMaximumCategoryPages.Value;
            RandomizeOrder = checkBoxRandomOrder.Checked;

            using (StreamWriter settingsFile = new StreamWriter(settingsFileName, /*append*/false))
            {
                settingsFile.WriteLine(settingsMinorVersion);
                settingsFile.WriteLine(Username);
                settingsFile.WriteLine(Password);
                settingsFile.WriteLine(PageListSourceToString(PageListSource));
                settingsFile.WriteLine(comboBoxPageListSourceCategory.Text);
                settingsFile.WriteLine(textBoxPageListSourceFileName.Text);
                settingsFile.WriteLine(MaxLoadAheadPages);
                settingsFile.WriteLine(MaxCategoryPages);
                settingsFile.WriteLine(RandomizeOrder ? "true" : "false");
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private string PageListSourceToString(PageListSource pageListSource)
        {
            switch (pageListSource)
            {
                case PageListSource.category:
                    return "category";
                case PageListSource.file:
                    return "file";
                default:
                    throw new ArgumentOutOfRangeException("pageListSource");
            }
        }

        private PageListSource StringToPageListSource(string str)
        {
            if (str == "category")
                return PageListSource.category;
            if (str == "file")
                return PageListSource.file;
            throw new ArgumentOutOfRangeException("str");
        }
    }
}
