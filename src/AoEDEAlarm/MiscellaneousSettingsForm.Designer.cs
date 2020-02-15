namespace AoEDEAlarm {
    partial class MiscellaneousSettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.masterVolumeUpDown = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.uiScaleUpDown = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.masterVolumeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiScaleUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // masterVolumeUpDown
            // 
            this.masterVolumeUpDown.Location = new System.Drawing.Point(156, 27);
            this.masterVolumeUpDown.Margin = new System.Windows.Forms.Padding(7);
            this.masterVolumeUpDown.Name = "masterVolumeUpDown";
            this.masterVolumeUpDown.Size = new System.Drawing.Size(126, 38);
            this.masterVolumeUpDown.TabIndex = 0;
            this.masterVolumeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.masterVolumeUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(407, 187);
            this.button1.Margin = new System.Windows.Forms.Padding(7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 45);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(321, 187);
            this.button2.Margin = new System.Windows.Forms.Padding(7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 45);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "音量";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "ＵＩスケール";
            // 
            // uiScaleUpDown
            // 
            this.uiScaleUpDown.Location = new System.Drawing.Point(156, 90);
            this.uiScaleUpDown.Margin = new System.Windows.Forms.Padding(7);
            this.uiScaleUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.uiScaleUpDown.Name = "uiScaleUpDown";
            this.uiScaleUpDown.Size = new System.Drawing.Size(126, 38);
            this.uiScaleUpDown.TabIndex = 0;
            this.uiScaleUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.uiScaleUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(292, 90);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(124, 38);
            this.button3.TabIndex = 3;
            this.button3.Text = "リセット";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // VolumeAdjustForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(498, 248);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uiScaleUpDown);
            this.Controls.Add(this.masterVolumeUpDown);
            this.Font = new System.Drawing.Font("游明朝", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "VolumeAdjustForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VolumeAdjustForm";
            this.Load += new System.EventHandler(this.VolumeAdjustForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.masterVolumeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiScaleUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown masterVolumeUpDown;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown uiScaleUpDown;
        private System.Windows.Forms.Button button3;
    }
}