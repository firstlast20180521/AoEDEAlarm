using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AoEDEAlarm {
    public class TempClass<T> where T : new() {
        public static void SaveXml(T s, string settingFileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter sw = new StreamWriter(settingFileName, false, Encoding.UTF8)) {
                serializer.Serialize(sw, s);
            }

        }

        public static T LoadXml(string settingFileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T setting;
            try {
                using (Stream reader = new FileStream(settingFileName, FileMode.Open)) {
                    setting = (T)serializer.Deserialize(reader);
                }
                return setting;

            } catch (Exception) {
                return new T();
                //                if (ex is System.IO.FileNotFoundException) {
                //                }
                //setting = new ApplicationSetting() {
                //    Hotkey_Run = (int)(Keys.R | Keys.Control | Keys.Shift),
                //    Hotkey_Stop = (int)(Keys.S | Keys.Control | Keys.Shift),
                //    Hotkey_Customise = (int)(Keys.C | Keys.Control | Keys.Shift),
                //    Wood = new ApplicationSetting.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                //    Food = new ApplicationSetting.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                //    Gold = new ApplicationSetting.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                //    Stone = new ApplicationSetting.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                //    Housing = new ApplicationSetting.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                //    NotWorking = new ApplicationSetting.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, },
                //};
                //return setting;
            }
        }

    }
}
