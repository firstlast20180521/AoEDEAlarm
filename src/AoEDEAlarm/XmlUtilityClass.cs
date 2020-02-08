using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AoEDEAlarm {
    public class XmlUtilityClass<T> where T : new() {
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
            }
        }
    }
}
