using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoEDEAlarm {
     public static class GlobalValues2 {
        public static NAudioWaveHelper AudioHelper_WoodStock { get; set; }
        public static NAudioWaveHelper AudioHelper_FoodStock { get; set; }
        public static NAudioWaveHelper AudioHelper_GoldStock { get; set; }
        public static NAudioWaveHelper AudioHelper_StoneStock { get; set; }
        public static NAudioWaveHelper AudioHelper_Housing { get; set; }
        public static NAudioWaveHelper AudioHelper_NotWorking { get; set; }
        public static NAudioWaveHelper AudioHelper_Players { get; set; }
        public static NAudioWaveHelper AudioHelper_MiniMap { get; set; }

        static GlobalValues2() {
            //音源ファイルと音量を指定する。
            AudioHelper_WoodStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.Wood.SoundFileName), GlobalValues.AlarmSetting.Wood.SoundVolume);
            AudioHelper_FoodStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.Food.SoundFileName), GlobalValues.AlarmSetting.Food.SoundVolume);
            AudioHelper_GoldStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.Gold.SoundFileName), GlobalValues.AlarmSetting.Gold.SoundVolume);
            AudioHelper_StoneStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.Stone.SoundFileName), GlobalValues.AlarmSetting.Stone.SoundVolume);
            AudioHelper_Housing = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.Housing.SoundFileName), GlobalValues.AlarmSetting.Housing.SoundVolume);
            AudioHelper_NotWorking = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.NotWorking.SoundFileName), GlobalValues.AlarmSetting.NotWorking.SoundVolume);
            AudioHelper_Players = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.NotWorking.SoundFileName), GlobalValues.AlarmSetting.Players.SoundVolume);
            AudioHelper_MiniMap = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.AlarmSetting.MiniMap.SoundFileName), GlobalValues.AlarmSetting.MiniMap.SoundVolume);

        }
    }


}
