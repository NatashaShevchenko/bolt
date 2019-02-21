namespace ScrewNutUI
{
    partial class MainForm
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
            this.KompasGroupBox = new System.Windows.Forms.GroupBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.LaunchButton = new System.Windows.Forms.Button();
            this.BoltGroupBox = new System.Windows.Forms.GroupBox();
            this.ScrewdriverHoleTypeComboBox = new System.Windows.Forms.ComboBox();
            this.lbl6 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.ChamferRadiusNumeric = new System.Windows.Forms.NumericUpDown();
            this.HatDiameterNumeric = new System.Windows.Forms.NumericUpDown();
            this.lbl5 = new System.Windows.Forms.Label();
            this.ThreadLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.KernelLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.HatLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.BuildButton = new System.Windows.Forms.Button();
            this.KompasGroupBox.SuspendLayout();
            this.BoltGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChamferRadiusNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HatDiameterNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KernelLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HatLengthNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // KompasGroupBox
            // 
            this.KompasGroupBox.Controls.Add(this.CloseButton);
            this.KompasGroupBox.Controls.Add(this.LaunchButton);
            this.KompasGroupBox.Location = new System.Drawing.Point(16, 15);
            this.KompasGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.KompasGroupBox.Name = "KompasGroupBox";
            this.KompasGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.KompasGroupBox.Size = new System.Drawing.Size(445, 75);
            this.KompasGroupBox.TabIndex = 0;
            this.KompasGroupBox.TabStop = false;
            this.KompasGroupBox.Text = "Компас 3D";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(276, 23);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(4);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(153, 31);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Закрыть";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LaunchButton
            // 
            this.LaunchButton.Location = new System.Drawing.Point(8, 23);
            this.LaunchButton.Margin = new System.Windows.Forms.Padding(4);
            this.LaunchButton.Name = "LaunchButton";
            this.LaunchButton.Size = new System.Drawing.Size(153, 31);
            this.LaunchButton.TabIndex = 0;
            this.LaunchButton.Text = "Запуск";
            this.LaunchButton.UseVisualStyleBackColor = true;
            this.LaunchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // BoltGroupBox
            // 
            this.BoltGroupBox.Controls.Add(this.ScrewdriverHoleTypeComboBox);
            this.BoltGroupBox.Controls.Add(this.lbl6);
            this.BoltGroupBox.Controls.Add(this.lbl2);
            this.BoltGroupBox.Controls.Add(this.ChamferRadiusNumeric);
            this.BoltGroupBox.Controls.Add(this.HatDiameterNumeric);
            this.BoltGroupBox.Controls.Add(this.lbl5);
            this.BoltGroupBox.Controls.Add(this.ThreadLengthNumeric);
            this.BoltGroupBox.Controls.Add(this.KernelLengthNumeric);
            this.BoltGroupBox.Controls.Add(this.HatLengthNumeric);
            this.BoltGroupBox.Controls.Add(this.lbl4);
            this.BoltGroupBox.Controls.Add(this.lbl3);
            this.BoltGroupBox.Controls.Add(this.lbl1);
            this.BoltGroupBox.Location = new System.Drawing.Point(16, 97);
            this.BoltGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.BoltGroupBox.Name = "BoltGroupBox";
            this.BoltGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.BoltGroupBox.Size = new System.Drawing.Size(445, 216);
            this.BoltGroupBox.TabIndex = 1;
            this.BoltGroupBox.TabStop = false;
            this.BoltGroupBox.Text = "Параметры болта";
            // 
            // ScrewdriverHoleTypeComboBox
            // 
            this.ScrewdriverHoleTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScrewdriverHoleTypeComboBox.FormattingEnabled = true;
            this.ScrewdriverHoleTypeComboBox.Location = new System.Drawing.Point(292, 176);
            this.ScrewdriverHoleTypeComboBox.Name = "ScrewdriverHoleTypeComboBox";
            this.ScrewdriverHoleTypeComboBox.Size = new System.Drawing.Size(137, 24);
            this.ScrewdriverHoleTypeComboBox.TabIndex = 17;
            // 
            // lbl6
            // 
            this.lbl6.AutoSize = true;
            this.lbl6.Location = new System.Drawing.Point(8, 176);
            this.lbl6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl6.Name = "lbl6";
            this.lbl6.Size = new System.Drawing.Size(170, 17);
            this.lbl6.TabIndex = 16;
            this.lbl6.Text = "Отверстие под отвертку";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(8, 26);
            this.lbl2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(121, 17);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "Диаметр шляпки";
            // 
            // ChamferRadiusNumeric
            // 
            this.ChamferRadiusNumeric.Location = new System.Drawing.Point(292, 146);
            this.ChamferRadiusNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.ChamferRadiusNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ChamferRadiusNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ChamferRadiusNumeric.Name = "ChamferRadiusNumeric";
            this.ChamferRadiusNumeric.Size = new System.Drawing.Size(137, 22);
            this.ChamferRadiusNumeric.TabIndex = 15;
            this.ChamferRadiusNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // HatDiameterNumeric
            // 
            this.HatDiameterNumeric.Location = new System.Drawing.Point(292, 23);
            this.HatDiameterNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.HatDiameterNumeric.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.HatDiameterNumeric.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.HatDiameterNumeric.Name = "HatDiameterNumeric";
            this.HatDiameterNumeric.Size = new System.Drawing.Size(137, 22);
            this.HatDiameterNumeric.TabIndex = 11;
            this.HatDiameterNumeric.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            // 
            // lbl5
            // 
            this.lbl5.AutoSize = true;
            this.lbl5.Location = new System.Drawing.Point(8, 149);
            this.lbl5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(133, 17);
            this.lbl5.TabIndex = 14;
            this.lbl5.Text = "Радиус скругления";
            // 
            // ThreadLengthNumeric
            // 
            this.ThreadLengthNumeric.Location = new System.Drawing.Point(292, 114);
            this.ThreadLengthNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.ThreadLengthNumeric.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ThreadLengthNumeric.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ThreadLengthNumeric.Name = "ThreadLengthNumeric";
            this.ThreadLengthNumeric.Size = new System.Drawing.Size(137, 22);
            this.ThreadLengthNumeric.TabIndex = 13;
            this.ThreadLengthNumeric.Value = new decimal(new int[] {
            375,
            0,
            0,
            0});
            // 
            // KernelLengthNumeric
            // 
            this.KernelLengthNumeric.Location = new System.Drawing.Point(292, 82);
            this.KernelLengthNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.KernelLengthNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.KernelLengthNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.KernelLengthNumeric.Name = "KernelLengthNumeric";
            this.KernelLengthNumeric.Size = new System.Drawing.Size(137, 22);
            this.KernelLengthNumeric.TabIndex = 12;
            this.KernelLengthNumeric.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // HatLengthNumeric
            // 
            this.HatLengthNumeric.Location = new System.Drawing.Point(292, 52);
            this.HatLengthNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.HatLengthNumeric.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.HatLengthNumeric.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.HatLengthNumeric.Name = "HatLengthNumeric";
            this.HatLengthNumeric.Size = new System.Drawing.Size(137, 22);
            this.HatLengthNumeric.TabIndex = 10;
            this.HatLengthNumeric.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // lbl4
            // 
            this.lbl4.AutoSize = true;
            this.lbl4.Location = new System.Drawing.Point(8, 117);
            this.lbl4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(268, 17);
            this.lbl4.TabIndex = 9;
            this.lbl4.Text = "Длина резьбовой части стержня болта";
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Location = new System.Drawing.Point(8, 85);
            this.lbl3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(153, 17);
            this.lbl3.TabIndex = 8;
            this.lbl3.Text = "Длина стержня болта";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(8, 55);
            this.lbl1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(111, 17);
            this.lbl1.TabIndex = 3;
            this.lbl1.Text = "Высота шляпки";
            // 
            // BuildButton
            // 
            this.BuildButton.Location = new System.Drawing.Point(308, 321);
            this.BuildButton.Margin = new System.Windows.Forms.Padding(4);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(153, 31);
            this.BuildButton.TabIndex = 2;
            this.BuildButton.Text = "Построить";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 358);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.BoltGroupBox);
            this.Controls.Add(this.KompasGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Болт с гайкой";
            this.KompasGroupBox.ResumeLayout(false);
            this.BoltGroupBox.ResumeLayout(false);
            this.BoltGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChamferRadiusNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HatDiameterNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KernelLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HatLengthNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox KompasGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button LaunchButton;
        private System.Windows.Forms.GroupBox BoltGroupBox;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.NumericUpDown ThreadLengthNumeric;
        private System.Windows.Forms.NumericUpDown KernelLengthNumeric;
        private System.Windows.Forms.NumericUpDown HatDiameterNumeric;
        private System.Windows.Forms.NumericUpDown HatLengthNumeric;
        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.NumericUpDown ChamferRadiusNumeric;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.ComboBox ScrewdriverHoleTypeComboBox;
        private System.Windows.Forms.Label lbl6;
    }
}

