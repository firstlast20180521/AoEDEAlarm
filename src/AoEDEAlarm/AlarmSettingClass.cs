using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AoEDEAlarm {
    public class AlarmSettingClass {
        public AlarmInf Wood { get; set; }
        public AlarmInf Food { get; set; }
        public AlarmInf Gold { get; set; }
        public AlarmInf Stone { get; set; }
        public AlarmInf Housing { get; set; }
        public AlarmInf NotWorking { get; set; }
        public AlarmInf Players { get; set; }
        public AlarmInf MiniMap { get; set; }

        public class AlarmInf {
            public string SoundFileName { get; set; }
            public float SoundVolume { get; set; }
            public int AlarmValue { get; set; }
            public bool Enabled { get; set; }
        }

        public AlarmSettingClass() {
            Wood = new AlarmInf() { SoundFileName = @"tada.wav", SoundVolume = 1.0f, AlarmValue = 500, Enabled=true };
            Food = new AlarmInf() { SoundFileName = @"tada.wav", SoundVolume = 1.0f, AlarmValue = 1000, Enabled = true };
            Gold = new AlarmInf() { SoundFileName = @"tada.wav", SoundVolume = 1.0f, AlarmValue = 1000, Enabled = true };
            Stone = new AlarmInf() { SoundFileName = @"tada.wav", SoundVolume = 1.0f, AlarmValue = 1000, Enabled = true };
            Housing = new AlarmInf() { SoundFileName = @"Windows Balloon.wav", SoundVolume = 1.0f, AlarmValue = 1, Enabled = true }; //分母ー分子＜＝N
            NotWorking = new AlarmInf() { SoundFileName = @"notify.wav", SoundVolume = 1.0f, AlarmValue = 1, Enabled = true };
            Players = new AlarmInf() { SoundFileName = @"notify.wav", SoundVolume = 1.0f, AlarmValue = 1, Enabled = true };
            MiniMap = new AlarmInf() { SoundFileName = @"notify.wav", SoundVolume = 1.0f, AlarmValue = 1, Enabled = true };

        }
    }
}
