namespace CheckDataTools
{
    partial class Form2
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
            this.m_BtnSelectExcel = new System.Windows.Forms.Button();
            this.m_BtnCheckData = new System.Windows.Forms.Button();
            this.m_BtnCheckRes = new System.Windows.Forms.Button();
            this.m_TextExcelDir = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.m_TextResDir = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_BtnSelectExcel
            // 
            this.m_BtnSelectExcel.Location = new System.Drawing.Point(36, 12);
            this.m_BtnSelectExcel.Name = "m_BtnSelectExcel";
            this.m_BtnSelectExcel.Size = new System.Drawing.Size(127, 53);
            this.m_BtnSelectExcel.TabIndex = 0;
            this.m_BtnSelectExcel.Text = "Excel文件夹";
            this.m_BtnSelectExcel.UseVisualStyleBackColor = true;
            this.m_BtnSelectExcel.Click += new System.EventHandler(this.m_BtnSelectExcel_Click);
            // 
            // m_BtnCheckData
            // 
            this.m_BtnCheckData.Location = new System.Drawing.Point(36, 143);
            this.m_BtnCheckData.Name = "m_BtnCheckData";
            this.m_BtnCheckData.Size = new System.Drawing.Size(127, 53);
            this.m_BtnCheckData.TabIndex = 1;
            this.m_BtnCheckData.Text = "表格检查";
            this.m_BtnCheckData.UseVisualStyleBackColor = true;
            this.m_BtnCheckData.Click += new System.EventHandler(this.button2_Click);
            // 
            // m_BtnCheckRes
            // 
            this.m_BtnCheckRes.Location = new System.Drawing.Point(190, 143);
            this.m_BtnCheckRes.Name = "m_BtnCheckRes";
            this.m_BtnCheckRes.Size = new System.Drawing.Size(127, 53);
            this.m_BtnCheckRes.TabIndex = 2;
            this.m_BtnCheckRes.Text = "资源检查";
            this.m_BtnCheckRes.UseVisualStyleBackColor = true;
            this.m_BtnCheckRes.Click += new System.EventHandler(this.button3_Click);
            // 
            // m_TextExcelDir
            // 
            this.m_TextExcelDir.Location = new System.Drawing.Point(181, 29);
            this.m_TextExcelDir.Name = "m_TextExcelDir";
            this.m_TextExcelDir.Size = new System.Drawing.Size(448, 21);
            this.m_TextExcelDir.TabIndex = 3;
            this.m_TextExcelDir.TextChanged += new System.EventHandler(this.m_TextExcelDir_TextChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(36, 220);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(652, 336);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(36, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 53);
            this.button1.TabIndex = 6;
            this.button1.Text = "Res文件夹";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_TextResDir
            // 
            this.m_TextResDir.Location = new System.Drawing.Point(181, 87);
            this.m_TextResDir.Name = "m_TextResDir";
            this.m_TextResDir.Size = new System.Drawing.Size(448, 21);
            this.m_TextResDir.TabIndex = 7;
            this.m_TextResDir.TextChanged += new System.EventHandler(this.m_TextResDir_TextChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 591);
            this.Controls.Add(this.m_TextResDir);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.m_TextExcelDir);
            this.Controls.Add(this.m_BtnCheckRes);
            this.Controls.Add(this.m_BtnCheckData);
            this.Controls.Add(this.m_BtnSelectExcel);
            this.Name = "Form2";
            this.Text = "表格检查工具";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_BtnSelectExcel;
        private System.Windows.Forms.Button m_BtnCheckData;
        private System.Windows.Forms.Button m_BtnCheckRes;
        private System.Windows.Forms.TextBox m_TextExcelDir;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox m_TextResDir;
    }
}