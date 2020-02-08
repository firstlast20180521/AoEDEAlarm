namespace AoEDEAlarm {
    partial class TransparentForm {
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(189, 81);
            this.textBox1.TabIndex = 2;
            this.textBox1.TabStop = false;
            this.textBox1.WordWrap = false;
            this.textBox1.DoubleClick += new System.EventHandler(this.textBox1_DoubleClick);
            // 
            // TransparentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(189, 81);
            this.ControlBox = false;
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("游明朝 Demibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.Green;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TransparentForm";
            this.Opacity = 0.5D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Yellow;
            this.Activated += new System.EventHandler(this.TransparentForm_Activated);
            this.Deactivate += new System.EventHandler(this.TransparentForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransparentForm_FormClosing);
            this.Load += new System.EventHandler(this.Msg_Load);
            this.Click += new System.EventHandler(this.TransparentForm_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Msg_Paint);
            this.DoubleClick += new System.EventHandler(this.Msg_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Msg_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Msg_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox1;
    }
}