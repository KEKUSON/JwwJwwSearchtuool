using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using PdfiumViewer;

namespace JWWCADFileSearchTool
{
    public partial class Form1 : Form
    {
        // ソート用変数
        private int sortColumn = -1;
        private bool ascending = true;

        // プレビュー用変数
        private Image previewImage = null;
        private float zoomFactor = 1.0f;
        private Point lastMousePosition;
        private bool isDragging = false;

        // 設定値
        private string jwwCadPath = "";
        private string jacConvertPath = "";
        private bool enableColorOutput = true;
        private AppSettings appSettings = new AppSettings();
        public Form1()
        {
            InitializeComponent();
            LoadSettings();
            InitializeListView();
        }

        #region 初期化メソッド

        private void LoadSettings()
        {
            // 設定の読み込み
            appSettings = AppSettings.Load();
            jwwCadPath = appSettings.JWWCADPath;
            jacConvertPath = appSettings.JacConvertPath;
            enableColorOutput = appSettings.EnableColorOutput;
        }
        private void InitializeListView()
        {
            // ListViewの初期設定
            listViewResults.View = View.Details;
            listViewResults.FullRowSelect = true;
            listViewResults.GridLines = true;
            listViewResults.Sorting = SortOrder.Ascending;

            // カラムの追加
            listViewResults.Columns.Add("ファイル名", 200);
            listViewResults.Columns.Add("パス", 300);
            listViewResults.Columns.Add("サイズ", 80, HorizontalAlignment.Right);
            listViewResults.Columns.Add("更新日時", 150);
        }
        private void PreviewPDFAsImage(string pdfPath)
        {
            try
            {
                using (var document = PdfDocument.Load(pdfPath))
                {
                    // GetPageの代わりにRenderメソッドを使用
                    using (var bitmap = document.Render(0, 800, 1000, false))
                    {
                        pictureBoxPreview.Image = bitmap;
                        pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDFプレビューエラー: {ex.Message}");
            }
        }
        #endregion

        #region 検索機能

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            // 検索前に結果をクリア
            listViewResults.Items.Clear();
            previewImage = null;
            pictureBoxPreview.Image = null;

            // 検索パスのチェック
            if (string.IsNullOrEmpty(textBoxSearchPath.Text) || !Directory.Exists(textBoxSearchPath.Text))
            {
                MessageBox.Show("有効な検索フォルダを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 検索開始
            Cursor = Cursors.WaitCursor;
            toolStripStatusLabel.Text = "検索中...";
            Application.DoEvents();

            try
            {
                SearchJWWFiles(textBoxSearchPath.Text, textBoxKeyword.Text, checkBoxIncludeSubfolders.Checked);
                toolStripStatusLabel.Text = $"検索完了: {listViewResults.Items.Count} 件見つかりました";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"検索中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "エラーが発生しました";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SearchJWWFiles(string searchPath, string keyword, bool includeSubfolders)
        {
            SearchOption option = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            // JWWファイルの検索
            string[] jwwFiles = Directory.GetFiles(searchPath, "*.jww", option);
            foreach (string file in jwwFiles)
            {
                ProcessFile(file, keyword);
            }

            // ZIPファイル内のJWWファイル検索（オプション）
            if (checkBoxSearchZip.Checked)
            {
                string[] zipFiles = Directory.GetFiles(searchPath, "*.zip", option);
                foreach (string zipFile in zipFiles)
                {
                    SearchJWWInZip(zipFile, keyword);
                }
            }
        }

        private void ProcessFile(string filePath, string keyword)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            // キーワードが指定されていない場合、またはファイル名にキーワードが含まれる場合
            if (string.IsNullOrEmpty(keyword) ||
                fileInfo.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // ListViewに追加
                ListViewItem item = new ListViewItem(fileInfo.Name);
                item.SubItems.Add(fileInfo.DirectoryName);
                item.SubItems.Add(FormatFileSize(fileInfo.Length));
                item.SubItems.Add(fileInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));
                item.Tag = filePath; // ファイルパスをTagに保存

                listViewResults.Items.Add(item);
            }
        }

        private void SearchJWWInZip(string zipFilePath, string keyword)
        {
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.EndsWith(".jww", StringComparison.OrdinalIgnoreCase))
                        {
                            // キーワードが指定されていない場合、またはエントリ名にキーワードが含まれる場合
                            if (string.IsNullOrEmpty(keyword) ||
                                entry.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                // ListViewに追加
                                ListViewItem item = new ListViewItem(entry.Name);
                                item.SubItems.Add($"[ZIP] {zipFilePath}");
                                item.SubItems.Add(FormatFileSize(entry.Length));
                                item.SubItems.Add(entry.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));
                                item.Tag = $"{zipFilePath}|{entry.FullName}"; // ZIPパスとエントリパスをTagに保存

                                listViewResults.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ZIPファイル処理エラー: {ex.Message}");
                // エラーを無視して処理を続行
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        #endregion

        #region リストビュー操作

        private void listViewResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 同じカラムがクリックされた場合は昇順/降順を切り替え
            if (e.Column == sortColumn)
            {
                ascending = !ascending;
            }
            else
            {
                // 別のカラムがクリックされた場合は、そのカラムで昇順に並び替え
                sortColumn = e.Column;
                ascending = true;
            }

            // 並び替えの実行
            listViewResults.ListViewItemSorter = new ListViewItemComparer(e.Column, ascending);
        }

        private void listViewResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewResults.SelectedItems.Count > 0)
            {
                string filePath = listViewResults.SelectedItems[0].Tag.ToString();

                // ZIPファイル内のJWWファイルかどうかチェック
                if (filePath.Contains("|"))
                {
                    string[] parts = filePath.Split('|');
                    string zipPath = parts[0];
                    string entryPath = parts[1];

                    // 一時ファイルとして展開
                    string tempFile = ExtractFileFromZip(zipPath, entryPath);
                    if (!string.IsNullOrEmpty(tempFile))
                    {
                        DisplayPreview(tempFile);
                    }
                }
                else
                {
                    // 通常のJWWファイル
                    DisplayPreview(filePath);
                }
            }
            else
            {
                // 選択がクリアされた場合
                previewImage = null;
                pictureBoxPreview.Image = null;
            }
        }

        private string ExtractFileFromZip(string zipPath, string entryPath)
        {
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "JWWCADFileSearchTool");
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                string tempFile = Path.Combine(tempDir, Path.GetFileName(entryPath));

                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    ZipArchiveEntry entry = archive.GetEntry(entryPath);
                    if (entry != null)
                    {
                        // 既存のファイルを削除
                        if (File.Exists(tempFile))
                        {
                            File.Delete(tempFile);
                        }

                        // ファイルを展開
                        entry.ExtractToFile(tempFile);
                        return tempFile;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ZIP展開エラー: {ex.Message}");
                MessageBox.Show($"ZIPファイルからの展開中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        #endregion

        #region プレビュー機能

        private void DisplayPreview(string filePath)
        {
            try
            {
                // プレビュー方法によって処理を分岐
                if (radioButtonPDF.Checked)
                {
                    // JacConvertを使用してPDFプレビュー
                    GeneratePDFPreview(filePath);
                }
                else
                {
                    // JWWCADを使用して画像プレビュー
                    GenerateImagePreview(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"プレビュー生成中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 画像ファイルかどうかを判定するヘルパーメソッド
        private bool IsImageFile(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };
            string ext = Path.GetExtension(filePath).ToLower();
            return imageExtensions.Contains(ext);
        }
        private void GenerateImagePreview(string filePath)
        {
            // JWWCADパスのチェック
            if (string.IsNullOrEmpty(jwwCadPath) || !File.Exists(jwwCadPath))
            {
                MessageBox.Show("JWWCADのパスが設定されていません。設定メニューから設定してください。", "設定エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 一時ファイルパスの作成
            string tempDir = Path.Combine(Path.GetTempPath(), "JWWCADFileSearchTool");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string tempImagePath = Path.Combine(tempDir, $"{Path.GetFileNameWithoutExtension(filePath)}_preview.png");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = jwwCadPath,
                Arguments = $"\"{filePath}\" \"{tempImagePath}\"", // /p を削除
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit(10000); // 最大10秒待機
            }

            // 生成された画像をロード
            if (File.Exists(tempImagePath))
            {
                using (FileStream fs = new FileStream(tempImagePath, FileMode.Open, FileAccess.Read))
                {
                    previewImage = Image.FromStream(fs);
                    zoomFactor = 1.0f; // ズーム倍率をリセット
                    pictureBoxPreview.Image = previewImage;
                }
            }
            else
            {
                previewImage = null;
                pictureBoxPreview.Image = null;
                MessageBox.Show("プレビュー画像の生成に失敗しました。", "プレビューエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GeneratePDFPreview(string filePath)
        {
            try
            {
                // 一時ファイルパスの作成
                string tempDir = Path.Combine(Path.GetTempPath(), "JWWCADFileSearchTool");
                Directory.CreateDirectory(tempDir);

                string tempPdfPath = Path.Combine(tempDir, $"{Path.GetFileNameWithoutExtension(filePath)}_preview.pdf");

                // 既存のPDFファイルを削除
                if (File.Exists(tempPdfPath))
                {
                    File.Delete(tempPdfPath);
                }

                // JacConvertを起動してPDFを生成するコマンドライン
                // コマンドライン引数を確認し、印刷コマンドがないことを確認
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = jacConvertPath,
                    // "P"オプションが印刷を意味している可能性があるので、他のオプションを確認するか削除
                    Arguments = $"\"{filePath}\" \"{tempPdfPath}\" {(enableColorOutput ? "C" : "M")}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(psi))
                {
                    // プロセスの出力を読み取る
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // タイムアウト設定を追加
                    if (!process.WaitForExit(15000)) // 15秒待機
                    {
                        process.Kill();
                        MessageBox.Show("PDF生成がタイムアウトしました。", "エラー");
                        return;
                    }

                    // 終了コードをチェック
                    if (process.ExitCode != 0)
                    {
                        Debug.WriteLine($"PDF生成プロセスが異常終了しました。終了コード: {process.ExitCode}");
                        Debug.WriteLine($"標準出力: {output}");
                        Debug.WriteLine($"エラー出力: {error}");
                    }
                }

                // PDFファイルの存在確認
                if (!File.Exists(tempPdfPath) || new FileInfo(tempPdfPath).Length == 0)
                {
                    MessageBox.Show("PDFの生成に失敗しました。", "エラー");
                    return;
                }

                try
                {
                    // PDFをイメージとして読み込み - 印刷せずに表示する
                    using (var document = PdfDocument.Load(tempPdfPath))
                    {
                        if (document.PageCount == 0)
                        {
                            MessageBox.Show("PDFにページがありません。", "エラー");
                            return;
                        }

                        // 最初のページをレンダリング
                        using (var bitmap = document.Render(0, 800, 1000, false))
                        {
                            // メインスレッドでUIを更新
                            this.Invoke((MethodInvoker)delegate
                            {
                                pictureBoxPreview.Image = bitmap;
                                pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PDFのプレビュー表示に失敗しました: {ex.Message}", "エラー");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF生成中にエラーが発生しました: {ex.Message}", "エラー");
            }
        }
        #endregion

        #region ズーム機能


        private void ApplyZoom(Point mousePosition, float oldZoom)
        {
            if (previewImage == null) return;

            // 新しいサイズの計算
            int newWidth = (int)(previewImage.Width * zoomFactor);
            int newHeight = (int)(previewImage.Height * zoomFactor);

            // 表示画像の再生成
            Bitmap zoomedImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(zoomedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(previewImage, 0, 0, newWidth, newHeight);
            }

            pictureBoxPreview.Image = zoomedImage;
            pictureBoxPreview.Refresh();

            // ズーム比率の表示
            toolStripStatusLabel.Text = $"ズーム: {zoomFactor:P0}";
        }

        private void pictureBoxPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && previewImage != null)
            {
                isDragging = true;
                lastMousePosition = e.Location;
                pictureBoxPreview.Cursor = Cursors.Hand;
            }
        }

        private void pictureBoxPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // 画像のパン（ドラッグによる移動）
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                // 親パネルのAutoScrollPositionを使用する
                Panel panel = pictureBoxPreview.Parent as Panel;
                if (panel != null)
                {
                    Point currentPosition = panel.AutoScrollPosition;
                    // AutoScrollPositionは負の値でセットする必要がある
                    panel.AutoScrollPosition = new Point(
                        -(currentPosition.X + deltaX),
                        -(currentPosition.Y + deltaY)
                    );
                }

                lastMousePosition = e.Location;
            }
        }

        private void pictureBoxPreview_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            pictureBoxPreview.Cursor = Cursors.Default;
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (previewImage != null)
            {
                zoomFactor = 1.0f;
                pictureBoxPreview.Image = previewImage;
                pictureBoxPreview.Refresh();
                toolStripStatusLabel.Text = "ズーム: 100%";
            }
        }
        private void pictureBoxPreview_MouseWheel(object sender, MouseEventArgs e)
        {
            if (previewImage == null) return;

            float oldZoom = zoomFactor;

            // ズーム倍率の調整（ホイール上: 拡大、ホイール下: 縮小）
            if (e.Delta > 0)
                zoomFactor = Math.Min(zoomFactor * 1.25f, 10.0f); // 最大10倍まで
            else
                zoomFactor = Math.Max(zoomFactor / 1.25f, 0.1f);  // 最小0.1倍まで

            // ズーム適用
            ApplyZoom(e.Location, oldZoom);
        }


        #endregion

        #region 設定関連

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 設定フォームを表示
            using (SettingsForm settingsForm = new SettingsForm())
            {
                // 現在の設定を反映
                settingsForm.JWWCADPath = jwwCadPath;
                settingsForm.JacConvertPath = jacConvertPath;
                settingsForm.EnableColorOutput = enableColorOutput;

                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // 設定が変更された場合、新しい値を取得
                    jwwCadPath = settingsForm.JWWCADPath;
                    jacConvertPath = settingsForm.JacConvertPath;
                    enableColorOutput = settingsForm.EnableColorOutput;

                    // 設定を保存
                    appSettings.JWWCADPath = jwwCadPath;
                    appSettings.JacConvertPath = jacConvertPath;
                    appSettings.EnableColorOutput = enableColorOutput;
                    appSettings.Save();
                }
            }
        }


        #endregion

                    #region その他イベントハンドラ

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "検索するフォルダを選択してください";
                if (!string.IsNullOrEmpty(textBoxSearchPath.Text) && Directory.Exists(textBoxSearchPath.Text))
                {
                    folderDialog.SelectedPath = textBoxSearchPath.Text;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxSearchPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "JWWCADファイル検索ツール\nバージョン 1.0\n\n© 2025 Your Company",
                "バージョン情報",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 一時ファイルの削除などのクリーンアップ処理
            string tempDir = Path.Combine(Path.GetTempPath(), "JWWCADFileSearchTool");
            if (Directory.Exists(tempDir))
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // 削除に失敗しても終了を妨げない
                }
            }
        }

        #endregion
    }

    #region ListViewItemComparer クラス

    // ListViewのソート用カスタムコンパレータ
    public class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;
        private bool ascending;

        public ListViewItemComparer(int column, bool asc)
        {
            col = column;
            ascending = asc;
        }

        public int Compare(object x, object y)
        {
            ListViewItem itemX = (ListViewItem)x;
            ListViewItem itemY = (ListViewItem)y;

            // ソート方向
            int direction = ascending ? 1 : -1;

            // カラムによって比較方法を変える
            if (col == 2) // サイズ列は数値として比較
            {
                string sizeX = itemX.SubItems[col].Text;
                string sizeY = itemY.SubItems[col].Text;

                // 単位を除去して数値化
                double numX = ParseFileSize(sizeX);
                double numY = ParseFileSize(sizeY);

                return direction * numX.CompareTo(numY);
            }
            else if (col == 3) // 日時列は日付として比較
            {
                DateTime dateX, dateY;

                if (DateTime.TryParse(itemX.SubItems[col].Text, out dateX) &&
                    DateTime.TryParse(itemY.SubItems[col].Text, out dateY))
                {
                    return direction * dateX.CompareTo(dateY);
                }
            }

            // その他は文字列として比較
            return direction * string.Compare(itemX.SubItems[col].Text, itemY.SubItems[col].Text);
        }

        private double ParseFileSize(string sizeText)
        {
            try
            {
                // 単位を除去して数値部分を取得
                string[] parts = sizeText.Split(' ');
                double size = double.Parse(parts[0]);

                // 単位によって倍率を適用
                string unit = parts[1].ToUpper();
                switch (unit)
                {
                    case "KB": return size * 1024;
                    case "MB": return size * 1024 * 1024;
                    case "GB": return size * 1024 * 1024 * 1024;
                    default: return size; // B（バイト）
                }
            }
            catch
            {
                return 0;
            }
        }
    }

    #endregion
}