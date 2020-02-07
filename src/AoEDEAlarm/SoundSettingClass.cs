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
            //Housing = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"Windows Balloon.wav"), Volume = 0.2f, AlarmValue = 1 }; //分母ー分子＜＝N
            //NotWorking = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"notify.wav"), Volume = 0.2f, AlarmValue = 1 };
            //WoodStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"tada.wav"), Volume = 0.2f, AlarmValue = 500 };
            //FoodStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"tada.wav"), Volume = 0.2f, AlarmValue = 1000 };
            //GoldStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"tada.wav"), Volume = 0.2f, AlarmValue = 1000 };
            //StoneStock = new SoundInf() { FileName = Path.Combine(ConstValues.SoundPath, @"tada.wav"), Volume = 0.2f, AlarmValue = 1000 };

            Housing = new SoundInf() { FileName = @"Windows Balloon.wav", Volume = 0.2f, AlarmValue = 1 }; //分母ー分子＜＝N
            NotWorking = new SoundInf() { FileName =  @"notify.wav", Volume = 0.2f, AlarmValue = 1 };
            WoodStock = new SoundInf() { FileName =  @"tada.wav", Volume = 0.2f, AlarmValue = 500 };
            FoodStock = new SoundInf() { FileName =  @"tada.wav", Volume = 0.2f, AlarmValue = 1000 };
            GoldStock = new SoundInf() { FileName =  @"tada.wav", Volume = 0.2f, AlarmValue = 1000 };
            StoneStock = new SoundInf() { FileName =  @"tada.wav", Volume = 0.2f, AlarmValue = 1000 };

        }
    }
}
