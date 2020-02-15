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
    public partial class MiscellaneousSettingsForm : Form {
        public MiscellaneousSettingsForm() {
            InitializeComponent();
        }

        private void VolumeAdjustForm_Load(object sender, EventArgs e) {
            masterVolumeUpDown.Value = GlobalValues.ApplicationSetting.MasterVolume;
            uiScaleUpDown.Value = GlobalValues.ApplicationSetting.UiScale;

        }

        private void button1_Click(object sender, EventArgs e) {
            GlobalValues.ApplicationSetting.MasterVolume = (int)masterVolumeUpDown.Value;
            GlobalValues.ApplicationSetting.UiScale = (int)uiScaleUpDown.Value;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            GlobalValues.ApplicationSetting.UiScale = Program.GetScale();
            GlobalValues.NotWorkingImageArray = Program.GetNotWorkingImageArray(GlobalValues.ApplicationSetting.UiScale);
            uiScaleUpDown.Value = GlobalValues.ApplicationSetting.UiScale;
        }
    }
}
