namespace Persondata_o_matic
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
            this.labelMaxLoadAhead = new System.Windows.Forms.Label();
            this.numericUpDownMaximumLoadAheadPages = new System.Windows.Forms.NumericUpDown();
            this.labelMaxCategoryPages = new System.Windows.Forms.Label();
            this.numericUpDownMaximumCategoryPages = new System.Windows.Forms.NumericUpDown();
            this.checkBoxRandomOrder = new System.Windows.Forms.CheckBox();
            this.comboBoxPageListSourceCategory = new System.Windows.Forms.ComboBox();
            this.textBoxPageListSourceFileName = new System.Windows.Forms.TextBox();
            this.radioButtonCategory = new System.Windows.Forms.RadioButton();
            this.radioButtonFile = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximumLoadAheadPages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximumCategoryPages)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(97, 19);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(140, 20);
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
            this.textBoxPassword.Size = new System.Drawing.Size(140, 20);
            this.textBoxPassword.TabIndex = 3;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(237, 184);
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
            this.label1.Location = new System.Drawing.Point(14, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Page list &source:";
            // 
            // labelMaxLoadAhead
            // 
            this.labelMaxLoadAhead.AutoSize = true;
            this.labelMaxLoadAhead.Location = new System.Drawing.Point(14, 148);
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
            this.numericUpDownMaximumLoadAheadPages.Location = new System.Drawing.Point(173, 146);
            this.numericUpDownMaximumLoadAheadPages.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownMaximumLoadAheadPages.Name = "numericUpDownMaximumLoadAheadPages";
            this.numericUpDownMaximumLoadAheadPages.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMaximumLoadAheadPages.TabIndex = 7;
            this.numericUpDownMaximumLoadAheadPages.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelMaxCategoryPages
            // 
            this.labelMaxCategoryPages.AutoSize = true;
            this.labelMaxCategoryPages.Location = new System.Drawing.Point(14, 122);
            this.labelMaxCategoryPages.Name = "labelMaxCategoryPages";
            this.labelMaxCategoryPages.Size = new System.Drawing.Size(136, 13);
            this.labelMaxCategoryPages.TabIndex = 6;
            this.labelMaxCategoryPages.Text = "M&aximum pages to retrieve:";
            // 
            // numericUpDownMaximumCategoryPages
            // 
            this.numericUpDownMaximumCategoryPages.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownMaximumCategoryPages.Location = new System.Drawing.Point(173, 120);
            this.numericUpDownMaximumCategoryPages.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownMaximumCategoryPages.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMaximumCategoryPages.Name = "numericUpDownMaximumCategoryPages";
            this.numericUpDownMaximumCategoryPages.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMaximumCategoryPages.TabIndex = 7;
            this.numericUpDownMaximumCategoryPages.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // checkBoxRandomOrder
            // 
            this.checkBoxRandomOrder.AutoSize = true;
            this.checkBoxRandomOrder.Checked = true;
            this.checkBoxRandomOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRandomOrder.Location = new System.Drawing.Point(304, 123);
            this.checkBoxRandomOrder.Name = "checkBoxRandomOrder";
            this.checkBoxRandomOrder.Size = new System.Drawing.Size(106, 17);
            this.checkBoxRandomOrder.TabIndex = 9;
            this.checkBoxRandomOrder.Text = "Randomize order";
            this.checkBoxRandomOrder.UseVisualStyleBackColor = true;
            // 
            // comboBoxPageListSourceCategory
            // 
            this.comboBoxPageListSourceCategory.FormattingEnabled = true;
            this.comboBoxPageListSourceCategory.Items.AddRange(new object[] {
            "Category:Persondata templates without name parameter",
            "Category:Persondata templates without short description parameter"});
            this.comboBoxPageListSourceCategory.Location = new System.Drawing.Point(182, 71);
            this.comboBoxPageListSourceCategory.Name = "comboBoxPageListSourceCategory";
            this.comboBoxPageListSourceCategory.Size = new System.Drawing.Size(342, 21);
            this.comboBoxPageListSourceCategory.TabIndex = 10;
            this.comboBoxPageListSourceCategory.Text = "Category:Persondata templates without short description parameter";
            // 
            // textBoxPageListSourceFileName
            // 
            this.textBoxPageListSourceFileName.Location = new System.Drawing.Point(183, 94);
            this.textBoxPageListSourceFileName.Name = "textBoxPageListSourceFileName";
            this.textBoxPageListSourceFileName.Size = new System.Drawing.Size(342, 20);
            this.textBoxPageListSourceFileName.TabIndex = 12;
            this.textBoxPageListSourceFileName.Text = "pages.txt";
            // 
            // radioButtonCategory
            // 
            this.radioButtonCategory.AutoSize = true;
            this.radioButtonCategory.Checked = true;
            this.radioButtonCategory.Location = new System.Drawing.Point(106, 72);
            this.radioButtonCategory.Name = "radioButtonCategory";
            this.radioButtonCategory.Size = new System.Drawing.Size(70, 17);
            this.radioButtonCategory.TabIndex = 13;
            this.radioButtonCategory.TabStop = true;
            this.radioButtonCategory.Text = "Category:";
            this.radioButtonCategory.UseVisualStyleBackColor = true;
            // 
            // radioButtonFile
            // 
            this.radioButtonFile.AutoSize = true;
            this.radioButtonFile.Location = new System.Drawing.Point(106, 95);
            this.radioButtonFile.Name = "radioButtonFile";
            this.radioButtonFile.Size = new System.Drawing.Size(65, 17);
            this.radioButtonFile.TabIndex = 14;
            this.radioButtonFile.Text = "Text file:";
            this.radioButtonFile.UseVisualStyleBackColor = true;
            // 
            // FormLogin
            // 
            this.AcceptButton = this.buttonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 219);
            this.Controls.Add(this.radioButtonFile);
            this.Controls.Add(this.radioButtonCategory);
            this.Controls.Add(this.textBoxPageListSourceFileName);
            this.Controls.Add(this.comboBoxPageListSourceCategory);
            this.Controls.Add(this.checkBoxRandomOrder);
            this.Controls.Add(this.numericUpDownMaximumCategoryPages);
            this.Controls.Add(this.labelMaxCategoryPages);
            this.Controls.Add(this.numericUpDownMaximumLoadAheadPages);
            this.Controls.Add(this.labelMaxLoadAhead);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximumCategoryPages)).EndInit();
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
        private System.Windows.Forms.Label labelMaxLoadAhead;
        private System.Windows.Forms.NumericUpDown numericUpDownMaximumLoadAheadPages;
        private System.Windows.Forms.Label labelMaxCategoryPages;
        private System.Windows.Forms.NumericUpDown numericUpDownMaximumCategoryPages;
        private System.Windows.Forms.CheckBox checkBoxRandomOrder;
        private System.Windows.Forms.ComboBox comboBoxPageListSourceCategory;
        private System.Windows.Forms.TextBox textBoxPageListSourceFileName;
        private System.Windows.Forms.RadioButton radioButtonCategory;
        private System.Windows.Forms.RadioButton radioButtonFile;
    }
}