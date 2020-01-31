namespace AoEDEAlarm {
    partial class PositionSettingForm {
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
            this.pctCanvas = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pctPopulation = new System.Windows.Forms.PictureBox();
            this.pctStone = new System.Windows.Forms.PictureBox();
            this.pctGold = new System.Windows.Forms.PictureBox();
            this.pctFood = new System.Windows.Forms.PictureBox();
            this.rdoWood = new System.Windows.Forms.RadioButton();
            this.rdoFood = new System.Windows.Forms.RadioButton();
            this.rdoGold = new System.Windows.Forms.RadioButton();
            this.rdoStone = new System.Windows.Forms.RadioButton();
            this.rdoPopulation = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pctWood = new System.Windows.Forms.PictureBox();
            this.rdoIdlePopulation = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.pctIdlePopulation = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pctCanvas)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPopulation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctStone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctGold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctFood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctWood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctIdlePopulation)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pctCanvas
            // 
            this.pctCanvas.Location = new System.Drawing.Point(3, 3);
            this.pctCanvas.Name = "pctCanvas";
            this.pctCanvas.Size = new System.Drawing.Size(449, 310);
            this.pctCanvas.TabIndex = 0;
            this.pctCanvas.TabStop = false;
            this.pctCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pctCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pctCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(367, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "保存して画面を閉じる";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.84671F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.15328F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("游明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(685, 376);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.87756F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.25532F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.86712F));
            this.tableLayoutPanel2.Controls.Add(this.pctPopulation, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.pctStone, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.pctGold, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.pctFood, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.rdoWood, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rdoFood, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.rdoGold, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.rdoStone, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.rdoPopulation, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label5, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.pctWood, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.rdoIdlePopulation, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label6, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.pctIdlePopulation, 2, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(218, 317);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // pctPopulation
            // 
            this.pctPopulation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctPopulation.Location = new System.Drawing.Point(88, 212);
            this.pctPopulation.Name = "pctPopulation";
            this.pctPopulation.Size = new System.Drawing.Size(126, 45);
            this.pctPopulation.TabIndex = 10;
            this.pctPopulation.TabStop = false;
            // 
            // pctStone
            // 
            this.pctStone.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctStone.Location = new System.Drawing.Point(88, 160);
            this.pctStone.Name = "pctStone";
            this.pctStone.Size = new System.Drawing.Size(126, 45);
            this.pctStone.TabIndex = 9;
            this.pctStone.TabStop = false;
            // 
            // pctGold
            // 
            this.pctGold.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctGold.Location = new System.Drawing.Point(88, 108);
            this.pctGold.Name = "pctGold";
            this.pctGold.Size = new System.Drawing.Size(126, 45);
            this.pctGold.TabIndex = 8;
            this.pctGold.TabStop = false;
            // 
            // pctFood
            // 
            this.pctFood.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctFood.Location = new System.Drawing.Point(88, 56);
            this.pctFood.Name = "pctFood";
            this.pctFood.Size = new System.Drawing.Size(126, 45);
            this.pctFood.TabIndex = 7;
            this.pctFood.TabStop = false;
            // 
            // rdoWood
            // 
            this.rdoWood.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoWood.AutoSize = true;
            this.rdoWood.Location = new System.Drawing.Point(9, 20);
            this.rdoWood.Name = "rdoWood";
            this.rdoWood.Size = new System.Drawing.Size(14, 13);
            this.rdoWood.TabIndex = 0;
            this.rdoWood.TabStop = true;
            this.rdoWood.UseVisualStyleBackColor = true;
            // 
            // rdoFood
            // 
            this.rdoFood.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoFood.AutoSize = true;
            this.rdoFood.Location = new System.Drawing.Point(9, 72);
            this.rdoFood.Name = "rdoFood";
            this.rdoFood.Size = new System.Drawing.Size(14, 13);
            this.rdoFood.TabIndex = 1;
            this.rdoFood.TabStop = true;
            this.rdoFood.UseVisualStyleBackColor = true;
            // 
            // rdoGold
            // 
            this.rdoGold.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoGold.AutoSize = true;
            this.rdoGold.Location = new System.Drawing.Point(9, 124);
            this.rdoGold.Name = "rdoGold";
            this.rdoGold.Size = new System.Drawing.Size(14, 13);
            this.rdoGold.TabIndex = 2;
            this.rdoGold.TabStop = true;
            this.rdoGold.UseVisualStyleBackColor = true;
            // 
            // rdoStone
            // 
            this.rdoStone.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoStone.AutoSize = true;
            this.rdoStone.Location = new System.Drawing.Point(9, 176);
            this.rdoStone.Name = "rdoStone";
            this.rdoStone.Size = new System.Drawing.Size(14, 13);
            this.rdoStone.TabIndex = 3;
            this.rdoStone.TabStop = true;
            this.rdoStone.UseVisualStyleBackColor = true;
            // 
            // rdoPopulation
            // 
            this.rdoPopulation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoPopulation.AutoSize = true;
            this.rdoPopulation.Location = new System.Drawing.Point(9, 228);
            this.rdoPopulation.Name = "rdoPopulation";
            this.rdoPopulation.Size = new System.Drawing.Size(14, 13);
            this.rdoPopulation.TabIndex = 4;
            this.rdoPopulation.TabStop = true;
            this.rdoPopulation.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "木";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "食料";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "金";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "人口";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "石";
            // 
            // pctWood
            // 
            this.pctWood.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctWood.Location = new System.Drawing.Point(88, 4);
            this.pctWood.Name = "pctWood";
            this.pctWood.Size = new System.Drawing.Size(126, 45);
            this.pctWood.TabIndex = 6;
            this.pctWood.TabStop = false;
            // 
            // rdoIdlePopulation
            // 
            this.rdoIdlePopulation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoIdlePopulation.AutoSize = true;
            this.rdoIdlePopulation.Location = new System.Drawing.Point(9, 282);
            this.rdoIdlePopulation.Name = "rdoIdlePopulation";
            this.rdoIdlePopulation.Size = new System.Drawing.Size(14, 13);
            this.rdoIdlePopulation.TabIndex = 4;
            this.rdoIdlePopulation.TabStop = true;
            this.rdoIdlePopulation.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 264);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 48);
            this.label6.TabIndex = 5;
            this.label6.Text = "遊んでいる農民の数";
            // 
            // pctIdlePopulation
            // 
            this.pctIdlePopulation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pctIdlePopulation.Location = new System.Drawing.Point(88, 266);
            this.pctIdlePopulation.Name = "pctIdlePopulation";
            this.pctIdlePopulation.Size = new System.Drawing.Size(126, 45);
            this.pctIdlePopulation.TabIndex = 10;
            this.pctIdlePopulation.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.button1, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.button2, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(227, 326);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(455, 47);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(276, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 41);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pctCanvas);
            this.panel1.Location = new System.Drawing.Point(227, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(455, 317);
            this.panel1.TabIndex = 3;
            // 
            // PositionSettingForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(685, 376);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimizeBox = false;
            this.Name = "PositionSettingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PositionSettingForm_FormClosing);
            this.Load += new System.EventHandler(this.PositionSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctCanvas)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPopulation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctStone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctGold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctFood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctWood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctIdlePopulation)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pctCanvas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rdoWood;
        private System.Windows.Forms.RadioButton rdoFood;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rdoGold;
        private System.Windows.Forms.RadioButton rdoStone;
        private System.Windows.Forms.RadioButton rdoPopulation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pctPopulation;
        private System.Windows.Forms.PictureBox pctStone;
        private System.Windows.Forms.PictureBox pctGold;
        private System.Windows.Forms.PictureBox pctFood;
        private System.Windows.Forms.PictureBox pctWood;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoIdlePopulation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pctIdlePopulation;
        private System.Windows.Forms.Button button2;
    }
}