using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public partial class PositionSettingForm : Form {

        private MouseEventArgs sPos;
        private MouseEventArgs ePos;

        public PositionSettingForm() {
            InitializeComponent();
        }

        private enum ResourceKind {
            Wood,
            Food,
            Gold,
            Stone,
            Population,
        }

        private RadioButton[] radioButtons = new RadioButton[Enum.GetNames(typeof(ResourceKind)).Length];
        private PictureBox[] pictureBoxes = new PictureBox[Enum.GetNames(typeof(ResourceKind)).Length];

        AoEDEAlarmSettings _ps = new AoEDEAlarmSettings {
            Wood = new AoEDEAlarmSettings.Rectangle {
                X = AoedeStaticGlobal.Settings.Wood.X,
                Y = AoedeStaticGlobal.Settings.Wood.Y,
                Width = AoedeStaticGlobal.Settings.Wood.Width,
                Height = AoedeStaticGlobal.Settings.Wood.Height,
            },

            Food = new AoEDEAlarmSettings.Rectangle {
                X = AoedeStaticGlobal.Settings.Food.X,
                Y = AoedeStaticGlobal.Settings.Food.Y,
                Width = AoedeStaticGlobal.Settings.Food.Width,
                Height = AoedeStaticGlobal.Settings.Food.Height,
            },

            Gold = new AoEDEAlarmSettings.Rectangle {
                X = AoedeStaticGlobal.Settings.Gold.X,
                Y = AoedeStaticGlobal.Settings.Gold.Y,
                Width = AoedeStaticGlobal.Settings.Gold.Width,
                Height = AoedeStaticGlobal.Settings.Gold.Height,
            },

            Stone = new AoEDEAlarmSettings.Rectangle {
                X = AoedeStaticGlobal.Settings.Stone.X,
                Y = AoedeStaticGlobal.Settings.Stone.Y,
                Width = AoedeStaticGlobal.Settings.Stone.Width,
                Height = AoedeStaticGlobal.Settings.Stone.Height,
            },

            Population = new AoEDEAlarmSettings.Rectangle {
                X = AoedeStaticGlobal.Settings.Population.X,
                Y = AoedeStaticGlobal.Settings.Population.Y,
                Width = AoedeStaticGlobal.Settings.Population.Width,
                Height = AoedeStaticGlobal.Settings.Population.Height,
            },

        };



        private void PositionSettingForm_Load(object sender, EventArgs e) {

            //Radio button
            radioButtons[0] = this.rdoWood;
            radioButtons[1] = this.rdoFood;
            radioButtons[2] = this.rdoGold;
            radioButtons[3] = this.rdoStone;
            radioButtons[4] = this.rdoPopulation;

            radioButtons[0].Focus();

            for (int i = 0; i < radioButtons.Length; i++) {
                radioButtons[i].CheckedChanged += new System.EventHandler(this.rdoX_CheckedChanged);
            }

            //Picture box
            pictureBoxes[0] = this.pctWood;
            pictureBoxes[1] = this.pctFood;
            pictureBoxes[2] = this.pctGold;
            pictureBoxes[3] = this.pctStone;
            pictureBoxes[4] = this.pctPopulation;

            //フルスクリーン矩形を作成
            Rectangle rect = Screen.PrimaryScreen.Bounds;

            using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb)) {
                //キャプチャ画像を作成する。
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);

                //Bitmap bmp = Win32Api.CaptureActiveWindow();

                //pictureBox1用の画像を作成する。
                pctCanvas.Image = new Bitmap(rect.Width, rect.Height);
                pctCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                Graphics g2 = Graphics.FromImage(pctCanvas.Image);
                g2.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
                g2.Dispose();

                DrawPicture(pctCanvas, _ps.Wood.X, _ps.Wood.Y, _ps.Wood.Width, _ps.Wood.Height, pctWood);
                DrawPicture(pctCanvas, _ps.Food.X, _ps.Food.Y, _ps.Food.Width, _ps.Food.Height, pctFood);
                DrawPicture(pctCanvas, _ps.Gold.X, _ps.Gold.Y, _ps.Gold.Width, _ps.Gold.Height, pctGold);
                DrawPicture(pctCanvas, _ps.Stone.X, _ps.Stone.Y, _ps.Stone.Width, _ps.Stone.Height, pctStone);
                DrawPicture(pctCanvas, _ps.Population.X, _ps.Population.Y, _ps.Population.Width, _ps.Population.Height, pctPopulation);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                Graphics g = pctCanvas.CreateGraphics();
                Pen bpen = new Pen(Color.Black, 1);
                bpen.DashStyle = DashStyle.Dash;
                pctCanvas.Refresh();
                g.DrawRectangle(bpen, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y);
                ePos = e;
                g.Dispose();

            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
            Console.WriteLine($"Down(x:{e.X}, y:{e.Y})");
            if (e.Button == MouseButtons.Left) {
                //'開始点の取得
                sPos = e;
                ePos = e;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
            //Console.WriteLine($"Up(x:{e.X}, y:{e.Y})");
            if (e.Button == MouseButtons.Left) {
                pctCanvas.Refresh();  //最後の四角形を削除
                Graphics g = pctCanvas.CreateGraphics();
                Pen bpen = new Pen(Color.White, 1);
                bpen.DashStyle = DashStyle.Dash;

                //範囲確定の四角形を描く
                g.DrawRectangle(bpen, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y);
                g.Dispose();

                //-------------- 指定範囲の画像取得部分 -----------------

                if (rdoWood.Checked) {
                    _ps.Wood.X = sPos.X;
                    _ps.Wood.Y = sPos.Y;
                    _ps.Wood.Width = ePos.X - sPos.X;
                    _ps.Wood.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctWood);
                    return;
                }

                if (rdoFood.Checked) {
                    _ps.Food.X = sPos.X;
                    _ps.Food.Y = sPos.Y;
                    _ps.Food.Width = ePos.X - sPos.X;
                    _ps.Food.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctFood);
                    return;
                }

                if (rdoGold.Checked) {
                    _ps.Gold.X = sPos.X;
                    _ps.Gold.Y = sPos.Y;
                    _ps.Gold.Width = ePos.X - sPos.X;
                    _ps.Gold.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctGold);
                    return;
                }

                if (rdoStone.Checked) {
                    _ps.Stone.X = sPos.X;
                    _ps.Stone.Y = sPos.Y;
                    _ps.Stone.Width = ePos.X - sPos.X;
                    _ps.Stone.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctStone);
                    return;
                }

                if (rdoPopulation.Checked) {
                    _ps.Population.X = sPos.X;
                    _ps.Population.Y = sPos.Y;
                    _ps.Population.Width = ePos.X - sPos.X;
                    _ps.Population.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctPopulation);
                    return;
                }

            }

        }

        private void DrawPicture(PictureBox pictureBox1, int x, int y, int width, int height, PictureBox pictureBox2) {

            Bitmap bmp = new Bitmap(pictureBox1.Image);

            //四角形の範囲の画像を取得
            Rectangle rect = new Rectangle(x, y, width, height);

            //選択範囲が異常の場合表示処理をしない
            if (width < 2 || height < 2) return;

            pictureBox2.Image = new Bitmap(width, height);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            Graphics g2 = Graphics.FromImage(pictureBox2.Image);

            g2.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
            
            g2.Dispose();

            bmp.Dispose();

            return;

        }

        private void PositionSettingForm_FormClosing(object sender, FormClosingEventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            AoedeStaticGlobal.Settings.Wood.X = _ps.Wood.X;
            AoedeStaticGlobal.Settings.Wood.Y = _ps.Wood.Y;
            AoedeStaticGlobal.Settings.Wood.Width = _ps.Wood.Width;
            AoedeStaticGlobal.Settings.Wood.Height = _ps.Wood.Height;

            AoedeStaticGlobal.Settings.Food.X = _ps.Food.X;
            AoedeStaticGlobal.Settings.Food.Y = _ps.Food.Y;
            AoedeStaticGlobal.Settings.Food.Width = _ps.Food.Width;
            AoedeStaticGlobal.Settings.Food.Height = _ps.Food.Height;

            AoedeStaticGlobal.Settings.Gold.X = _ps.Gold.X;
            AoedeStaticGlobal.Settings.Gold.Y = _ps.Gold.Y;
            AoedeStaticGlobal.Settings.Gold.Width = _ps.Gold.Width;
            AoedeStaticGlobal.Settings.Gold.Height = _ps.Gold.Height;

            AoedeStaticGlobal.Settings.Stone.X = _ps.Stone.X;
            AoedeStaticGlobal.Settings.Stone.Y = _ps.Stone.Y;
            AoedeStaticGlobal.Settings.Stone.Width = _ps.Stone.Width;
            AoedeStaticGlobal.Settings.Stone.Height = _ps.Stone.Height;

            AoedeStaticGlobal.Settings.Population.X = _ps.Population.X;
            AoedeStaticGlobal.Settings.Population.Y = _ps.Population.Y;
            AoedeStaticGlobal.Settings.Population.Width = _ps.Population.Width;
            AoedeStaticGlobal.Settings.Population.Height = _ps.Population.Height;

            AoEDEAlarmSettings.SaveXml(AoedeStaticGlobal.Settings);

            MessageBox.Show(text: "位置を保存しました。"
                , caption: "画像位置設定"
                , buttons: MessageBoxButtons.OK
                , icon: MessageBoxIcon.Information
                , defaultButton: MessageBoxDefaultButton.Button1
                , options: MessageBoxOptions.DefaultDesktopOnly
                );

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();

        }

        private void rdoX_CheckedChanged(object sender, EventArgs e) {

            pctCanvas.Refresh();  //最後の四角形を削除
            Graphics g = pctCanvas.CreateGraphics();
            Pen bpen = new Pen(Color.Red, 1);
            bpen.DashStyle = DashStyle.Solid;

            //範囲確定の四角形を描く
            if (radioButtons[(int)ResourceKind.Wood].Checked) {
                g.DrawRectangle(bpen, _ps.Wood.X, _ps.Wood.Y, _ps.Wood.Width, _ps.Wood.Height);
                return;
            }

            if (radioButtons[(int)ResourceKind.Food].Checked) {
                g.DrawRectangle(bpen, _ps.Food.X, _ps.Food.Y, _ps.Food.Width, _ps.Food.Height);
                return;
            }

            if (radioButtons[(int)ResourceKind.Gold].Checked) {
                g.DrawRectangle(bpen, _ps.Gold.X, _ps.Gold.Y, _ps.Gold.Width, _ps.Gold.Height);
                return;
            }

            if (radioButtons[(int)ResourceKind.Stone].Checked) {
                g.DrawRectangle(bpen, _ps.Stone.X, _ps.Stone.Y, _ps.Stone.Width, _ps.Stone.Height);
                return;
            }

            if (radioButtons[(int)ResourceKind.Population].Checked) {
                g.DrawRectangle(bpen, _ps.Population.X, _ps.Population.Y, _ps.Population.Width, _ps.Population.Height);
                return;
            }

            g.Dispose();

        }

    }
}
