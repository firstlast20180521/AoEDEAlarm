using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public class AoEDEAlarmSettings {
        public int Hotkey_Run;
        public int Hotkey_Stop;
        public int Hotkey_Customise;

        public Rectangle Wood { get; set; }
        public Rectangle Food { get; set; }
        public Rectangle Gold { get; set; }
        public Rectangle Stone { get; set; }
        public Rectangle Housing { get; set; }
        public Rectangle NotWorking { get; set; }

        public class Rectangle {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public static void SaveXml(AoEDEAlarmSettings s) {
            XmlSerializer serializer = new XmlSerializer(typeof(AoEDEAlarmSettings));
            using (StreamWriter sw = new StreamWriter(AoedeStaticGlobal.SettingFileName, false, Encoding.UTF8)) {
                serializer.Serialize(sw, s);
            }

        }

        public static AoEDEAlarmSettings LoadXml() {
            XmlSerializer serializer = new XmlSerializer(typeof(AoEDEAlarmSettings));
            AoEDEAlarmSettings i;
            try {
                using (Stream reader = new FileStream(AoedeStaticGlobal.SettingFileName, FileMode.Open)) {
                    i = (AoEDEAlarmSettings)serializer.Deserialize(reader);
                }
                return i;

            } catch (Exception ex) {
                //if (ex is System.IO.FileNotFoundException) {
                //}
                AoEDEAlarmSettings ttt = new AoEDEAlarmSettings {
                    Hotkey_Run = (int)(Keys.R | Keys.Control | Keys.Shift),
                    Hotkey_Stop = (int)(Keys.S | Keys.Control | Keys.Shift),
                    Hotkey_Customise = (int)(Keys.C | Keys.Control | Keys.Shift),
                    Food = new AoEDEAlarmSettings.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                    Wood = new AoEDEAlarmSettings.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                    Gold = new AoEDEAlarmSettings.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                    Stone = new AoEDEAlarmSettings.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                    Housing = new AoEDEAlarmSettings.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                    NotWorking = new AoEDEAlarmSettings.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                };
                return ttt;
            }
        }
    }
}
