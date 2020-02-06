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

        //System.Media.SoundPlayer _SoundPlayer_HouseLacking = new System.Media.SoundPlayer();
        //System.Media.SoundPlayer _SoundPlayer_JobNotAssigned = new System.Media.SoundPlayer();

        public AoEDEAlarm() {
            //_SoundPlayer_HouseLacking = new System.Media.SoundPlayer(AoedeStaticGlobal.SoundFileName_Housing);
            //_SoundPlayer_JobNotAssigned = new System.Media.SoundPlayer(AoedeStaticGlobal.SoundFileName_JobNotAssigned);

        }

        public async Task<bool> Run(CancellationToken token) {

            while (true) {

                await Task.Delay(GlobalValues.ApplicationSetting.CheckInterval);
                bool isAlarmed = false;

                if (token.IsCancellationRequested) {
                    GlobalValues.Console.Invoke((MethodInvoker)delegate {
                        GlobalValues.Console.Start("監視処理を中断しました。");
                    });
                    break;
                }

                //フルスクリーン矩形を作成
                Rectangle rect = Screen.PrimaryScreen.Bounds;
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb)) {
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);

                    //Bitmap bmp = Win32Api.CaptureActiveWindow();

                    //一旦ファイルに上書き保存
                    //bmp.Save(AoedeStaticGlobal.DebugBitmapFileName1);

                    using (Mat mat = BitmapConverter.ToMat(bmp)) {
                        //
                        //木
                        //
                        int woodValue = CheckWood(mat);
                        if (woodValue >= GlobalValues.SoundSetting.WoodStock.AlarmValue) {
                            if (!isAlarmed) {
                                NAudioWaveHelper.Play_Sound_x(GlobalValues.SoundSetting.WoodStock.FileName, GlobalValues.SoundSetting.WoodStock.Volume);
                                isAlarmed = true;
                            }
                            GlobalValues.Console.Invoke((MethodInvoker)delegate {
                                GlobalValues.Console.Start($"木材が{woodValue}余っています。");
                            });
                            //continue;
                        }

                        //
                        //食料
                        //
                        int foodValue = CheckFood(mat);
                        if (foodValue >= GlobalValues.SoundSetting.FoodStock.AlarmValue) {
                            if (!isAlarmed) {
                                NAudioWaveHelper.Play_Sound_x(GlobalValues.SoundSetting.FoodStock.FileName, GlobalValues.SoundSetting.FoodStock.Volume);
                                isAlarmed = true;
                            }
                            GlobalValues.Console.Invoke((MethodInvoker)delegate {
                                GlobalValues.Console.Start($"食料が{foodValue}余っています。");
                            });
                            //continue;
                        }

                        //
                        //金
                        //
                        int goldValue = CheckGold(mat);
                        if (goldValue >= GlobalValues.SoundSetting.GoldStock.AlarmValue) {
                            if (!isAlarmed) {
                                NAudioWaveHelper.Play_Sound_x(GlobalValues.SoundSetting.GoldStock.FileName, GlobalValues.SoundSetting.GoldStock.Volume);
                                isAlarmed = true;
                            }
                            GlobalValues.Console.Invoke((MethodInvoker)delegate {
                                GlobalValues.Console.Start($"金が{goldValue}余っています。");
                            });
                            //continue;
                        }

                        //
                        //石
                        //
                        int stoneValue = CheckStone(mat);
                        if (stoneValue >= GlobalValues.SoundSetting.StoneStock.AlarmValue) {
                            if (!isAlarmed) {
                                NAudioWaveHelper.Play_Sound_x(GlobalValues.SoundSetting.StoneStock.FileName, GlobalValues.SoundSetting.StoneStock.Volume);
                                isAlarmed = true;
                            }
                            GlobalValues.Console.Invoke((MethodInvoker)delegate {
                                GlobalValues.Console.Start($"石が{stoneValue}余っています。");
                            });
                            //continue;
                        }

                        //
                        //遊び農民
                        //
                        int numberNotWorking;
                        bool rtn2 = CheckNotWorking(mat, out numberNotWorking);
                        if (rtn2) {
                            if (numberNotWorking >= GlobalValues.SoundSetting.NotWorking.AlarmValue) {
                                //アラーム音を出す。
                                if (!isAlarmed) { 
                                    NAudioWaveHelper.Play_Sound_x(GlobalValues.SoundSetting.NotWorking.FileName, GlobalValues.SoundSetting.NotWorking.Volume);
                                    isAlarmed = true;
                                }
                                GlobalValues.Console.Invoke((MethodInvoker)delegate {
                                    GlobalValues.Console.Start($"農民が{numberNotWorking}人遊んでいます。");
                                });
                                //continue;
                            }
                        }

                        //
                        //家
                        //
                        int popNumerator;
                        int popDenominator;
                        bool rtn1 = CheckHousingShortage(mat, out popNumerator, out popDenominator);
                        if (rtn1) {
                            if (popDenominator - popNumerator <= GlobalValues.SoundSetting.Housing.AlarmValue) {
                                if (!isAlarmed) { 
                                    NAudioWaveHelper.Play_Sound_x(GlobalValues.SoundSetting.Housing.FileName, GlobalValues.SoundSetting.Housing.Volume);
                                    isAlarmed = true;
                                }
                                GlobalValues.Console.Invoke((MethodInvoker)delegate {
                                    GlobalValues.Console.Start($"家が不足しています。");
                                });
                                //continue;
                            }
                        }


                    }
                }

            }

            //}
            return true;
        }

        private bool CheckHousingShortage(Mat mat, out int popNumerator, out int popDenominator) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Housing.X, GlobalValues.ApplicationSetting.Housing.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Housing.Width, GlobalValues.ApplicationSetting.Housing.Height);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {

                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));

                        mat4.ImWrite(ConstValues.DebugBitmapFileName1);

                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890/");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //最前面
                            //MessageBox.Show(text: $"->{text}<-"
                            //    , caption: "AoEDEAlarm"
                            //    , buttons: MessageBoxButtons.OK
                            //    , icon: MessageBoxIcon.Information
                            //    , defaultButton: MessageBoxDefaultButton.Button1
                            //    , options: MessageBoxOptions.DefaultDesktopOnly
                            //    );

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

                            //await Task.Delay(1000);

                        }


                    }
                }
            }

        }

        private int CheckWood(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Wood.X, GlobalValues.ApplicationSetting.Wood.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Wood.Width, GlobalValues.ApplicationSetting.Wood.Height);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {

                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));

                        mat4.ImWrite(ConstValues.DebugBitmapFileName1);

                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890/");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //最前面
                            //MessageBox.Show(text: $"->{text}<-"
                            //    , caption: "AoEDEAlarm"
                            //    , buttons: MessageBoxButtons.OK
                            //    , icon: MessageBoxIcon.Information
                            //    , defaultButton: MessageBoxDefaultButton.Button1
                            //    , options: MessageBoxOptions.DefaultDesktopOnly
                            //    );

                            //数値に分解
                            int x = 0;
                            bool r = int.TryParse(text, out x);
                            return x;

                            //await Task.Delay(1000);

                        }


                    }
                }
            }

        }

        private int CheckFood(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Food.X, GlobalValues.ApplicationSetting.Food.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Food.Width, GlobalValues.ApplicationSetting.Food.Height);
            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {
                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));
                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            int x = 0;
                            bool r = int.TryParse(text, out x);
                            return x;
                        }
                    }
                }
            }
        }

        private int CheckGold(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Gold.X, GlobalValues.ApplicationSetting.Gold.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Gold.Width, GlobalValues.ApplicationSetting.Gold.Height);
            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {
                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));
                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            int x = 0;
                            bool r = int.TryParse(text, out x);
                            return x;
                        }
                    }
                }
            }
        }

        private int CheckStone(Mat mat) {
            Point loc = new Point(GlobalValues.ApplicationSetting.Stone.X, GlobalValues.ApplicationSetting.Stone.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.Stone.Width, GlobalValues.ApplicationSetting.Stone.Height);
            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {
                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));
                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //数値に分解
                            int x = 0;
                            bool r = int.TryParse(text, out x);
                            return x;
                        }
                    }
                }
            }
        }

        private bool CheckNotWorking(Mat mat1, out int numberNotWorking) {

            numberNotWorking = 0;

            string[] files = System.IO.Directory.GetFiles(ConstValues.ImagePath, "*", System.IO.SearchOption.TopDirectoryOnly);

            Point loc = new Point(GlobalValues.ApplicationSetting.NotWorking.X, GlobalValues.ApplicationSetting.NotWorking.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.NotWorking.Width, GlobalValues.ApplicationSetting.NotWorking.Height);
            using (Mat mat2 = mat1.Clone(new OpenCvSharp.Rect(loc, sz))) {
                //mat2.ImWrite($@"D:\Work\Aaa\Aaa{_n++}.png");

                double max_value = 0;
                string max_x = "";

                for (int i = 0; i < files.Length; i++) {
                    using (Mat mat3 = new Mat(files[i], ImreadModes.Unchanged)) {
                        //bool rtn1 = OpenCvUtil.FindImage(mat2, mat3);
                        //if (rtn1) {
                        //    string x = Path.GetFileNameWithoutExtension(files[i]);
                        //    bool rtn2 = int.TryParse(x, out numberNotWorking);
                        //    return true;
                        //}
                        double rtn1;
                        rtn1 = OpenCvUtil.FindImage(mat2, mat3);
                        if (rtn1 > max_value) {
                            max_value = rtn1;
                            max_x= Path.GetFileNameWithoutExtension(files[i]);
                        }
                    }
                }

                if (max_value > 0.8d) {
                    bool rtn3 = int.TryParse(max_x, out numberNotWorking);
                    return true;
                }

                return false;

            }

        }

        private bool CheckNotWorking2(Mat mat, out int NumberNotWorking) {
            Point loc = new Point(GlobalValues.ApplicationSetting.NotWorking.X, GlobalValues.ApplicationSetting.NotWorking.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.NotWorking.Width, GlobalValues.ApplicationSetting.NotWorking.Height);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {

                using (Mat mat3 = OpenCvUtil.Reverse(mat2)) {
                    using (Mat mat4 = new Mat()) {
                        Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));

                        mat4.ImWrite(ConstValues.DebugBitmapFileName1);

                        using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                            tesseract.SetVariable("tessedit_char_whitelist", "1234567890/");
                            var page = tesseract.Process(mat4.ToBitmap());
                            string text = page.GetText();

                            //最前面
                            //MessageBox.Show(text: $"->{text}<-"
                            //    , caption: "AoEDEAlarm"
                            //    , buttons: MessageBoxButtons.OK
                            //    , icon: MessageBoxIcon.Information
                            //    , defaultButton: MessageBoxDefaultButton.Button1
                            //    , options: MessageBoxOptions.DefaultDesktopOnly
                            //    );

                            //数値に分解
                            return int.TryParse(text, out NumberNotWorking);

                        }


                    }
                }
            }
        }


        private bool CheckNotWorking1(Mat mat, out int NumberNotWorking) {
            Point loc = new Point(GlobalValues.ApplicationSetting.NotWorking.X, GlobalValues.ApplicationSetting.NotWorking.Y);
            Size sz = new Size(GlobalValues.ApplicationSetting.NotWorking.Width, GlobalValues.ApplicationSetting.NotWorking.Height);

            using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {
                using (var mat_hsv = mat2.CvtColor(ColorConversionCodes.BGR2HSV)) {
                    InputArray lower_white = InputArray.Create(new int[3] { 0, 0, 100 });
                    InputArray upper_white = InputArray.Create(new int[3] { 180, 45, 255 });
                    using (Mat mat_mask_white = mat_hsv.InRange(lower_white, upper_white)) {
                        using (Mat mat_reversed = OpenCvUtil.Reverse(mat_mask_white)) {
                            Cv2.Resize(mat_reversed, mat_reversed, new Size(mat_reversed.Width * 4, mat_reversed.Height * 4));
                            //Cv2.ImShow("Input image for Tesseract", mat_reversed);
                            //Cv2.WaitKey(1000);

                            mat_reversed.ImWrite(ConstValues.DebugBitmapFileName2);

                            using (var tesseract = new TesseractEngine(ConstValues.TessdataPath, "eng", EngineMode.LstmOnly)) {
                                tesseract.SetVariable("tessedit_char_whitelist", "1234567890");
                                var page = tesseract.Process(mat_reversed.ToBitmap());
                                string text = page.GetText();

                                //最前面
                                //MessageBox.Show(text: $"->{text}<-"
                                //    , caption: "AoEDEAlarm"
                                //    , buttons: MessageBoxButtons.OK
                                //    , icon: MessageBoxIcon.Information
                                //    , defaultButton: MessageBoxDefaultButton.Button1
                                //    , options: MessageBoxOptions.DefaultDesktopOnly
                                //    );

                                return int.TryParse(text, out NumberNotWorking);

                            }

                        }
                    }
                }
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
