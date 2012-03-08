namespace Persondata_o_matic
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelName = new System.Windows.Forms.Label();
            this.labelAlternativeNames = new System.Windows.Forms.Label();
            this.labelShortDescription = new System.Windows.Forms.Label();
            this.labelDateOfBirth = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxAlternativeNames = new System.Windows.Forms.TextBox();
            this.textBoxShortDescription = new System.Windows.Forms.TextBox();
            this.textBoxDateOfBirth = new System.Windows.Forms.TextBox();
            this.textBoxPlaceOfBirth = new System.Windows.Forms.TextBox();
            this.textBoxDateOfDeath = new System.Windows.Forms.TextBox();
            this.textBoxPlaceOfDeath = new System.Windows.Forms.TextBox();
            this.panelPersondata = new System.Windows.Forms.Panel();
            this.labelWarnings = new System.Windows.Forms.Label();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.textBoxEditSummary = new System.Windows.Forms.TextBox();
            this.labelEditSummary = new System.Windows.Forms.Label();
            this.buttonSkip = new System.Windows.Forms.Button();
            this.buttonSaveAndNext = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.buttonOpenInBrowser = new System.Windows.Forms.Button();
            this.tabControlPage = new System.Windows.Forms.TabControl();
            this.tabPageMarkup = new System.Windows.Forms.TabPage();
            this.textBoxWikitext = new System.Windows.Forms.TextBox();
            this.tabPageBrowser = new System.Windows.Forms.TabPage();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.panelPersondata.SuspendLayout();
            this.tabControlPage.SuspendLayout();
            this.tabPageMarkup.SuspendLayout();
            this.tabPageBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(7, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 5;
            this.labelName.Text = "&Name:";
            // 
            // labelAlternativeNames
            // 
            this.labelAlternativeNames.AutoSize = true;
            this.labelAlternativeNames.Location = new System.Drawing.Point(7, 29);
            this.labelAlternativeNames.Name = "labelAlternativeNames";
            this.labelAlternativeNames.Size = new System.Drawing.Size(94, 13);
            this.labelAlternativeNames.TabIndex = 7;
            this.labelAlternativeNames.Text = "&Alternative names:";
            // 
            // labelShortDescription
            // 
            this.labelShortDescription.AutoSize = true;
            this.labelShortDescription.Location = new System.Drawing.Point(7, 53);
            this.labelShortDescription.Name = "labelShortDescription";
            this.labelShortDescription.Size = new System.Drawing.Size(89, 13);
            this.labelShortDescription.TabIndex = 9;
            this.labelShortDescription.Text = "&Short description:";
            // 
            // labelDateOfBirth
            // 
            this.labelDateOfBirth.AutoSize = true;
            this.labelDateOfBirth.Location = new System.Drawing.Point(7, 77);
            this.labelDateOfBirth.Name = "labelDateOfBirth";
            this.labelDateOfBirth.Size = new System.Drawing.Size(68, 13);
            this.labelDateOfBirth.TabIndex = 11;
            this.labelDateOfBirth.Text = "Date of &birth:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Place of b&irth:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Place of d&eath:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Date of &death:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(125, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(267, 20);
            this.textBoxName.TabIndex = 6;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // textBoxAlternativeNames
            // 
            this.textBoxAlternativeNames.Location = new System.Drawing.Point(125, 26);
            this.textBoxAlternativeNames.Name = "textBoxAlternativeNames";
            this.textBoxAlternativeNames.Size = new System.Drawing.Size(267, 20);
            this.textBoxAlternativeNames.TabIndex = 8;
            this.textBoxAlternativeNames.TextChanged += new System.EventHandler(this.textBoxAlternativeNames_TextChanged);
            // 
            // textBoxShortDescription
            // 
            this.textBoxShortDescription.Location = new System.Drawing.Point(125, 50);
            this.textBoxShortDescription.Name = "textBoxShortDescription";
            this.textBoxShortDescription.Size = new System.Drawing.Size(267, 20);
            this.textBoxShortDescription.TabIndex = 10;
            this.textBoxShortDescription.TextChanged += new System.EventHandler(this.textBoxShortDescription_TextChanged);
            // 
            // textBoxDateOfBirth
            // 
            this.textBoxDateOfBirth.Location = new System.Drawing.Point(125, 74);
            this.textBoxDateOfBirth.Name = "textBoxDateOfBirth";
            this.textBoxDateOfBirth.Size = new System.Drawing.Size(267, 20);
            this.textBoxDateOfBirth.TabIndex = 12;
            this.textBoxDateOfBirth.TextChanged += new System.EventHandler(this.textBoxDateOfBirth_TextChanged);
            // 
            // textBoxPlaceOfBirth
            // 
            this.textBoxPlaceOfBirth.Location = new System.Drawing.Point(125, 96);
            this.textBoxPlaceOfBirth.Name = "textBoxPlaceOfBirth";
            this.textBoxPlaceOfBirth.Size = new System.Drawing.Size(267, 20);
            this.textBoxPlaceOfBirth.TabIndex = 14;
            this.textBoxPlaceOfBirth.TextChanged += new System.EventHandler(this.textBoxPlaceOfBirth_TextChanged);
            // 
            // textBoxDateOfDeath
            // 
            this.textBoxDateOfDeath.Location = new System.Drawing.Point(125, 119);
            this.textBoxDateOfDeath.Name = "textBoxDateOfDeath";
            this.textBoxDateOfDeath.Size = new System.Drawing.Size(267, 20);
            this.textBoxDateOfDeath.TabIndex = 16;
            this.textBoxDateOfDeath.TextChanged += new System.EventHandler(this.textBoxDateOfDeath_TextChanged);
            // 
            // textBoxPlaceOfDeath
            // 
            this.textBoxPlaceOfDeath.Location = new System.Drawing.Point(125, 141);
            this.textBoxPlaceOfDeath.Name = "textBoxPlaceOfDeath";
            this.textBoxPlaceOfDeath.Size = new System.Drawing.Size(267, 20);
            this.textBoxPlaceOfDeath.TabIndex = 18;
            this.textBoxPlaceOfDeath.TextChanged += new System.EventHandler(this.textBoxPlaceOfDeath_TextChanged);
            // 
            // panelPersondata
            // 
            this.panelPersondata.Controls.Add(this.labelWarnings);
            this.panelPersondata.Controls.Add(this.buttonRemove);
            this.panelPersondata.Controls.Add(this.textBoxEditSummary);
            this.panelPersondata.Controls.Add(this.labelEditSummary);
            this.panelPersondata.Controls.Add(this.buttonSkip);
            this.panelPersondata.Controls.Add(this.buttonSaveAndNext);
            this.panelPersondata.Controls.Add(this.textBoxName);
            this.panelPersondata.Controls.Add(this.textBoxPlaceOfDeath);
            this.panelPersondata.Controls.Add(this.labelName);
            this.panelPersondata.Controls.Add(this.textBoxDateOfDeath);
            this.panelPersondata.Controls.Add(this.labelAlternativeNames);
            this.panelPersondata.Controls.Add(this.textBoxPlaceOfBirth);
            this.panelPersondata.Controls.Add(this.labelShortDescription);
            this.panelPersondata.Controls.Add(this.textBoxDateOfBirth);
            this.panelPersondata.Controls.Add(this.labelDateOfBirth);
            this.panelPersondata.Controls.Add(this.textBoxShortDescription);
            this.panelPersondata.Controls.Add(this.label1);
            this.panelPersondata.Controls.Add(this.textBoxAlternativeNames);
            this.panelPersondata.Controls.Add(this.label3);
            this.panelPersondata.Controls.Add(this.label2);
            this.panelPersondata.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelPersondata.Location = new System.Drawing.Point(603, 0);
            this.panelPersondata.Name = "panelPersondata";
            this.panelPersondata.Size = new System.Drawing.Size(395, 584);
            this.panelPersondata.TabIndex = 15;
            // 
            // labelWarnings
            // 
            this.labelWarnings.AutoSize = true;
            this.labelWarnings.ForeColor = System.Drawing.Color.Red;
            this.labelWarnings.Location = new System.Drawing.Point(10, 252);
            this.labelWarnings.Name = "labelWarnings";
            this.labelWarnings.Size = new System.Drawing.Size(91, 13);
            this.labelWarnings.TabIndex = 24;
            this.labelWarnings.Text = "Warnings go here";
            this.labelWarnings.Visible = false;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(204, 200);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(91, 23);
            this.buttonRemove.TabIndex = 23;
            this.buttonRemove.Text = "&Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // textBoxEditSummary
            // 
            this.textBoxEditSummary.Location = new System.Drawing.Point(125, 171);
            this.textBoxEditSummary.Name = "textBoxEditSummary";
            this.textBoxEditSummary.Size = new System.Drawing.Size(267, 20);
            this.textBoxEditSummary.TabIndex = 20;
            this.textBoxEditSummary.TextChanged += new System.EventHandler(this.textBoxEditSummary_TextChanged);
            // 
            // labelEditSummary
            // 
            this.labelEditSummary.AutoSize = true;
            this.labelEditSummary.Location = new System.Drawing.Point(7, 174);
            this.labelEditSummary.Name = "labelEditSummary";
            this.labelEditSummary.Size = new System.Drawing.Size(72, 13);
            this.labelEditSummary.TabIndex = 19;
            this.labelEditSummary.Text = "&Edit summary:";
            // 
            // buttonSkip
            // 
            this.buttonSkip.Location = new System.Drawing.Point(107, 200);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new System.Drawing.Size(91, 23);
            this.buttonSkip.TabIndex = 22;
            this.buttonSkip.Text = "S&kip";
            this.buttonSkip.UseVisualStyleBackColor = true;
            this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
            // 
            // buttonSaveAndNext
            // 
            this.buttonSaveAndNext.Location = new System.Drawing.Point(10, 200);
            this.buttonSaveAndNext.Name = "buttonSaveAndNext";
            this.buttonSaveAndNext.Size = new System.Drawing.Size(91, 23);
            this.buttonSaveAndNext.TabIndex = 21;
            this.buttonSaveAndNext.Text = "&Save and next";
            this.buttonSaveAndNext.UseVisualStyleBackColor = true;
            this.buttonSaveAndNext.Click += new System.EventHandler(this.buttonSaveAndNext_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(12, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(30, 13);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "&Title:";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(48, 7);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.ReadOnly = true;
            this.textBoxTitle.Size = new System.Drawing.Size(442, 20);
            this.textBoxTitle.TabIndex = 1;
            // 
            // buttonOpenInBrowser
            // 
            this.buttonOpenInBrowser.Location = new System.Drawing.Point(496, 7);
            this.buttonOpenInBrowser.Name = "buttonOpenInBrowser";
            this.buttonOpenInBrowser.Size = new System.Drawing.Size(101, 23);
            this.buttonOpenInBrowser.TabIndex = 2;
            this.buttonOpenInBrowser.Text = "&Open in browser";
            this.buttonOpenInBrowser.UseVisualStyleBackColor = true;
            this.buttonOpenInBrowser.Click += new System.EventHandler(this.buttonOpenInBrowser_Click);
            // 
            // tabControlPage
            // 
            this.tabControlPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControlPage.Controls.Add(this.tabPageMarkup);
            this.tabControlPage.Controls.Add(this.tabPageBrowser);
            this.tabControlPage.Location = new System.Drawing.Point(2, 33);
            this.tabControlPage.Name = "tabControlPage";
            this.tabControlPage.SelectedIndex = 0;
            this.tabControlPage.Size = new System.Drawing.Size(602, 551);
            this.tabControlPage.TabIndex = 3;
            this.tabControlPage.SelectedIndexChanged += new System.EventHandler(this.tabControlPage_SelectedIndexChanged);
            // 
            // tabPageMarkup
            // 
            this.tabPageMarkup.Controls.Add(this.textBoxWikitext);
            this.tabPageMarkup.Location = new System.Drawing.Point(4, 22);
            this.tabPageMarkup.Name = "tabPageMarkup";
            this.tabPageMarkup.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMarkup.Size = new System.Drawing.Size(594, 525);
            this.tabPageMarkup.TabIndex = 0;
            this.tabPageMarkup.Text = "Markup";
            this.tabPageMarkup.UseVisualStyleBackColor = true;
            // 
            // textBoxWikitext
            // 
            this.textBoxWikitext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxWikitext.Location = new System.Drawing.Point(3, 3);
            this.textBoxWikitext.Multiline = true;
            this.textBoxWikitext.Name = "textBoxWikitext";
            this.textBoxWikitext.ReadOnly = true;
            this.textBoxWikitext.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxWikitext.Size = new System.Drawing.Size(588, 519);
            this.textBoxWikitext.TabIndex = 4;
            // 
            // tabPageBrowser
            // 
            this.tabPageBrowser.Controls.Add(this.webBrowser);
            this.tabPageBrowser.Location = new System.Drawing.Point(4, 22);
            this.tabPageBrowser.Name = "tabPageBrowser";
            this.tabPageBrowser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBrowser.Size = new System.Drawing.Size(594, 525);
            this.tabPageBrowser.TabIndex = 1;
            this.tabPageBrowser.Text = "Preview";
            this.tabPageBrowser.UseVisualStyleBackColor = true;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(3, 3);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(588, 519);
            this.webBrowser.TabIndex = 17;
            // 
            // FormMain
            // 
            this.AcceptButton = this.buttonSaveAndNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 584);
            this.Controls.Add(this.tabControlPage);
            this.Controls.Add(this.buttonOpenInBrowser);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.panelPersondata);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "Persondata-o-matic";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.panelPersondata.ResumeLayout(false);
            this.panelPersondata.PerformLayout();
            this.tabControlPage.ResumeLayout(false);
            this.tabPageMarkup.ResumeLayout(false);
            this.tabPageMarkup.PerformLayout();
            this.tabPageBrowser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelAlternativeNames;
        private System.Windows.Forms.Label labelShortDescription;
        private System.Windows.Forms.Label labelDateOfBirth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxAlternativeNames;
        private System.Windows.Forms.TextBox textBoxShortDescription;
        private System.Windows.Forms.TextBox textBoxDateOfBirth;
        private System.Windows.Forms.TextBox textBoxPlaceOfBirth;
        private System.Windows.Forms.TextBox textBoxDateOfDeath;
        private System.Windows.Forms.TextBox textBoxPlaceOfDeath;
        private System.Windows.Forms.Panel panelPersondata;
        private System.Windows.Forms.Button buttonSkip;
        private System.Windows.Forms.Button buttonSaveAndNext;
        private System.Windows.Forms.TextBox textBoxEditSummary;
        private System.Windows.Forms.Label labelEditSummary;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonOpenInBrowser;
        private System.Windows.Forms.TabControl tabControlPage;
        private System.Windows.Forms.TabPage tabPageMarkup;
        private System.Windows.Forms.TextBox textBoxWikitext;
        private System.Windows.Forms.TabPage tabPageBrowser;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Label labelWarnings;
    }
}

