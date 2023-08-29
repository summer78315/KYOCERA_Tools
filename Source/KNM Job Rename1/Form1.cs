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
        private int remainingSeconds; // �x�s�˼Ƭ��
        private System.Timers.Timer countdownTimer;
        private int interval; // �x�s�Ƶ{���j

        public Form1()
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            InitializeComponent();

            // �N button2 �]�w������
            button2.Enabled = false;
            // �]�w button2 ���I���C�⬰�Ǧ�
            button2.BackColor = Color.Gray;


            //�˭p�ɪ�TextBox�]�߿W�A�L�ؽu
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
            // ��l�ƭ˼ƭp�ɾ��A�����Ұ�
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
                MessageBox.Show("�п�ܨӷ���Ƨ��M�ؼи�Ƨ��C");
                return;
            }

            if (!int.TryParse(textBox3.Text, out interval) || interval <= 0)
            {
                MessageBox.Show("�п�J����ƪ��Ƶ{���j�C");
                return;
            }

            remainingSeconds = this.interval; // �]�w��l�˼Ƭ��
            countdownTimer.Start(); // �Ұʭ˼ƭp�ɾ�

            button1.Enabled = false; //�B�z�ɸT�ΰ�����s
            button1.BackColor = Color.Gray; // �]�w�I���C�⬰�Ǧ�
            buttonBrowseSource.Enabled = false; // �T�ο�ܨӷ���Ƨ����s
            buttonBrowseTarget.Enabled = false; // �T�ο�ܥؼи�Ƨ����s
            textBox3.Enabled = false; // �T�αƵ{���j��J��
            button2.Enabled = true;  // �ҥΰ�����s
            button2.BackColor = Color.DarkOliveGreen; // ��_�w�]�I���C��

            cancellationTokenSource = new CancellationTokenSource();

            await Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (InvokeRequired)
                    {
                        //�qthread��sUI����
                        BeginInvoke(new Action(() => ProcessFiles(sourceFolder, targetFolder)));
                    }
                    else
                    {
                        ProcessFiles(sourceFolder, targetFolder);
                    }
                    // ��s button1 ���奻���
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
                        // ... ���� ProcessFiles �Ψ�L�����ާ@ ...
                    }

                    await Task.Delay(interval * 1000);
                }
                countdownTimer.Stop();
                remainingSeconds = 0;


                // �B�z�����᭫�s�ҥα��
                BeginInvoke(new Action(() =>
                {
                    button1.Enabled = true;  // �ҥΰ�����s
                    button1.BackColor = Color.OrangeRed; // ��_�w�]�I���C��
                    buttonBrowseSource.Enabled = true; // �ҥο�ܨӷ���Ƨ����s
                    buttonBrowseTarget.Enabled = true; // �ҥο�ܥؼи�Ƨ����s
                    textBox3.Enabled = true; // �ҥαƵ{���j��J��
                    button2.Enabled = false; // ����������s
                    button2.BackColor = Color.Gray; // �]�w�I���C�⬰�Ǧ�
                }));
            });

            button1.Enabled = true;  // �ҥΰ�����s
            button1.BackColor = Color.OrangeRed; // ��_�w�]�I���C��
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            countdownTimer.Stop();
            button1.Enabled = true; // �ҥ� button1
            button1.BackColor = Color.OrangeRed; // ��_�w�]�I���C��
            button1.Font = new Font(button1.Font, FontStyle.Regular); // �]�w�r�鬰���`�˦�
            button1.ForeColor = Color.White; // ��_�w�]�r���C��
            button1.Text = "�~�����"; // �N button1 ���奻�]���u�~�����v
            button2.Enabled = false; // ���� button2
            button2.Enabled = false; // ���� button2
            button2.BackColor = Color.Gray; // �]�w�I���C�⬰�Ǧ�
            textBox3.Enabled = true; // �T�αƵ{���j��J��
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
                worksheet.Cells[1, 1].Value = "�w����W";
                worksheet.Cells[1, 2].Value = "�ഫ�e���W";
                worksheet.Cells[1, 3].Value = "�ഫ����W";
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
            return node?.InnerText ?? "�ϥΪ̪�";
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
