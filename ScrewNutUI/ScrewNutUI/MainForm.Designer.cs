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
            this.HeadDiameterNumeric = new System.Windows.Forms.NumericUpDown();
            this.HeadLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.ThreadLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.SmoothLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.HeadDiameterLabel = new System.Windows.Forms.Label();
            this.HeadLengthLabel = new System.Windows.Forms.Label();
            this.ThreadLabel = new System.Windows.Forms.Label();
            this.SmoothLengthLabel = new System.Windows.Forms.Label();
            this.NutGroupBox = new System.Windows.Forms.GroupBox();
            this.NutDiameterNumeric = new System.Windows.Forms.NumericUpDown();
            this.NutHeightNumeric = new System.Windows.Forms.NumericUpDown();
            this.NutDiameterLabel = new System.Windows.Forms.Label();
            this.NutHeightLabel = new System.Windows.Forms.Label();
            this.BuildButton = new System.Windows.Forms.Button();
            this.KompasGroupBox.SuspendLayout();
            this.BoltGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeadDiameterNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeadLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothLengthNumeric)).BeginInit();
            this.NutGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NutDiameterNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NutHeightNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // KompasGroupBox
            // 
            this.KompasGroupBox.Controls.Add(this.CloseButton);
            this.KompasGroupBox.Controls.Add(this.LaunchButton);
            this.KompasGroupBox.Location = new System.Drawing.Point(12, 12);
            this.KompasGroupBox.Name = "KompasGroupBox";
            this.KompasGroupBox.Size = new System.Drawing.Size(334, 61);
            this.KompasGroupBox.TabIndex = 0;
            this.KompasGroupBox.TabStop = false;
            this.KompasGroupBox.Text = "Компас 3D";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(207, 19);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(115, 25);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Закрыть";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LaunchButton
            // 
            this.LaunchButton.Location = new System.Drawing.Point(6, 19);
            this.LaunchButton.Name = "LaunchButton";
            this.LaunchButton.Size = new System.Drawing.Size(115, 25);
            this.LaunchButton.TabIndex = 0;
            this.LaunchButton.Text = "Запуск";
            this.LaunchButton.UseVisualStyleBackColor = true;
            this.LaunchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // BoltGroupBox
            // 
            this.BoltGroupBox.Controls.Add(this.HeadDiameterNumeric);
            this.BoltGroupBox.Controls.Add(this.HeadLengthNumeric);
            this.BoltGroupBox.Controls.Add(this.ThreadLengthNumeric);
            this.BoltGroupBox.Controls.Add(this.SmoothLengthNumeric);
            this.BoltGroupBox.Controls.Add(this.HeadDiameterLabel);
            this.BoltGroupBox.Controls.Add(this.HeadLengthLabel);
            this.BoltGroupBox.Controls.Add(this.ThreadLabel);
            this.BoltGroupBox.Controls.Add(this.SmoothLengthLabel);
            this.BoltGroupBox.Location = new System.Drawing.Point(12, 79);
            this.BoltGroupBox.Name = "BoltGroupBox";
            this.BoltGroupBox.Size = new System.Drawing.Size(334, 134);
            this.BoltGroupBox.TabIndex = 1;
            this.BoltGroupBox.TabStop = false;
            this.BoltGroupBox.Text = "Болт";
            // 
            // HeadDiameterNumeric
            // 
            this.HeadDiameterNumeric.Location = new System.Drawing.Point(244, 97);
            this.HeadDiameterNumeric.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.HeadDiameterNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.HeadDiameterNumeric.Name = "HeadDiameterNumeric";
            this.HeadDiameterNumeric.Size = new System.Drawing.Size(78, 20);
            this.HeadDiameterNumeric.TabIndex = 13;
            this.HeadDiameterNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // HeadLengthNumeric
            // 
            this.HeadLengthNumeric.Location = new System.Drawing.Point(244, 71);
            this.HeadLengthNumeric.Maximum = new decimal(new int[] {
            83,
            0,
            0,
            0});
            this.HeadLengthNumeric.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.HeadLengthNumeric.Name = "HeadLengthNumeric";
            this.HeadLengthNumeric.Size = new System.Drawing.Size(78, 20);
            this.HeadLengthNumeric.TabIndex = 12;
            this.HeadLengthNumeric.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            // 
            // ThreadLengthNumeric
            // 
            this.ThreadLengthNumeric.Location = new System.Drawing.Point(244, 45);
            this.ThreadLengthNumeric.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.ThreadLengthNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ThreadLengthNumeric.Name = "ThreadLengthNumeric";
            this.ThreadLengthNumeric.Size = new System.Drawing.Size(78, 20);
            this.ThreadLengthNumeric.TabIndex = 11;
            this.ThreadLengthNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // SmoothLengthNumeric
            // 
            this.SmoothLengthNumeric.Location = new System.Drawing.Point(244, 19);
            this.SmoothLengthNumeric.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.SmoothLengthNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SmoothLengthNumeric.Name = "SmoothLengthNumeric";
            this.SmoothLengthNumeric.Size = new System.Drawing.Size(78, 20);
            this.SmoothLengthNumeric.TabIndex = 10;
            this.SmoothLengthNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // HeadDiameterLabel
            // 
            this.HeadDiameterLabel.AutoSize = true;
            this.HeadDiameterLabel.Location = new System.Drawing.Point(6, 99);
            this.HeadDiameterLabel.Name = "HeadDiameterLabel";
            this.HeadDiameterLabel.Size = new System.Drawing.Size(207, 13);
            this.HeadDiameterLabel.TabIndex = 9;
            this.HeadDiameterLabel.Text = "Диаметр окружности головки болта (d)";
            // 
            // HeadLengthLabel
            // 
            this.HeadLengthLabel.AutoSize = true;
            this.HeadLengthLabel.Location = new System.Drawing.Point(6, 73);
            this.HeadLengthLabel.Name = "HeadLengthLabel";
            this.HeadLengthLabel.Size = new System.Drawing.Size(142, 13);
            this.HeadLengthLabel.TabIndex = 8;
            this.HeadLengthLabel.Text = "Длина головки болта (W3)";
            // 
            // ThreadLabel
            // 
            this.ThreadLabel.AutoSize = true;
            this.ThreadLabel.Location = new System.Drawing.Point(6, 47);
            this.ThreadLabel.Name = "ThreadLabel";
            this.ThreadLabel.Size = new System.Drawing.Size(232, 13);
            this.ThreadLabel.TabIndex = 7;
            this.ThreadLabel.Text = "Длина резьбовой части стержня болта (W2)";
            // 
            // SmoothLengthLabel
            // 
            this.SmoothLengthLabel.AutoSize = true;
            this.SmoothLengthLabel.Location = new System.Drawing.Point(6, 21);
            this.SmoothLengthLabel.Name = "SmoothLengthLabel";
            this.SmoothLengthLabel.Size = new System.Drawing.Size(219, 13);
            this.SmoothLengthLabel.TabIndex = 3;
            this.SmoothLengthLabel.Text = "Длина гладкой части стержня болта (W1)";
            // 
            // NutGroupBox
            // 
            this.NutGroupBox.Controls.Add(this.NutDiameterNumeric);
            this.NutGroupBox.Controls.Add(this.NutHeightNumeric);
            this.NutGroupBox.Controls.Add(this.NutDiameterLabel);
            this.NutGroupBox.Controls.Add(this.NutHeightLabel);
            this.NutGroupBox.Location = new System.Drawing.Point(12, 219);
            this.NutGroupBox.Name = "NutGroupBox";
            this.NutGroupBox.Size = new System.Drawing.Size(334, 81);
            this.NutGroupBox.TabIndex = 2;
            this.NutGroupBox.TabStop = false;
            this.NutGroupBox.Text = "Гайка";
            // 
            // NutDiameterNumeric
            // 
            this.NutDiameterNumeric.Location = new System.Drawing.Point(244, 49);
            this.NutDiameterNumeric.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.NutDiameterNumeric.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.NutDiameterNumeric.Name = "NutDiameterNumeric";
            this.NutDiameterNumeric.Size = new System.Drawing.Size(78, 20);
            this.NutDiameterNumeric.TabIndex = 14;
            this.NutDiameterNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // NutHeightNumeric
            // 
            this.NutHeightNumeric.Location = new System.Drawing.Point(244, 23);
            this.NutHeightNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.NutHeightNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NutHeightNumeric.Name = "NutHeightNumeric";
            this.NutHeightNumeric.Size = new System.Drawing.Size(78, 20);
            this.NutHeightNumeric.TabIndex = 15;
            this.NutHeightNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // NutDiameterLabel
            // 
            this.NutDiameterLabel.AutoSize = true;
            this.NutDiameterLabel.Location = new System.Drawing.Point(3, 51);
            this.NutDiameterLabel.Name = "NutDiameterLabel";
            this.NutDiameterLabel.Size = new System.Drawing.Size(102, 13);
            this.NutDiameterLabel.TabIndex = 15;
            this.NutDiameterLabel.Text = "Диаметр гайки (D)";
            // 
            // NutHeightLabel
            // 
            this.NutHeightLabel.AutoSize = true;
            this.NutHeightLabel.Location = new System.Drawing.Point(3, 25);
            this.NutHeightLabel.Name = "NutHeightLabel";
            this.NutHeightLabel.Size = new System.Drawing.Size(94, 13);
            this.NutHeightLabel.TabIndex = 14;
            this.NutHeightLabel.Text = "Высота гайки (H)";
            // 
            // BuildButton
            // 
            this.BuildButton.Location = new System.Drawing.Point(219, 306);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(115, 25);
            this.BuildButton.TabIndex = 2;
            this.BuildButton.Text = "Построить";
            this.BuildButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 338);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.NutGroupBox);
            this.Controls.Add(this.BoltGroupBox);
            this.Controls.Add(this.KompasGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Болт с гайкой";
            this.KompasGroupBox.ResumeLayout(false);
            this.BoltGroupBox.ResumeLayout(false);
            this.BoltGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeadDiameterNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeadLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothLengthNumeric)).EndInit();
            this.NutGroupBox.ResumeLayout(false);
            this.NutGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NutDiameterNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NutHeightNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox KompasGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button LaunchButton;
        private System.Windows.Forms.GroupBox BoltGroupBox;
        private System.Windows.Forms.GroupBox NutGroupBox;
        private System.Windows.Forms.Label HeadDiameterLabel;
        private System.Windows.Forms.Label HeadLengthLabel;
        private System.Windows.Forms.Label ThreadLabel;
        private System.Windows.Forms.Label SmoothLengthLabel;
        private System.Windows.Forms.NumericUpDown HeadDiameterNumeric;
        private System.Windows.Forms.NumericUpDown HeadLengthNumeric;
        private System.Windows.Forms.NumericUpDown ThreadLengthNumeric;
        private System.Windows.Forms.NumericUpDown SmoothLengthNumeric;
        private System.Windows.Forms.NumericUpDown NutDiameterNumeric;
        private System.Windows.Forms.NumericUpDown NutHeightNumeric;
        private System.Windows.Forms.Label NutDiameterLabel;
        private System.Windows.Forms.Label NutHeightLabel;
        private System.Windows.Forms.Button BuildButton;
    }
}

