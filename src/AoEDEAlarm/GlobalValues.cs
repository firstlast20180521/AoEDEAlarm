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
        public static TransparentForm Console;
        public static bool IsRunning;
        public static List<AlarmMessageClass> AlarmMessageList;
        public static Mat[] NotWorkingImageArray;
        public static double Scale;

        static GlobalValues() {
            ApplicationSetting = XmlUtilityClass<ApplicationSettingClass>.LoadXml(ConstValues.ApplicationSettingFileName);
            SoundSetting = XmlUtilityClass<SoundSettingClass>.LoadXml(ConstValues.SoundSettingFileName);
            Hotkey_Run = new HotKey();
            Hotkey_Stop = new HotKey();
            Hotkey_Customise = new HotKey();
            Console = new TransparentForm();
            IsRunning = false;
            AlarmMessageList = new List<AlarmMessageClass>();
            Scale = GetScale();
            NotWorkingImageArray = GetNotWorkingImageArray(Scale);
        }

        private static Mat[] GetNotWorkingImageArray(double scale) {
            string[] files = System.IO.Directory.GetFiles(ConstValues.NotWorkingImagePath, "*", System.IO.SearchOption.TopDirectoryOnly);
            NotWorkingImageArray = new Mat[files.Length];
            for (int i = 0; i < files.Length; i++) {
                using (Mat mat_original = new Mat(files[i], ImreadModes.Unchanged)) {
                    using (Mat mat_rotation = Cv2.GetRotationMatrix2D(new Point2f(0, 0), 0d, scale)) {
                        //using (Mat mat_affined = new Mat()) {
                        //    Cv2.WarpAffine(mat_original, mat_affined, mat_rotation, new OpenCvSharp.Size((double)mat_original.Width * scale, (double)mat_original.Height * scale));
                        //    int idx;
                        //    bool rtn = int.TryParse(Path.GetFileNameWithoutExtension(files[i]), out idx);
                        //    if (rtn) {
                        //        NotWorkingImageArray[idx] = mat_affined;
                        //    }
                        //}

                        Mat mat_affined = new Mat();
                        Cv2.WarpAffine(mat_original, mat_affined, mat_rotation, new OpenCvSharp.Size((double)mat_original.Width * scale, (double)mat_original.Height * scale));
                        int idx;
                        bool rtn = int.TryParse(Path.GetFileNameWithoutExtension(files[i]), out idx);
                        if (rtn) {
                            NotWorkingImageArray[idx] = mat_affined;
                        }


                    }
                }
            }
            return NotWorkingImageArray;
        }

        private static double GetScale() {

            double max_value = 0;
            double max_scalse = 0;

            Rectangle rect = Screen.PrimaryScreen.Bounds;
            using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb)) {
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
                using (Mat mat_full_screen = BitmapConverter.ToMat(bmp)) {
                    using (Mat mat_stone_age = new Mat(ConstValues.RulerImageFileName, ImreadModes.Unchanged)) {

                        for (int i = 75; i <= 150; i++) {
                            
                            double scale = ((double)i / (double)100);

                            using (Mat mat_rotation = Cv2.GetRotationMatrix2D(new Point2f(0, 0), 0d, scale)) {
                                using (Mat mat_affined = new Mat()) {
                                    Cv2.WarpAffine(mat_stone_age, mat_affined, mat_rotation, new OpenCvSharp.Size((double)mat_stone_age.Width * scale, (double)mat_stone_age.Height * scale));

                                    double v = OpenCvUtil.FindImage(mat_full_screen, mat_affined, 0.7d);
                                    if (v>max_value) {
                                        max_value = v;
                                        max_scalse = scale;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return max_scalse;
        }

        public static void AddMessage(string message) {
            //末尾にメッセージを追加する。
            AlarmMessageList.Add(new AlarmMessageClass() { TimeStump = DateTime.Now, Message = message });
        }

        //public static void ClearData() {
        //    GlobalValues.BackDataList.RemoveAll(s => DateTime.Now - s.TimeStump > new TimeSpan(0, 0, 10));
        //}

    }
}
