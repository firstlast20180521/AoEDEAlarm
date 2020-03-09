using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XImgProc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;
using System.Media;
using NAudio.Wave;

namespace AoEDEAlarm {
    class AoEDEAlarm : IDisposable {

        public AoEDEAlarm() {
        }

        public async Task<bool> Run(CancellationToken token) {
            //チェックする時間間隔を設定より代入する。
            TimeSpan time_span = new TimeSpan(0, 0, 0, 0, GlobalValues.ApplicationSetting.CheckInterval);
            DateTime last_clock = DateTime.MinValue;

            while (true) {
                //複数アラーム発生時に、最初のアラーム音だけ鳴らすためのフラグ。
                bool isAlarmed = false;

                if (token.IsCancellationRequested) {
                    GlobalValues.Console.Add("監視処理を中断しました。");
                    break;
                }

                //一定時間が経過していない場合は、１ループパス
                if (DateTime.Now - last_clock < time_span) {
                    //ここで鼓動を作り出す。
                    await Task.Delay(300);
                    continue;
                }

                //画像チェックを行なった時刻を変数に格納
                last_clock = DateTime.Now;

                //フルスクリーン矩形を作成
                Rectangle rect = Screen.PrimaryScreen.Bounds;
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb)) {
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);

                    //Bitmap bmp = Win32Api.CaptureActiveWindow();

                    //一旦ファイルに上書き保存
                    //bmp.Save(@"D:\Work\画像調査\フルスクリーン画像.png");

