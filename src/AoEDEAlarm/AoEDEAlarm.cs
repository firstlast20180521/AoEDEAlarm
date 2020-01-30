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

namespace AoEDEAlarm {
    class AoEDEAlarm : IDisposable {

        System.Media.SoundPlayer _SoundPlayer = new System.Media.SoundPlayer();

        public AoEDEAlarm() {
            _SoundPlayer = new System.Media.SoundPlayer(AoedeStaticGlobal.SoundFileName1);
        }

        public async Task<bool> Run(CancellationToken token) {

            while (true) {

                if (token.IsCancellationRequested) break;

                //フルスクリーン矩形を作成
                Rectangle rect = Screen.PrimaryScreen.Bounds;
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb)) {
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);

                    //Bitmap bmp = Win32Api.CaptureActiveWindow();

                    //一旦ファイルに上書き保存
                    //bmp.Save(AoedeGlobal.DebugBitmapFileName);

                    using (Mat mat = BitmapConverter.ToMat(bmp)) {
                        //Cv2.ImShow("test", mat);

                        Point loc = new Point(AoedeStaticGlobal.Settings.Population.X, AoedeStaticGlobal.Settings.Population.Y);
                        Size sz = new Size(AoedeStaticGlobal.Settings.Population.Width, AoedeStaticGlobal.Settings.Population.Height);

                        using (Mat mat2 = mat.Clone(new OpenCvSharp.Rect(loc, sz))) {

                            using (Mat mat3 = Reverse(mat2)) {
                                using (Mat mat4 = new Mat()) {
                                    Cv2.Resize(mat3, mat4, new Size(mat3.Width * 3, mat3.Height * 3));
                                    using (var tesseract = new TesseractEngine(AoedeStaticGlobal.TessdataPath, "eng", EngineMode.LstmOnly)) {
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
                                        if (arr.Length == 2) {
                                            int x1;
                                            int x2;
                                            bool r1 = int.TryParse(arr[0], out x1);
                                            bool r2 = int.TryParse(arr[1], out x2);
                                            if (r1 && r2) {
                                                if (x2 - x1 <= 1) {
                                                    //アラーム音を出す。
                                                    _SoundPlayer.PlaySync();
                                                }
                                            }
                                        }
                                        //await Task.Delay(1000);

                                    }


                                }
                            }
                        }

                    }
                }
                await Task.Delay(1000);
            }

            //}
            return true;
        }

        public static Mat Reverse(Mat src) {

            Mat mat = src.Clone();

            for (var y = 0; y < mat.Height; y++) {
                for (var x = 0; x < mat.Width; x++) {
                    var px = mat.Get<Vec3b>(y, x);
                    px[0] = (byte)(255 - px[0]);
                    px[1] = (byte)(255 - px[1]);
                    px[2] = (byte)(255 - px[2]);
                    mat.Set(y, x, px);
                }
            }

            return mat;

            //Cv2.ImShow("image", src);
            //Cv2.WaitKey();

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
