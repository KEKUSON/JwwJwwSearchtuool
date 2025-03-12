using System;
using System.Windows.Forms;

namespace JwwJwwSearchtuool  // プロジェクト名に合わせて変更してください
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            // 既存の設定をロード
            textBoxJwwPath.Text = Properties.Settings.Default.JwwPath;
            textBoxJacPath.Text = Properties.Settings.Default.JacConvertPath;
            checkBoxColorOutput.Checked = Properties.Settings.Default.UseColorOutput;
        }

        // JWWCADパス参照ボタン
        private void buttonBrowseJww_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "実行ファイル (*.exe)|*.exe";
                dialog.Title = "JWWCADの実行ファイルを選択";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxJwwPath.Text = dialog.FileName;
                }
            }
        }

        // JacConvertパス参照ボタン
        private void buttonBrowseJac_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "実行ファイル (*.exe)|*.exe";
                dialog.Title = "JacConvertの実行ファイルを選択";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxJacPath.Text = dialog.FileName;
                }
            }
        }

        // OKボタン
        private void buttonOK_Click(object sender, EventArgs e)
        {
            // 設定を保存
            Properties.Settings.Default.JwwPath = textBoxJwwPath.Text;
            Properties.Settings.Default.JacConvertPath = textBoxJacPath.Text;
            Properties.Settings.Default.UseColorOutput = checkBoxColorOutput.Checked;
            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        // キャンセルボタン
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}