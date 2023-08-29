namespace KNM_Job_Rename1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonBrowseSource = new TextBox();
            buttonBrowseTarget = new TextBox();
            textBox3 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            button1 = new Button();
            button2 = new Button();
            label8 = new Label();
            SuspendLayout();
            // 
            // buttonBrowseSource
            // 
            buttonBrowseSource.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonBrowseSource.Location = new Point(145, 192);
            buttonBrowseSource.Name = "buttonBrowseSource";
            buttonBrowseSource.Size = new Size(627, 29);
            buttonBrowseSource.TabIndex = 0;
            // 
            // buttonBrowseTarget
            // 
            buttonBrowseTarget.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonBrowseTarget.Location = new Point(145, 236);
            buttonBrowseTarget.Name = "buttonBrowseTarget";
            buttonBrowseTarget.Size = new Size(627, 29);
            buttonBrowseTarget.TabIndex = 1;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBox3.Location = new Point(145, 279);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(93, 29);
            textBox3.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(22, 195);
            label1.Name = "label1";
            label1.Size = new Size(121, 20);
            label1.TabIndex = 3;
            label1.Text = "源資料夾路徑：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(6, 239);
            label2.Name = "label2";
            label2.Size = new Size(137, 20);
            label2.TabIndex = 4;
            label2.Text = "目標資料夾路徑：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(6, 282);
            label3.Name = "label3";
            label3.Size = new Size(137, 20);
            label3.TabIndex = 5;
            label3.Text = "排程間隔（秒）：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(12, 82);
            label4.Name = "label4";
            label4.Size = new Size(177, 20);
            label4.TabIndex = 6;
            label4.Text = "1. 請選擇來源資料夾 T:\\";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(12, 110);
            label5.Name = "label5";
            label5.Size = new Size(546, 20);
            label5.TabIndex = 7;
            label5.Text = "2. 請選擇目標資料夾 C:\\Users\\Administrator\\Desktop\\XML_Export\\target";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(12, 139);
            label6.Name = "label6";
            label6.Size = new Size(128, 20);
            label6.TabIndex = 8;
            label6.Text = "3. 請輸入秒數 60";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(12, 389);
            label7.Name = "label7";
            label7.Size = new Size(677, 100);
            label7.TabIndex = 9;
            label7.Text = "註1. 此將自動讀取xml編碼，如無制定編碼，預設編碼utf-8\r\n\r\n註2.此將記錄已轉換的檔案至processed_files.xlsx，包含轉換前後的檔名\r\n\r\n註3.該程式一旦執行，不管暫停與否，不要手動開啟processed_files.xlsx，因為程式已鎖定使用";
            // 
            // button1
            // 
            button1.BackColor = Color.OrangeRed;
            button1.ForeColor = Color.White;
            button1.Location = new Point(145, 331);
            button1.Name = "button1";
            button1.Size = new Size(93, 37);
            button1.TabIndex = 10;
            button1.Text = "執行";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.DarkOliveGreen;
            button2.ForeColor = SystemColors.Window;
            button2.Location = new Point(262, 331);
            button2.Name = "button2";
            button2.Size = new Size(93, 37);
            button2.TabIndex = 11;
            button2.Text = "暫停";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft JhengHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label8.ForeColor = Color.Crimson;
            label8.Location = new Point(6, 18);
            label8.Name = "label8";
            label8.Size = new Size(491, 41);
            label8.TabIndex = 12;
            label8.Text = "KNM 四功備存 轉換PDF檔名系統";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Info;
            ClientSize = new Size(790, 508);
            Controls.Add(label8);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox3);
            Controls.Add(buttonBrowseTarget);
            Controls.Add(buttonBrowseSource);
            Name = "Form1";
            Text = "KNM Job ReName";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private TextBox buttonBrowseSource;
        private TextBox buttonBrowseTarget;
        private TextBox textBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Button button1;
        private Button button2;
        private Label label8;
    }
}