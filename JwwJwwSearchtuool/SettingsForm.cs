using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JWWCADFileSearchTool
{
    public partial class SettingsForm : Form
    {
        public string JWWCADPath { get; set; }
        public string JacConvertPath { get; set; }
        public bool EnableColorOutput { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // 現在の設定値をフォームに反映
            textBoxJWWCADPath.Text = JWWCADPath;
            textBoxJacConvertPath.Text = JacConvertPath;
            checkBoxEnableColorOutput.Checked = EnableColorOutput;
        }

        private void buttonBrowseJWWCAD_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
                openFileDialog.Title = "JWWCADの実行ファイルを選択";

                if (!string.IsNullOrEmpty(textBoxJWWCADPath.Text) && File.Exists(textBoxJWWCADPath.Text))
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(textBoxJWWCADPath.Text);
                    openFileDialog.FileName = Path.GetFileName(textBoxJWWCADPath.Text);
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxJWWCADPath.Text = openFileDialog.FileName;
                }
            }
        }

        private void buttonBrowseJacConvert_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
                openFileDialog.Title = "JacConvertの実行ファイルを選択";

                if (!string.IsNullOrEmpty(textBoxJacConvertPath.Text) && File.Exists(textBoxJacConvertPath.Text))
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(textBoxJacConvertPath.Text);
                    openFileDialog.FileName = Path.GetFileName(textBoxJacConvertPath.Text);
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxJacConvertPath.Text = openFileDialog.FileName;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // 入力値をプロパティに設定
            JWWCADPath = textBoxJWWCADPath.Text;
            JacConvertPath = textBoxJacConvertPath.Text;
            EnableColorOutput = checkBoxEnableColorOutput.Checked;

            // 入力値の検証
            if (!string.IsNullOrEmpty(JWWCADPath) && !File.Exists(JWWCADPath))
            {
                MessageBox.Show("JWWCADの実行ファイルが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(JacConvertPath) && !File.Exists(JacConvertPath))
            {
                MessageBox.Show("JacConvertの実行ファイルが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}