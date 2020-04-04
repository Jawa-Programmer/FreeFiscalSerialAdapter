namespace FreeFiscal
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.PortListBox = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.TextOut = new System.Windows.Forms.RichTextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.ComandToSend = new System.Windows.Forms.TextBox();
            this.Send = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PortListBox
            // 
            this.PortListBox.FormattingEnabled = true;
            this.PortListBox.Location = new System.Drawing.Point(9, 55);
            this.PortListBox.Name = "PortListBox";
            this.PortListBox.Size = new System.Drawing.Size(164, 95);
            this.PortListBox.Sorted = true;
            this.PortListBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(179, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Подключится";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(179, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Обновить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(261, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "Выбирите порт, к которому подключен Ардуино с \r\nпредустановленной программой Free" +
    "Fiscal";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.PortListBox);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 163);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подключение";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(179, 127);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Отключится";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // TextOut
            // 
            this.TextOut.Location = new System.Drawing.Point(295, 25);
            this.TextOut.Name = "TextOut";
            this.TextOut.ReadOnly = true;
            this.TextOut.Size = new System.Drawing.Size(492, 459);
            this.TextOut.TabIndex = 5;
            this.TextOut.Text = "";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 181);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(277, 23);
            this.button5.TabIndex = 6;
            this.button5.Text = "Об ФН";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // ComandToSend
            // 
            this.ComandToSend.Location = new System.Drawing.Point(296, 494);
            this.ComandToSend.Name = "ComandToSend";
            this.ComandToSend.Size = new System.Drawing.Size(407, 20);
            this.ComandToSend.TabIndex = 7;
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(709, 491);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(78, 23);
            this.Send.TabIndex = 8;
            this.Send.Text = "отправить";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(258, 52);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ввести команду можно вручную. Напишите саму \r\nкоманду и аргументы в виде байтов о" +
    "тделеных \r\nпробелами в шестнадцатиричном формате.\r\nНапример 31 - получить номер " +
    "ФН";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 526);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.ComandToSend);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.TextOut);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox PortListBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox TextOut;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox ComandToSend;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.Label label2;
    }
}

