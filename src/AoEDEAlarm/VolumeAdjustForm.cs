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
    public partial class VolumeAdjustForm : Form {
        public VolumeAdjustForm() {
            InitializeComponent();
        }

        private void VolumeAdjustForm_Load(object sender, EventArgs e) {
            numericUpDown1.Value = GlobalValues.ApplicationSetting.MasterVolume;

        }

        private void button1_Click(object sender, EventArgs e) {
            GlobalValues.ApplicationSetting.MasterVolume = (int)numericUpDown1.Value;
            this.Close();
        }
    }
}
