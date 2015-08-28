namespace NewTranslator.Settings
{
	partial class OptionsPageControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbService = new System.Windows.Forms.GroupBox();
            this.cmdSwap = new System.Windows.Forms.Button();
            this.cmbTargetLanguage = new System.Windows.Forms.ComboBox();
            this.cmbSourceLanguage = new System.Windows.Forms.ComboBox();
            this.cmbService = new System.Windows.Forms.ComboBox();
            this.lblTargetLanguage = new System.Windows.Forms.Label();
            this.lblSourceLanguage = new System.Windows.Forms.Label();
            this.lblTranslationService = new System.Windows.Forms.Label();
            this.bingTranslatorGroupBox = new System.Windows.Forms.GroupBox();
            this.microsofttranslatorLinkLabel = new System.Windows.Forms.LinkLabel();
            this.txtBingClientSecret = new System.Windows.Forms.TextBox();
            this.txtBingClientId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbService.SuspendLayout();
            this.bingTranslatorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbService
            // 
            this.gbService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbService.Controls.Add(this.cmdSwap);
            this.gbService.Controls.Add(this.cmbTargetLanguage);
            this.gbService.Controls.Add(this.cmbSourceLanguage);
            this.gbService.Controls.Add(this.cmbService);
            this.gbService.Controls.Add(this.lblTargetLanguage);
            this.gbService.Controls.Add(this.lblSourceLanguage);
            this.gbService.Controls.Add(this.lblTranslationService);
            this.gbService.Location = new System.Drawing.Point(4, 4);
            this.gbService.Name = "gbService";
            this.gbService.Size = new System.Drawing.Size(394, 107);
            this.gbService.TabIndex = 0;
            this.gbService.TabStop = false;
            this.gbService.Text = "Translation service";
            // 
            // cmdSwap
            // 
            this.cmdSwap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSwap.Location = new System.Drawing.Point(340, 46);
            this.cmdSwap.Name = "cmdSwap";
            this.cmdSwap.Size = new System.Drawing.Size(48, 48);
            this.cmdSwap.TabIndex = 6;
            this.cmdSwap.Text = "Swap";
            this.cmdSwap.UseVisualStyleBackColor = true;
            this.cmdSwap.Click += new System.EventHandler(this.cmdSwap_Click);
            // 
            // cmbTargetLanguage
            // 
            this.cmbTargetLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTargetLanguage.DisplayMember = "Name";
            this.cmbTargetLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetLanguage.FormattingEnabled = true;
            this.cmbTargetLanguage.Location = new System.Drawing.Point(120, 73);
            this.cmbTargetLanguage.Name = "cmbTargetLanguage";
            this.cmbTargetLanguage.Size = new System.Drawing.Size(213, 21);
            this.cmbTargetLanguage.TabIndex = 5;
            this.cmbTargetLanguage.Tag = "";
            this.cmbTargetLanguage.ValueMember = "Code";
            this.cmbTargetLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // cmbSourceLanguage
            // 
            this.cmbSourceLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSourceLanguage.DisplayMember = "Name";
            this.cmbSourceLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceLanguage.FormattingEnabled = true;
            this.cmbSourceLanguage.Location = new System.Drawing.Point(120, 46);
            this.cmbSourceLanguage.Name = "cmbSourceLanguage";
            this.cmbSourceLanguage.Size = new System.Drawing.Size(213, 21);
            this.cmbSourceLanguage.TabIndex = 4;
            this.cmbSourceLanguage.ValueMember = "Code";
            this.cmbSourceLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // cmbService
            // 
            this.cmbService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbService.DisplayMember = "AccessibleName";
            this.cmbService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbService.FormattingEnabled = true;
            this.cmbService.Location = new System.Drawing.Point(120, 19);
            this.cmbService.Name = "cmbService";
            this.cmbService.Size = new System.Drawing.Size(268, 21);
            this.cmbService.TabIndex = 3;
            this.cmbService.ValueMember = "Name";
            this.cmbService.SelectedIndexChanged += new System.EventHandler(this.cmbService_SelectedIndexChanged);
            // 
            // lblTargetLanguage
            // 
            this.lblTargetLanguage.AutoSize = true;
            this.lblTargetLanguage.Location = new System.Drawing.Point(7, 76);
            this.lblTargetLanguage.Name = "lblTargetLanguage";
            this.lblTargetLanguage.Size = new System.Drawing.Size(85, 13);
            this.lblTargetLanguage.TabIndex = 2;
            this.lblTargetLanguage.Text = "Target language";
            // 
            // lblSourceLanguage
            // 
            this.lblSourceLanguage.AutoSize = true;
            this.lblSourceLanguage.Location = new System.Drawing.Point(7, 49);
            this.lblSourceLanguage.Name = "lblSourceLanguage";
            this.lblSourceLanguage.Size = new System.Drawing.Size(88, 13);
            this.lblSourceLanguage.TabIndex = 1;
            this.lblSourceLanguage.Text = "Source language";
            // 
            // lblTranslationService
            // 
            this.lblTranslationService.AutoSize = true;
            this.lblTranslationService.Location = new System.Drawing.Point(7, 22);
            this.lblTranslationService.Name = "lblTranslationService";
            this.lblTranslationService.Size = new System.Drawing.Size(43, 13);
            this.lblTranslationService.TabIndex = 0;
            this.lblTranslationService.Text = "Service";
            // 
            // bingTranslatorGroupBox
            // 
            this.bingTranslatorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bingTranslatorGroupBox.Controls.Add(this.microsofttranslatorLinkLabel);
            this.bingTranslatorGroupBox.Controls.Add(this.txtBingClientSecret);
            this.bingTranslatorGroupBox.Controls.Add(this.txtBingClientId);
            this.bingTranslatorGroupBox.Controls.Add(this.label2);
            this.bingTranslatorGroupBox.Controls.Add(this.label1);
            this.bingTranslatorGroupBox.Location = new System.Drawing.Point(4, 118);
            this.bingTranslatorGroupBox.Name = "bingTranslatorGroupBox";
            this.bingTranslatorGroupBox.Size = new System.Drawing.Size(394, 98);
            this.bingTranslatorGroupBox.TabIndex = 1;
            this.bingTranslatorGroupBox.TabStop = false;
            this.bingTranslatorGroupBox.Text = "Bing translator credential";
            // 
            // microsofttranslatorLinkLabel
            // 
            this.microsofttranslatorLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.microsofttranslatorLinkLabel.AutoSize = true;
            this.microsofttranslatorLinkLabel.Location = new System.Drawing.Point(324, 71);
            this.microsofttranslatorLinkLabel.Name = "microsofttranslatorLinkLabel";
            this.microsofttranslatorLinkLabel.Size = new System.Drawing.Size(64, 13);
            this.microsofttranslatorLinkLabel.TabIndex = 4;
            this.microsofttranslatorLinkLabel.TabStop = true;
            this.microsofttranslatorLinkLabel.Text = "See how to.";
            // 
            // txtBingClientSecret
            // 
            this.txtBingClientSecret.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBingClientSecret.Location = new System.Drawing.Point(120, 48);
            this.txtBingClientSecret.Name = "txtBingClientSecret";
            this.txtBingClientSecret.Size = new System.Drawing.Size(268, 20);
            this.txtBingClientSecret.TabIndex = 3;
            // 
            // txtBingClientId
            // 
            this.txtBingClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBingClientId.Location = new System.Drawing.Point(120, 22);
            this.txtBingClientId.Name = "txtBingClientId";
            this.txtBingClientId.Size = new System.Drawing.Size(268, 20);
            this.txtBingClientId.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Client secret";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client ID";
            // 
            // OptionsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bingTranslatorGroupBox);
            this.Controls.Add(this.gbService);
            this.Name = "OptionsPageControl";
            this.Size = new System.Drawing.Size(401, 257);
            this.gbService.ResumeLayout(false);
            this.gbService.PerformLayout();
            this.bingTranslatorGroupBox.ResumeLayout(false);
            this.bingTranslatorGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbService;
		private System.Windows.Forms.Label lblTargetLanguage;
		private System.Windows.Forms.Label lblSourceLanguage;
		private System.Windows.Forms.Label lblTranslationService;
		private System.Windows.Forms.ComboBox cmbTargetLanguage;
		private System.Windows.Forms.ComboBox cmbSourceLanguage;
		private System.Windows.Forms.ComboBox cmbService;
		private System.Windows.Forms.Button cmdSwap;
        private System.Windows.Forms.GroupBox bingTranslatorGroupBox;
        private System.Windows.Forms.TextBox txtBingClientSecret;
        private System.Windows.Forms.TextBox txtBingClientId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel microsofttranslatorLinkLabel;
    }
}
