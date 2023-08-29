using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Timers;

namespace KNM_Job_Rename1
{
    public partial class Form1 : Form
    {

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private ExcelPackage excelPackage;
        private int remainingSeconds; // 儲存倒數秒數
        private System.Timers.Timer countdownTimer;
        private int interval; // 儲存排程間隔

        public Form1()
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            InitializeComponent();

            // 將 button2 設定為停用
            button2.Enabled = false;
            // 設定 button2 的背景顏色為灰色
            button2.BackColor = Color.Gray;


            //倒計時的TextBox設唯獨，無框線
            //textBoxCountdown.ReadOnly = true;
            //textBoxCountdown.BorderStyle = BorderStyle.None;


            button1.Click += Button1_Click;
            buttonBrowseSource.Click += ButtonBrowseSource_Click;
            buttonBrowseTarget.Click += ButtonBrowseTarget_Click;

            excelPackage = new ExcelPackage(new FileInfo("processed_files.xlsx"));

            if (excelPackage.Workbook.Worksheets.Count == 0)
            {
                excelPackage.Workbook.Worksheets.Add("Data");
            }
            // 初始化倒數計時器，但不啟動
            countdownTimer = new System.Timers.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Elapsed += CountdownTimer_Elapsed;
        }
        private void ButtonBrowseSource_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    buttonBrowseSource.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void ButtonBrowseTarget_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    buttonBrowseTarget.Text = folderDialog.SelectedPath;
                }
            }
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            string sourceFolder = buttonBrowseSource.Text;
            string targetFolder = buttonBrowseTarget.Text;

            if (string.IsNullOrWhiteSpace(sourceFolder) || string.IsNullOrWhiteSpace(targetFolder))
            {
                MessageBox.Show("請選擇來源資料夾和目標資料夾。");
                return;
            }

            if (!int.TryParse(textBox3.Text, out interval) || interval <= 0)
            {
                MessageBox.Show("請輸入正整數的排程間隔。");
                return;
            }

            remainingSeconds = this.interval; // 設定初始倒數秒數
            countdownTimer.Start(); // 啟動倒數計時器

            button1.Enabled = false; //處理時禁用執行按鈕
            button1.BackColor = Color.Gray; // 設定背景顏色為灰色
            buttonBrowseSource.Enabled = false; // 禁用選擇來源資料夾按鈕
            buttonBrowseTarget.Enabled = false; // 禁用選擇目標資料夾按鈕
            textBox3.Enabled = false; // 禁用排程間隔輸入框
            button2.Enabled = true;  // 啟用停止按鈕
            button2.BackColor = Color.DarkOliveGreen; // 恢復預設背景顏色

            cancellationTokenSource = new CancellationTokenSource();

            await Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (InvokeRequired)
                    {
                        //從thread更新UI介面
                        BeginInvoke(new Action(() => ProcessFiles(sourceFolder, targetFolder)));
                    }
                    else
                    {
                        ProcessFiles(sourceFolder, targetFolder);
                    }
                    // 更新 button1 的文本顯示
                    if (remainingSeconds > 0)
                    {
                        remainingSeconds--;
                        BeginInvoke(new Action(() => button1.Text = remainingSeconds.ToString()));
                    }
                    else
                    {
                        countdownTimer.Stop();
                        remainingSeconds = interval;
                        countdownTimer.Start();
                        BeginInvoke(new Action(() => button1.Text = remainingSeconds.ToString()));
                        // ... 執行 ProcessFiles 或其他相關操作 ...
                    }

                    await Task.Delay(interval * 1000);
                }
                countdownTimer.Stop();
                remainingSeconds = 0;


                // 處理完成後重新啟用控制項
                BeginInvoke(new Action(() =>
                {
                    button1.Enabled = true;  // 啟用執行按鈕
                    button1.BackColor = Color.OrangeRed; // 恢復預設背景顏色
                    buttonBrowseSource.Enabled = true; // 啟用選擇來源資料夾按鈕
                    buttonBrowseTarget.Enabled = true; // 啟用選擇目標資料夾按鈕
                    textBox3.Enabled = true; // 啟用排程間隔輸入框
                    button2.Enabled = false; // 關閉停止按鈕
                    button2.BackColor = Color.Gray; // 設定背景顏色為灰色
                }));
            });

            button1.Enabled = true;  // 啟用執行按鈕
            button1.BackColor = Color.OrangeRed; // 恢復預設背景顏色
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            countdownTimer.Stop();
            button1.Enabled = true; // 啟用 button1
            button1.BackColor = Color.OrangeRed; // 恢復預設背景顏色
            button1.Font = new Font(button1.Font, FontStyle.Regular); // 設定字體為正常樣式
            button1.ForeColor = Color.White; // 恢復預設字體顏色
            button1.Text = "繼續執行"; // 將 button1 的文本設為「繼續執行」
            button2.Enabled = false; // 關閉 button2
            button2.Enabled = false; // 關閉 button2
            button2.BackColor = Color.Gray; // 設定背景顏色為灰色
            textBox3.Enabled = true; // 禁用排程間隔輸入框
        }

        private void CountdownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            remainingSeconds--;

            if (remainingSeconds >= 0)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => button1.Text = remainingSeconds.ToString()));
                }
                else
                {
                    button1.Text = remainingSeconds.ToString();
                }
            }
            else
            {
                countdownTimer.Stop();
                remainingSeconds = interval;
                countdownTimer.Start();
                Console.WriteLine($"Remaining seconds: {remainingSeconds}");

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => button1.Text = remainingSeconds.ToString()));
                }
                else
                {
                    button1.Text = remainingSeconds.ToString();
                }
            }
        }

        private void ProcessFiles(string sourceFolder, string targetFolder)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
            int startRow = worksheet.Dimension?.Rows ?? 1;

            // Add headers if not already present
            if (startRow == 1)
            {
                worksheet.Cells[1, 1].Value = "已轉文件名";
                worksheet.Cells[1, 2].Value = "轉換前文件名";
                worksheet.Cells[1, 3].Value = "轉換後文件名";
                startRow++; // Move to the next row
            }

            foreach (string xmlFilePath in Directory.GetFiles(sourceFolder, "*.xml", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileName(xmlFilePath);

                // Check if the filename already exists in column A
                if (ProcessedFilesContains(filename))
                {
                    Console.WriteLine($"File {filename} already processed, skipping.");
                    continue; // Skip this file
                }

                string xmlEncoding = GetXmlEncoding(xmlFilePath);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFilePath);
                XmlElement rootElement = xmlDocument.DocumentElement;

                string pdfFilename = filename.Replace(".xml", ".pdf");
                string pdfPath = Path.Combine(Path.GetDirectoryName(xmlFilePath), pdfFilename);

                if (File.Exists(pdfPath))
                {
                    string username = GetNodeText(rootElement, "username");
                    string jobType = GetNodeText(rootElement, "jobType");
                    string timestamp = GetNodeText(rootElement, "timestamp").Replace(":", "-").Replace(" ", "_");
                    string fullname = GetNodeText(rootElement, "fullname");

                    fullname = Regex.Replace(fullname, @"[\/:*?""<>|]", "_");

                    string newPdfName = $"{username}_{jobType}_{timestamp}_{fullname}.pdf";
                    newPdfName = Regex.Replace(newPdfName, @"[\/:*?""<>|]", "_");

                    string newPdfPath = Path.Combine(targetFolder, newPdfName);
                    File.Copy(pdfPath, newPdfPath, true); // Overwrite existing file

                    Console.WriteLine($"Copied {pdfFilename} to {newPdfName}");

                    // Add data to Excel
                    worksheet.Cells[startRow, 1].Value = filename;
                    worksheet.Cells[startRow, 2].Value = pdfFilename;
                    worksheet.Cells[startRow, 3].Value = newPdfName;
                    excelPackage.Save();

                    startRow++; // Move to the next row
                }
                else
                {
                    Console.WriteLine($"Corresponding PDF file for {filename} not found.");
                }
            }
        }

        private bool ProcessedFilesContains(string filename)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension?.Rows ?? 0;

            for (int row = 2; row <= rowCount; row++) // Start from the second row (skipping headers)
            {
                if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() == filename)
                {
                    return true; // File already processed
                }
            }

            return false; // File not yet processed
        }



        private string GetNodeText(XmlElement element, string nodeName)
        {
            XmlNode node = element.SelectSingleNode(nodeName);
            return node?.InnerText ?? "使用者空";
        }

        private string GetXmlEncoding(string xmlPath)
        {
            using (StreamReader reader = new StreamReader(xmlPath))
            {
                string firstLine = reader.ReadLine();
                Match match = Regex.Match(firstLine, @"encoding=[""']([^""']+)[""']");
                if (match.Success)
                {
                    string encoding = match.Groups[1].Value;
                    return encoding;
                }
            }
            return "utf-8"; // Default to utf-8 encoding
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
