using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoEDEAlarm {
    public static class AoedeStaticGlobal {
        public static AoEDEAlarmSettings Settings;

        public static CancellationTokenSource tokenSource = null;
        public static bool IsRunning;

        public static string WorkDirectoryPath { get; set; }
        public static string TessdataPath { get; set; }
        public static string SoundPath { get; set; }
        public static string DocumentPath { get; set; }

        public static string SettingFileName { get; set; }
        public static string SoundFileName_Housing { get; set; }
        public static string SoundFileName_JobNotAssigned { get; set; }
        public static string HelpFileName { get; set; }
        public static string DebugBitmapFileName1 { get; set; }
        public static string DebugBitmapFileName2 { get; set; }
        //public static string DebugBitmapFileName3 { get; set; }
        //public static string DebugBitmapFileName4 { get; set; }
        //public static string DebugBitmapFileName5 { get; set; }
        //public static string DebugBitmapFileName6 { get; set; }

        static AoedeStaticGlobal() {

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
            SoundFileName_Housing = Path.Combine(SoundPath, @"Windows Balloon.wav");
            SoundFileName_JobNotAssigned = Path.Combine(SoundPath, @"chord.wav");
            HelpFileName = Path.Combine(DocumentPath, @"Help.htm");
            DebugBitmapFileName1 = Path.Combine(WorkDirectoryPath, @"xxx1.bmp");
            DebugBitmapFileName2 = Path.Combine(WorkDirectoryPath, @"xxx2.bmp");
            //DebugBitmapFileName3 = Path.Combine(WorkDirectoryPath, @"xxx3.bmp");
            //DebugBitmapFileName4 = Path.Combine(WorkDirectoryPath, @"xxx4.bmp");
            //DebugBitmapFileName5 = Path.Combine(WorkDirectoryPath, @"xxx5.bmp");
            //DebugBitmapFileName6 = Path.Combine(WorkDirectoryPath, @"xxx6.bmp");

        }
    }
}
