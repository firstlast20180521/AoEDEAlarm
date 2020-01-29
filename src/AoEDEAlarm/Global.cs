using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoEDEAlarm {
    public class AoedeGlobal {
        public HotKey Hotkey_Run;
        public HotKey Hotkey_Stop;
        public HotKey Hotkey_Customise;

        public static AoEDEAlarmSettings Settings;

        public static CancellationTokenSource tokenSource = null;
        public static bool IsRunning;

        public static string WorkDirectoryPath { get; set; }
        public static string TessdataPath { get; set; }
        public static string SoundPath { get; set; }
        public static string DocumentPath { get; set; }

        public static string SettingFileName { get; set; }
        public static string SoundFileName1 { get; set; }
        public static string HelpFileName { get; set; }
        public static string DebugBitmapFileName { get; set; }

        public AoedeGlobal() {
            Hotkey_Run = new HotKey();

            Assembly assm = Assembly.GetExecutingAssembly();
            IsRunning = false;

            string executing_directory_name = Path.GetDirectoryName(assm.Location);

            WorkDirectoryPath = Path.Combine(executing_directory_name, "work");
            if (!Directory.Exists(WorkDirectoryPath)) {
                Directory.CreateDirectory(WorkDirectoryPath);
            }

            TessdataPath = Path.Combine(executing_directory_name, "tessdata");
            SoundPath = Path.Combine(executing_directory_name, "sound");
            DocumentPath = Path.Combine(executing_directory_name, "doc");

            SettingFileName = Path.Combine(executing_directory_name, @"setting.xml");
            SoundFileName1 = Path.Combine(SoundPath, @"Windows Balloon.wav");
            HelpFileName = Path.Combine(DocumentPath, @"Help.htm");
            DebugBitmapFileName = Path.Combine(WorkDirectoryPath, @"xxx.bmp");

        }


    }
}
