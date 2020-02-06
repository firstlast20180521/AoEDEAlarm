using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AoEDEAlarm {
    public class SoundSettingClass {
        public SoundInf Housing { get; set; }
        public SoundInf NotWorking { get; set; }
        public SoundInf WoodStock { get; set; }
        public SoundInf FoodStock { get; set; }
        public SoundInf GoldStock { get; set; }
        public SoundInf StoneStock { get; set; }

        public class SoundInf {
            public string FileName { get; set; }
            public float Volume { get; set; }
            public int AlarmValue { get; set; }
        }

        public SoundSettingClass() {
            Housing = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"Windows Balloon.wav"), Volume = 0.2f, AlarmValue = 1 }; //分母ー分子＜＝N
            NotWorking = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"notify.wav"), Volume = 0.2f, AlarmValue = 1 };
            WoodStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.2f, AlarmValue = 500 };
            FoodStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.2f, AlarmValue = 1000 };
            GoldStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.2f, AlarmValue = 1000 };
            StoneStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.2f, AlarmValue = 1000 };

        }

        //public static void SaveXml(SoundSettingClass s) {
        //    XmlSerializer serializer = new XmlSerializer(typeof(SoundSettingClass));
        //    using (StreamWriter sw = new StreamWriter(ConstValues.ApplicationSettingFileName, false, Encoding.UTF8)) {
        //        serializer.Serialize(sw, s);
        //    }

        //}

        //public static SoundSettingClass LoadXml() {
        //    XmlSerializer serializer = new XmlSerializer(typeof(SoundSettingClass));
        //    SoundSettingClass ss;
        //    try {
        //        using (Stream reader = new FileStream(ConstValues.ApplicationSettingFileName, FileMode.Open)) {
        //            ss = (SoundSettingClass)serializer.Deserialize(reader);
        //        }
        //        return ss;

        //    } catch (Exception) {
        //        //if (ex is System.IO.FileNotFoundException) {
        //        //}
        //        ss = new SoundSettingClass {
        //            Housing = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"Windows バルーン.wav"), Volume = 0.5f },
        //            NotWorking = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"notify.wav"), Volume = 0.5f },
        //            WoodStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.5f },
        //            FoodStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.5f },
        //            GoldStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.5f },
        //            StoneStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"chimes.wav"), Volume = 0.5f },
        //        };
        //        return ss;
        //    }
        //}

    }
}
