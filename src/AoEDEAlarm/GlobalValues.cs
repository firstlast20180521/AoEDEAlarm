using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public static class GlobalValues {
        public static ApplicationSettingClass ApplicationSetting;
        public static SoundSettingClass SoundSetting;
        public static HotKey Hotkey_Run;
        public static HotKey Hotkey_Stop;
        public static HotKey Hotkey_Customise;
        public static CancellationTokenSource AlarmTokenSource = null;
      //public static TransparentForm Console;
        public static bool IsRunning;
      //public static List<AlarmMessageClass> AlarmMessageList;
        public static Mat[] NotWorkingImageArray;
        public static double Scale;
        public static TransparentMessage Console;

        static GlobalValues() {
            ApplicationSetting = XmlUtilityClass<ApplicationSettingClass>.LoadXml(ConstValues.ApplicationSettingFileName);
            SoundSetting = XmlUtilityClass<SoundSettingClass>.LoadXml(ConstValues.SoundSettingFileName);
            Hotkey_Run = new HotKey();
            Hotkey_Stop = new HotKey();
            Hotkey_Customise = new HotKey();
            Console = new TransparentMessage();
           
        }


        //public static void AddMessage(string message) {
        //    //末尾にメッセージを追加する。
        //    AlarmMessageList.Add(new AlarmMessageClass() { TimeStump = DateTime.Now, Message = message });
        //}


    }
}
