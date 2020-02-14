using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public partial class TransparentForm : Form {

        public TransparentForm() {
            InitializeComponent();
        }

        private void TransparentForm_Load(object sender, EventArgs e) {
            //画面移動用
            //this.MouseDown += Label1_MouseDown;
            //this.MouseMove += Label1_MouseMove;
            //label1.MouseDown += new MouseEventHandler(Label1_MouseDown);
            //label1.MouseMove += new MouseEventHandler(Label1_MouseMove);
            this.Location = Properties.Settings.Default.FormPosition;

        }

        private void Label1_MouseDown(object sender, MouseEventArgs e) {
        }

        private void Label1_MouseMove(object sender, MouseEventArgs e) {
        }

        private void TransparentForm_DoubleClick(object sender, EventArgs e) {

        }

        private void TransparentForm_FormClosing(object sender, FormClosingEventArgs e) {

        }

        private void TransparentForm_Activated(object sender, EventArgs e) {

        }

        private void TransparentForm_Deactivate(object sender, EventArgs e) {

        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            MessageBox.Show("aaaaaad");
        }

        private void label1_Click(object sender, EventArgs e) {

        }
    }
}
