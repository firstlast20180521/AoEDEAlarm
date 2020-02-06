using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoEDEAlarm {
    static class GlobalValuesClass2 {
        public static NAudioWaveHelper NAudio_Housing;
        public static NAudioWaveHelper NAudio_NotWorking;
        public static NAudioWaveHelper NAudio_WoodStock;
        public static NAudioWaveHelper NAudio_FoodStock;
        public static NAudioWaveHelper NAudio_GoldStock;
        public static NAudioWaveHelper NAudio_StoneStock;


        static GlobalValuesClass2() {
            NAudio_Housing = new NAudioWaveHelper(GlobalValues.SoundSetting.Housing.FileName, GlobalValues.SoundSetting.Housing.Volume);
            NAudio_NotWorking = new NAudioWaveHelper(GlobalValues.SoundSetting.NotWorking.FileName, GlobalValues.SoundSetting.NotWorking.Volume);
            NAudio_WoodStock = new NAudioWaveHelper(GlobalValues.SoundSetting.WoodStock.FileName, GlobalValues.SoundSetting.WoodStock.Volume);
            NAudio_FoodStock = new NAudioWaveHelper(GlobalValues.SoundSetting.FoodStock.FileName, GlobalValues.SoundSetting.FoodStock.Volume);
            NAudio_GoldStock = new NAudioWaveHelper(GlobalValues.SoundSetting.GoldStock.FileName, GlobalValues.SoundSetting.GoldStock.Volume);
            NAudio_StoneStock = new NAudioWaveHelper(GlobalValues.SoundSetting.StoneStock.FileName, GlobalValues.SoundSetting.StoneStock.Volume);

        }
    }


}
