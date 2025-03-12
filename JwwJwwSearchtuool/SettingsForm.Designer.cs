namespace JWWCADFileSearchTool
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxJWWCADPath = new System.Windows.Forms.TextBox();
            this.buttonBrowseJWWCAD = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxJacConvertPath = new System.Windows.Forms.TextBox();
            this.buttonBrowseJacConvert = new System.Windows.Forms.Button();
            this.checkBoxEnableColorOutput = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "JWWCADパス：";
            // 
            // textBoxJWWCADPath
            // 
            this.textBoxJWWCADPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxJWWCADPath.Location = new System.Drawing.Point(109, 31);
            this.textBoxJWWCADPath.Name = "textBoxJWWCADPath";
            this.textBoxJWWCADPath.Size = new System.Drawing.Size(297, 19);
            this.textBoxJWWCADPath.TabIndex = 1;
            // 
            // buttonBrowseJWWCAD
            // 
            this.buttonBrowseJWWCAD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseJWWCAD.Location = new System.Drawing.Point(412, 29);
            this.buttonBrowseJWWCAD.Name = "buttonBrowseJWWCAD";
            this.buttonBrowseJWWCAD.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseJWWCAD.TabIndex = 2;
            this.buttonBrowseJWWCAD.Text = "参照...";
            this.buttonBrowseJWWCAD.UseVisualStyleBackColor = true;
            this.buttonBrowseJWWCAD.Click += new System.EventHandler(this.buttonBrowseJWWCAD_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "JacConvertパス：";
            // 
            // textBoxJacConvertPath
            // 
            this.textBoxJacConvertPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxJacConvertPath.Location = new System.Drawing.Point(109, 71);
            this.textBoxJacConvertPath.Name = "textBoxJacConvertPath";
            this.textBoxJacConvertPath.Size = new System.Drawing.Size(297, 19);
            this.textBoxJacConvertPath.TabIndex = 4;
            // 
            // buttonBrowseJacConvert
            // 
            this.buttonBrowseJacConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseJacConvert.Location = new System.Drawing.Point(412, 69);
            this.buttonBrowseJacConvert.Name = "buttonBrowseJacConvert";
            this.buttonBrowseJacConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseJacConvert.TabIndex = 5;
            this.buttonBrowseJacConvert.Text = "参照...";
            this.buttonBrowseJacConvert.UseVisualStyleBackColor = true;
            this.buttonBrowseJacConvert.Click += new System.EventHandler(this.buttonBrowseJacConvert_Click);
            // 
            // checkBoxEnableColorOutput
            // 
            this.checkBoxEnableColorOutput.AutoSize = true;
            this.checkBoxEnableColorOutput.Checked = true;
            this.checkBoxEnableColorOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableColorOutput.Location = new System.Drawing.Point(109, 114);
            this.checkBoxEnableColorOutput.Name = "checkBoxEnableColorOutput";
            this.checkBoxEnableColorOutput.Size = new System.Drawing.Size(117, 16);
            this.checkBoxEnableColorOutput.TabIndex = 6;
            this.checkBoxEnableColorOutput.Text = "カラー出力を有効化";
            this.checkBoxEnableColorOutput.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(331, 184);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(412, 184);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxEnableColorOutput);
            this.groupBox1.Controls.Add(this.buttonBrowseJacConvert);
            this.groupBox1.Controls.Add(this.textBoxJacConvertPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonBrowseJWWCAD);
            this.groupBox1.Controls.Add(this.textBoxJWWCADPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 151);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "アプリケーション設定";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(524, 219);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "設定";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxJWWCADPath;
        private System.Windows.Forms.Button buttonBrowseJWWCAD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxJacConvertPath;
        private System.Windows.Forms.Button buttonBrowseJacConvert;
        private System.Windows.Forms.CheckBox checkBoxEnableColorOutput;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}