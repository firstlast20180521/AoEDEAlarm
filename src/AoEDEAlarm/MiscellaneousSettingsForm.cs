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
            updownMasterVolume.Value = GlobalValues.ApplicationSetting.MasterVolume;
            updownMonitorInterval.Value = GlobalValues.ApplicationSetting.CheckInterval;
            updownUiScale.Value = GlobalValues.ApplicationSetting.UiScale;

            chkWood.Checked = GlobalValues.AlarmSetting.Wood.Enabled;
            chkFood.Checked = GlobalValues.AlarmSetting.Food.Enabled;
            chkGold.Checked = GlobalValues.AlarmSetting.Gold.Enabled;
            chkStone.Checked = GlobalValues.AlarmSetting.Stone.Enabled;
            chkHousing.Checked = GlobalValues.AlarmSetting.Housing.Enabled;
            chkNotWorking.Checked = GlobalValues.AlarmSetting.NotWorking.Enabled;

            updownWood.Value = GlobalValues.AlarmSetting.Wood.AlarmValue;
            updownFood.Value = GlobalValues.AlarmSetting.Food.AlarmValue;
            updownGold.Value = GlobalValues.AlarmSetting.Gold.AlarmValue;
            updownStone.Value = GlobalValues.AlarmSetting.Stone.AlarmValue;
            updownHousing.Value = GlobalValues.AlarmSetting.Housing.AlarmValue;
            updownNotWorking.Value = GlobalValues.AlarmSetting.NotWorking.AlarmValue;

        }

        private void button1_Click(object sender, EventArgs e) {
            Save();
            //MessageBox.Show(text: "保存しました。"
            //    , caption: "各種設定"
            //    , buttons: MessageBoxButtons.OK
            //    , icon: MessageBoxIcon.Information
            //    , defaultButton: MessageBoxDefaultButton.Button1
            //    , options: MessageBoxOptions.DefaultDesktopOnly
            //    );

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            GlobalValues.ApplicationSetting.UiScale = Program.GetScale();
            GlobalValues.NotWorkingImageArray = Program.GetNotWorkingImageArray(GlobalValues.ApplicationSetting.UiScale);
            updownUiScale.Value = GlobalValues.ApplicationSetting.UiScale;
        }

        private void Save() {
            GlobalValues.ApplicationSetting.MasterVolume = (int)updownMasterVolume.Value;
            GlobalValues.ApplicationSetting.CheckInterval = (int)updownMonitorInterval.Value;
            GlobalValues.ApplicationSetting.UiScale = (int)updownUiScale.Value;

            GlobalValues.AlarmSetting.Wood.Enabled = chkWood.Checked;
            GlobalValues.AlarmSetting.Food.Enabled = chkFood.Checked;
            GlobalValues.AlarmSetting.Gold.Enabled = chkGold.Checked;
            GlobalValues.AlarmSetting.Stone.Enabled = chkStone.Checked;
            GlobalValues.AlarmSetting.Housing.Enabled = chkHousing.Checked;
            GlobalValues.AlarmSetting.NotWorking.Enabled = chkNotWorking.Checked;

            GlobalValues.AlarmSetting.Wood.AlarmValue = (int)updownWood.Value;
            GlobalValues.AlarmSetting.Food.AlarmValue = (int)updownFood.Value;
            GlobalValues.AlarmSetting.Gold.AlarmValue = (int)updownGold.Value;
            GlobalValues.AlarmSetting.Stone.AlarmValue = (int)updownStone.Value;
            GlobalValues.AlarmSetting.Housing.AlarmValue = (int)updownHousing.Value;
            GlobalValues.AlarmSetting.NotWorking.AlarmValue = (int)updownNotWorking.Value;

        }

        private void button4_Click(object sender, EventArgs e) {
            Save();
            MessageBox.Show("保存しました。", "各種設定", buttons: MessageBoxButtons.OK);
            //MessageBox.Show(text: "保存しました。"
            //    , caption: "各種設定"
            //    , buttons: MessageBoxButtons.OK
            //    , icon: MessageBoxIcon.Information
            //    , defaultButton: MessageBoxDefaultButton.Button1
            //    , options: MessageBoxOptions.DefaultDesktopOnly
            //    );

        }
    }
}
