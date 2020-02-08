using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace AoEDEAlarm {
    public static class ConstValues {
        public static string WorkDirectoryPath { get; set; }
        public static string TessdataPath { get; set; }
        public static string SoundPath { get; set; }
        public static string DocumentPath { get; set; }
        public static string ImagePath { get; set; }

        public static string ApplicationSettingFileName { get; set; }
        public static string SoundSettingFileName { get; set; }
        public static string HelpFileName { get; set; }
        public static string DebugBitmapFileName1 { get; set; }
        public static string DebugBitmapFileName2 { get; set; }

        static ConstValues() {
            Assembly assm = Assembly.GetExecutingAssembly();
            string executing_directory_name = Path.GetDirectoryName(assm.Location);

            WorkDirectoryPath = Path.Combine(executing_directory_name, "work");
            if (!Directory.Exists(WorkDirectoryPath)) {
                Directory.CreateDirectory(WorkDirectoryPath);
            }

            TessdataPath = Path.Combine(executing_directory_name, "tessdata");
            SoundPath = Path.Combine(executing_directory_name, "sound");
            DocumentPath = Path.Combine(executing_directory_name, "doc");
            ImagePath = Path.Combine(executing_directory_name, "image");

            ApplicationSettingFileName = Path.Combine(executing_directory_name, @"application_setting.xml");
            SoundSettingFileName = Path.Combine(executing_directory_name, @"sound_setting.xml");
            HelpFileName = Path.Combine(DocumentPath, @"Help.htm");
            DebugBitmapFileName1 = Path.Combine(WorkDirectoryPath, @"xxx1.bmp");
            DebugBitmapFileName2 = Path.Combine(WorkDirectoryPath, @"xxx2.bmp");

        }
    }
}
