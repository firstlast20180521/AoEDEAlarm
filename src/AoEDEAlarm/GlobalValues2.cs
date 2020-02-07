using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoEDEAlarm {
     public static class GlobalValues2 {
        public static NAudioWaveHelper AudioHelper_Housing { get; set; }
        public static NAudioWaveHelper AudioHelper_NotWorking { get; set; }
        public static NAudioWaveHelper AudioHelper_WoodStock { get; set; }
        public static NAudioWaveHelper AudioHelper_FoodStock { get; set; }
        public static NAudioWaveHelper AudioHelper_GoldStock { get; set; }
        public static NAudioWaveHelper AudioHelper_StoneStock { get; set; }

        static GlobalValues2() {
            //音源ファイルと音量を指定する。
            AudioHelper_Housing = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.SoundSetting.Housing.FileName), GlobalValues.SoundSetting.Housing.Volume);
            AudioHelper_NotWorking = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.SoundSetting.NotWorking.FileName), GlobalValues.SoundSetting.NotWorking.Volume);
            AudioHelper_WoodStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.SoundSetting.WoodStock.FileName), GlobalValues.SoundSetting.WoodStock.Volume);
            AudioHelper_FoodStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.SoundSetting.FoodStock.FileName), GlobalValues.SoundSetting.FoodStock.Volume);
            AudioHelper_GoldStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.SoundSetting.GoldStock.FileName), GlobalValues.SoundSetting.GoldStock.Volume);
            AudioHelper_StoneStock = new NAudioWaveHelper(Path.Combine(ConstValues.SoundPath, GlobalValues.SoundSetting.StoneStock.FileName), GlobalValues.SoundSetting.StoneStock.Volume);

        }
    }


}
