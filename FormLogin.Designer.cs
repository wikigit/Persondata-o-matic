﻿namespace Persondata_o_matic
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxCategory = new System.Windows.Forms.ListBox();
            this.labelMaxLoadAhead = new System.Windows.Forms.Label();
            this.numericUpDownMaximumLoadAheadPages = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximumLoadAheadPages)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(97, 19);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(325, 20);
            this.textBoxUsername.TabIndex = 1;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(13, 22);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(58, 13);
            this.labelUsername.TabIndex = 0;
            this.labelUsername.Text = "&Username:";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(13, 48);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "&Password:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(97, 45);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(325, 20);
            this.textBoxPassword.TabIndex = 3;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(163, 145);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonLogin.TabIndex = 8;
            this.buttonLogin.Text = "&Log in";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Type of persondata to work on:";
            // 
            // listBoxCategory
            // 
            this.listBoxCategory.FormattingEnabled = true;
            this.listBoxCategory.Items.AddRange(new object[] {
            "Missing name parameter",
            "Missing short description parameter"});
            this.listBoxCategory.Location = new System.Drawing.Point(174, 77);
            this.listBoxCategory.Name = "listBoxCategory";
            this.listBoxCategory.Size = new System.Drawing.Size(248, 30);
            this.listBoxCategory.TabIndex = 5;
            // 
            // labelMaxLoadAhead
            // 
            this.labelMaxLoadAhead.AutoSize = true;
            this.labelMaxLoadAhead.Location = new System.Drawing.Point(15, 115);
            this.labelMaxLoadAhead.Name = "labelMaxLoadAhead";
            this.labelMaxLoadAhead.Size = new System.Drawing.Size(142, 13);
            this.labelMaxLoadAhead.TabIndex = 6;
            this.labelMaxLoadAhead.Text = "&Maximum load-ahead pages:";
            // 
            // numericUpDownMaximumLoadAheadPages
            // 
            this.numericUpDownMaximumLoadAheadPages.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMaximumLoadAheadPages.Location = new System.Drawing.Point(174, 113);
            this.numericUpDownMaximumLoadAheadPages.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownMaximumLoadAheadPages.Name = "numericUpDownMaximumLoadAheadPages";
            this.numericUpDownMaximumLoadAheadPages.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMaximumLoadAheadPages.TabIndex = 7;
            this.numericUpDownMaximumLoadAheadPages.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // FormLogin
            // 
            this.AcceptButton = this.buttonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 180);
            this.Controls.Add(this.numericUpDownMaximumLoadAheadPages);
            this.Controls.Add(this.labelMaxLoadAhead);
            this.Controls.Add(this.listBoxCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.textBoxUsername);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLogin";
            this.Text = "Log in";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximumLoadAheadPages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxCategory;
        private System.Windows.Forms.Label labelMaxLoadAhead;
        private System.Windows.Forms.NumericUpDown numericUpDownMaximumLoadAheadPages;
    }
}