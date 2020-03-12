using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoEDEAlarm {
    public class OpenCvUtil {

        public static double FindImage(Mat mat1, Mat mat2, double thresh) {
            using (Mat result = new Mat()) {
                // テンプレートマッチ
                try {
                    Cv2.MatchTemplate(mat1, mat2, result, TemplateMatchModes.CCoeffNormed);

                    // しきい値の範囲に絞る
                    Cv2.Threshold(result, result, thresh, 1.0, ThresholdTypes.Tozero);

                    // 類似度が最大/最小となる画素の位置を調べる
                    OpenCvSharp.Point minloc, maxloc;
                    double minval, maxval;
                    Cv2.MinMaxLoc(result, out minval, out maxval, out minloc, out maxloc);

                    if (maxval >= thresh) {
                        // 見つかった場所に赤枠を表示
                        //Rect rect = new Rect(maxloc.X, maxloc.Y, mat2.Width, mat2.Height);
                        //Cv2.Rectangle(mat1, rect, new OpenCvSharp.Scalar(0, 0, 255), 2);
                        //Cv2.ImShow(string.Format("min:<{0:f2}> max:<{1:f2}>", minval, maxval), mat1);
                        //Cv2.WaitKey(100);
                        return maxval;
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"{ex.Message}");
                }
                return 0;
            }

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

        public void Reverse2(Mat mat) {

            for (var y = 0; y < mat.Height; y++) {
                for (var x = 0; x < mat.Width; x++) {
                    var px = mat.Get<Vec3b>(y, x);
                    px[0] = (byte)(255 - px[0]);
                    px[1] = (byte)(255 - px[1]);
                    px[2] = (byte)(255 - px[2]);
                    mat.Set(y, x, px);
                }
            }
        }

        internal static Point Enlarge(Point location, int uiScale) {
            double v = (double)uiScale / 100d;
            return new OpenCvSharp.Point((double)location.X * v, (double)location.Y * v);
        }

        internal static Size Enlarge(Size size, int uiScale) {
            double v = (double)uiScale / 100d;
            return new OpenCvSharp.Size((double)size.Width * v, (double)size.Height * v);
        }

    }
}
