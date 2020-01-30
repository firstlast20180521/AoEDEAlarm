using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public class AoedeGlobal {
        public HotKey Hotkey_Run;
        public HotKey Hotkey_Stop;
        public HotKey Hotkey_Customise;

        public AoedeGlobal() {
            //MessageBox.Show($"AoedeGlobal.SettingFileName--->{"---7---"}");
            Hotkey_Run = new HotKey();
            Hotkey_Stop = new HotKey();
            Hotkey_Customise = new HotKey();

        }


    }
}
