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
    public static class GlobalValues {
        public static ApplicationSettingClass ApplicationSetting;
        public static SoundSettingClass SoundSetting;
        public static HotKey Hotkey_Run;
        public static HotKey Hotkey_Stop;
        public static HotKey Hotkey_Customise;
        public static CancellationTokenSource tokenSource = null;
        public static TransparentForm Console;
        public static bool IsRunning;

        static GlobalValues() {
            ApplicationSetting = TempClass<ApplicationSettingClass>.LoadXml(ConstValues.ApplicationSettingFileName);
            SoundSetting = TempClass<SoundSettingClass>.LoadXml(ConstValues.SoundSettingFileName);
            Hotkey_Run = new HotKey();
            Hotkey_Stop = new HotKey();
            Hotkey_Customise = new HotKey();
            Console = new TransparentForm();
            IsRunning = false;

        }


    }
}