                    using (Mat mat = BitmapConverter.ToMat(bmp)) {

                        //
                        //ミニマップ
                        //
                        string message;
                        bool rtnMiniMap = CheckMiniMap(mat, out message);
                        if (rtnMiniMap) {
                            //GlobalValues.Console.Add($"--->{message}<---");
                        }

                        //
                        //プレイヤー
                        //
                        string[] playerNames;
                        bool rtnPlayer = CheckPlayer(mat, out playerNames);
                        if (rtnPlayer) {
                            for (int i = 0; i < playerNames.Length; i++) {
                                //GlobalValues.Console.Invoke((MethodInvoker)delegate {
                                //    //GlobalValues.Console.Start($"--->{playerNames[i]}<---");
                                //    //GlobalValues.AddMessage($"--->{playerNames[i]}<---");
                                //});
                            }
                        }

                        //
                        //木
                        //
                        if (GlobalValues.AlarmSetting.Wood.Enabled) {
                            int woodValue = CheckWood(mat);
                            if (woodValue >= GlobalValues.AlarmSetting.Wood.AlarmValue) {
                                if (!isAlarmed) {
                                    GlobalValues2.AudioHelper_WoodStock.Play(GlobalValues.ApplicationSetting.MasterVolume);
                                    isAlarmed = true;
                                }
                                GlobalValues.Console.Add($"木が{woodValue}余っています。");
                            }
                        }

                        //
                        //食料
                        //
                        if (GlobalValues.AlarmSetting.Food.Enabled) {
                            int foodValue = CheckFood(mat);
                            if (foodValue >= GlobalValues.AlarmSetting.Food.AlarmValue) {
                                if (!isAlarmed) {
                                    GlobalValues2.AudioHelper_FoodStock.Play(GlobalValues.ApplicationSetting.MasterVolume);
                                    isAlarmed = true;
                                }
                                GlobalValues.Console.Add($"食料が{foodValue}余っています。");
                            }
                        }

                        //
                        //金
                        //
                        if (GlobalValues.AlarmSetting.Gold.Enabled) {
                            int goldValue = CheckGold(mat);
                            if (goldValue >= GlobalValues.AlarmSetting.Gold.AlarmValue) {
                                if (!isAlarmed) {
                                    GlobalValues2.AudioHelper_GoldStock.Play(GlobalValues.ApplicationSetting.MasterVolume);
                                    isAlarmed = true;
                                }
                                GlobalValues.Console.Add($"金が{goldValue}余っています。");
                            }
                        }

                        //
                        //石
                        //
                        if (GlobalValues.AlarmSetting.Stone.Enabled) {
                            int stoneValue = CheckStone(mat);
                            if (stoneValue >= GlobalValues.AlarmSetting.Stone.AlarmValue) {
                                if (!isAlarmed) {
                                    GlobalValues2.AudioHelper_StoneStock.Play(GlobalValues.ApplicationSetting.MasterVolume);
                                    isAlarmed = true;
                                }
                                GlobalValues.Console.Add($"石が{stoneValue}余っています。");
                            }
                        }

                        //
                        //遊休農民
                        //
                        if (GlobalValues.AlarmSetting.NotWorking.Enabled) {
                            int numberNotWorking;
                            bool rtn2 = CheckNotWorking(mat, out numberNotWorking);
                            if (rtn2) {
                                if (numberNotWorking >= GlobalValues.AlarmSetting.NotWorking.AlarmValue) {
                                    //アラーム音を出す。
                                    if (!isAlarmed) {
                                        GlobalValues2.AudioHelper_NotWorking.Play(GlobalValues.ApplicationSetting.MasterVolume);
                                        isAlarmed = true;
                                    }
                                    GlobalValues.Console.Add($"農民が{numberNotWorking}人遊んでいます。");
                                }
                            }
                        }

                        //
                        //家
                        //
                        if (GlobalValues.AlarmSetting.Housing.Enabled) {
                            int popNumerator;
                            int popDenominator;
                            bool rtn1 = CheckHousingShortage(mat, out popNumerator, out popDenominator);
                            if (rtn1) {
                                if (popDenominator - popNumerator <= GlobalValues.AlarmSetting.Housing.AlarmValue) {
                                    if (!isAlarmed) {
                                        GlobalValues2.AudioHelper_Housing.Play(GlobalValues.ApplicationSetting.MasterVolume);
                                        isAlarmed = true;
                                    }
                                    GlobalValues.Console.Add($"家が不足しています。");
                                }
                            }
                        }
                    }
                }
            }
            //↑ループ
            return true;
        }

        /// <summary>
        /// 色を抽出して、変化を見る処理
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool CheckMiniMap(Mat mat, out string message) {
            message = "";
            try {
                Point loc = new Point(GlobalValues.ApplicationSetting.MiniMap.X, GlobalValues.ApplicationSetting.MiniMap.Y);
                Size sz = new Size(GlobalValues.ApplicationSetting.MiniMap.Width, GlobalValues.ApplicationSetting.MiniMap.Height);
                //double x = GlobalValues.ApplicationSetting.Housing.X * GlobalValues.Scale;
                //double y = GlobalValues.ApplicationSetting.Housing.Y * GlobalValues.Scale;
                //double width = GlobalValues.ApplicationSetting.Housing.Width * GlobalValues.Scale;
                //double height = GlobalValues.ApplicationSetting.Housing.Height * GlobalValues.Scale;
                //Point loc = new Point(x, y);
                //Size sz = new Size(width, height);

                Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
                Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

                using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc2, sz2))) {
                    //mat2.ImWrite(@"D:\Work\画像調査\部分画像1.png",  );

                    Scalar s_min = new Scalar(0, 0, 100);
                    Scalar s_max = new Scalar(100, 100, 255);                    //The lower boundary is neither an array of the same size and same type as src, nor a scalar
                    using (Mat mask_image = mat2.InRange(s_min, s_max)) {
                        //Cv2.ImShow("mask_image", mask_image);
                        //Cv2.WaitKey(10);
                        //message = "Done";
                    }
                }

            } catch (Exception ex) {
                Console.WriteLine($"{ex.Message}");
            }
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="playerNames"></param>
        /// <returns></returns>
        private bool CheckPlayer(Mat mat, out string[] playerNames) {
            playerNames = new string[8];
            for (int i = 0; i < playerNames.Length; i++) {
                playerNames[i] = "";
            }
            try {
                Point loc = new Point(GlobalValues.ApplicationSetting.Players.X, GlobalValues.ApplicationSetting.Players.Y);
                Size sz = new Size(GlobalValues.ApplicationSetting.Players.Width, GlobalValues.ApplicationSetting.Players.Height);

                Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
                Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

                using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc2, sz2))) {
                    Cv2.ImShow("mat2", mat2);
                    Cv2.WaitKey();
                    //InputArray lower_white = InputArray.Create(new int[3] { 0, 0, 100 });
                    //InputArray upper_white = InputArray.Create(new int[3] { 180, 45, 255 });
                    //using (Mat mat_mask_white = mat2.InRange(lower_white, upper_white)) {
                    //    Cv2.ImShow("mat_mask_white", mat_mask_white);
                    //    Cv2.WaitKey(10);
                    //}
                    using (Mat hsv_image = new Mat()) {
                        Cv2.CvtColor(mat2, hsv_image, ColorConversionCodes.BGR2HSV);
                        Scalar s_min = new Scalar(20, 80, 10);
                        Scalar s_max = new Scalar(50, 255, 255);                    //The lower boundary is neither an array of the same size and same type as src, nor a scalar
                        using (Mat mask_image = hsv_image.InRange(s_min, s_max)) {
                            Cv2.ImShow("mask_image", mask_image);
                            Cv2.WaitKey();
                        }
                    }
                }

            } catch (Exception ex) {
                Console.WriteLine($"{ex.Message}");
            }
            return true;
        }

        private Mat GetColorMat(Mat img) {
            //元の画像を読込
            //Mat img = new Mat("test.jpg");

            //二値化画像を保存するMatの準備
            Mat bin_iplImg = new Mat(); //Cv.CreateImage(img.Size, BitDepth.U8, 1);

            //RGB要素のMatの準備 Cv2.CreateImage(img.Size, BitDepth.U8, 1)
            Mat r_iplImg = img.EmptyClone();
            Mat g_iplImg = img.EmptyClone();
            Mat b_iplImg = img.EmptyClone();

            //元画像をRGBに分解
            Mat[] temp = Cv2.Split(img);

            //RGB各要素の閾値
            int r_threshold = 180;
            int g_threshold = 230;
            int b_threshold = 150;

            // 各RGB要素で閾値以下のピクセルを抽出する
            Cv2.Threshold(temp[2], r_iplImg, r_threshold, 255, ThresholdTypes.BinaryInv);
            Cv2.Threshold(temp[1], g_iplImg, g_threshold, 255, ThresholdTypes.BinaryInv);
            Cv2.Threshold(temp[0], b_iplImg, b_threshold, 255, ThresholdTypes.BinaryInv);

            // ORでRGB要素を合算
            Cv2.BitwiseOr(b_iplImg, g_iplImg, bin_iplImg, null);
            Cv2.BitwiseOr(bin_iplImg, r_iplImg, bin_iplImg, null);

            return bin_iplImg;
        }

        private bool CheckHousingShortage(Mat mat, out int popNumerator, out int popDenominator) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Housing.X, GlobalValues.ApplicationSetting.Housing.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Housing.Width, GlobalValues.ApplicationSetting.Housing.Height);

            Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
            Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc2, sz2))) {
                //Cv2.ImShow("mat2", mat2);
                //Cv2.WaitKey(100);

                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));

                        //mat4.ImWrite(ConstValues.DebugBitmapFileName1);

                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890/");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            string[] arr = text.Split(new char[] { '/' });
                            if (arr?.Length == 2) {
                                bool r1 = int.TryParse(arr[0], out popNumerator);
                                bool r2 = int.TryParse(arr[1], out popDenominator);
                                if (r1 && r2) return true;
                            }
                            popNumerator = 0;
                            popDenominator = 0;
                            return false;
                        }
                    }
                }
            }
        }

        private int CheckWood(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Wood.X, GlobalValues.ApplicationSetting.Wood.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Wood.Width, GlobalValues.ApplicationSetting.Wood.Height);

            Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
            Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc2, sz2))) {

                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));

                        //mat4.ImWrite(ConstValues.DebugBitmapFileName1);
                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890/");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            int v = 0;
                            bool r = int.TryParse(text, out v);
                            return v;
                        }
                    }
                }
            }
        }

        private int CheckFood(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Food.X, GlobalValues.ApplicationSetting.Food.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Food.Width, GlobalValues.ApplicationSetting.Food.Height);

            Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
            Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc2, sz2))) {
                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));
                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            int v = 0;
                            bool r = int.TryParse(text, out v);
                            return v;
                        }
                    }
                }
            }
        }

        private int CheckGold(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Gold.X, GlobalValues.ApplicationSetting.Gold.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Gold.Width, GlobalValues.ApplicationSetting.Gold.Height);

            Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
            Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc2, sz2))) {
                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));
                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            int v = 0;
                            bool r = int.TryParse(text, out v);
                            return v;
                        }
                    }
                }
            }
        }

        private int CheckStone(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Stone.X, GlobalValues.ApplicationSetting.Stone.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Stone.Width, GlobalValues.ApplicationSetting.Stone.Height);

            Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
            Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc2, sz2))) {
                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));
                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            int v = 0;
                            bool r = int.TryParse(text, out v);
                            return v;
                        }
                    }
                }
            }
        }

        private bool CheckNotWorking(Mat mat1, out int numberNotWorking) {

            numberNotWorking = 0;

            Point loc = new Point(GlobalValues.ApplicationSetting.NotWorking.X, GlobalValues.ApplicationSetting.NotWorking.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.NotWorking.Width, GlobalValues.ApplicationSetting.NotWorking.Height);

            Point loc2 = OpenCvUtil.Enlarge(loc, GlobalValues.ApplicationSetting.UiScale);
            Size sz2 = OpenCvUtil.Enlarge(sz, GlobalValues.ApplicationSetting.UiScale);

            using (Mat mat2 = mat1.Clone(new OpenCvSharp.Rect(loc2, sz2))) {

                double max_value = 0;
                int max_idx = -1;

                for (int i = 0; i < GlobalValues.NotWorkingImageArray.Length; i++) {
                    double v = OpenCvUtil.FindImage(mat2, GlobalValues.NotWorkingImageArray[i], 0.5d);
                    if (v > max_value) {
                        max_value = v;
                        max_idx = i;
                    }
                }

                numberNotWorking = max_idx;

                if (max_idx != -1) return true;

                return false;

            }

        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~AoEDEAlarm()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose() {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion


    }

}
