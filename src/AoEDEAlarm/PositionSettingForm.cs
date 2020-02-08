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

        private enum PositionKind {
            Wood,
            Food,
            Gold,
            Stone,
            Housing,
            NotWorking,
            Players,
            MiniMap,
        }

        private RadioButton[] radioButtons = new RadioButton[Enum.GetNames(typeof(PositionKind)).Length];
        private PictureBox[] pictureBoxes = new PictureBox[Enum.GetNames(typeof(PositionKind)).Length];

        ApplicationSettingClass _ps = new ApplicationSettingClass {
            Wood = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.Wood.X,
                Y = GlobalValues.ApplicationSetting.Wood.Y,
                Width = GlobalValues.ApplicationSetting.Wood.Width,
                Height = GlobalValues.ApplicationSetting.Wood.Height,
            },

            Food = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.Food.X,
                Y = GlobalValues.ApplicationSetting.Food.Y,
                Width = GlobalValues.ApplicationSetting.Food.Width,
                Height = GlobalValues.ApplicationSetting.Food.Height,
            },

            Gold = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.Gold.X,
                Y = GlobalValues.ApplicationSetting.Gold.Y,
                Width = GlobalValues.ApplicationSetting.Gold.Width,
                Height = GlobalValues.ApplicationSetting.Gold.Height,
            },

            Stone = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.Stone.X,
                Y = GlobalValues.ApplicationSetting.Stone.Y,
                Width = GlobalValues.ApplicationSetting.Stone.Width,
                Height = GlobalValues.ApplicationSetting.Stone.Height,
            },

            Housing = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.Housing.X,
                Y = GlobalValues.ApplicationSetting.Housing.Y,
                Width = GlobalValues.ApplicationSetting.Housing.Width,
                Height = GlobalValues.ApplicationSetting.Housing.Height,
            },

            NotWorking = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.NotWorking.X,
                Y = GlobalValues.ApplicationSetting.NotWorking.Y,
                Width = GlobalValues.ApplicationSetting.NotWorking.Width,
                Height = GlobalValues.ApplicationSetting.NotWorking.Height,
            },

            Players = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.Players.X,
                Y = GlobalValues.ApplicationSetting.Players.Y,
                Width = GlobalValues.ApplicationSetting.Players.Width,
                Height = GlobalValues.ApplicationSetting.Players.Height,
            },

            MiniMap = new ApplicationSettingClass.Rectangle {
                X = GlobalValues.ApplicationSetting.MiniMap.X,
                Y = GlobalValues.ApplicationSetting.MiniMap.Y,
                Width = GlobalValues.ApplicationSetting.MiniMap.Width,
                Height = GlobalValues.ApplicationSetting.MiniMap.Height,
            },

        };



        private void PositionSettingForm_Load(object sender, EventArgs e) {

            //Radio button
            radioButtons[0] = this.rdoWood;
            radioButtons[1] = this.rdoFood;
            radioButtons[2] = this.rdoGold;
            radioButtons[3] = this.rdoStone;
            radioButtons[4] = this.rdoHousing;
            radioButtons[5] = this.rdoNotWorking;
            radioButtons[6] = this.rdoPlayers;
            radioButtons[7] = this.rdoMiniMap;

            radioButtons[0].Focus();

            for (int i = 0; i < radioButtons.Length; i++) {
                radioButtons[i].CheckedChanged += new System.EventHandler(this.rdoX_CheckedChanged);
            }

            //Picture box
            pictureBoxes[0] = this.pctWood;
            pictureBoxes[1] = this.pctFood;
            pictureBoxes[2] = this.pctGold;
            pictureBoxes[3] = this.pctStone;
            pictureBoxes[4] = this.pctHousing;
            pictureBoxes[5] = this.pctNotWorking;
            pictureBoxes[6] = this.pctPlayers;
            pictureBoxes[7] = this.pctMiniMap;

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
                DrawPicture(pctCanvas, _ps.Housing.X, _ps.Housing.Y, _ps.Housing.Width, _ps.Housing.Height, pctHousing);
                DrawPicture(pctCanvas, _ps.NotWorking.X, _ps.NotWorking.Y, _ps.NotWorking.Width, _ps.NotWorking.Height, pctNotWorking);
                DrawPicture(pctCanvas, _ps.Players.X, _ps.Players.Y, _ps.Players.Width, _ps.Players.Height, pctPlayers);
                DrawPicture(pctCanvas, _ps.MiniMap.X, _ps.MiniMap.Y, _ps.MiniMap.Width, _ps.MiniMap.Height, pctMiniMap);
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

                if (rdoHousing.Checked) {
                    _ps.Housing.X = sPos.X;
                    _ps.Housing.Y = sPos.Y;
                    _ps.Housing.Width = ePos.X - sPos.X;
                    _ps.Housing.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctHousing);
                    return;
                }

                if (rdoNotWorking.Checked) {
                    _ps.NotWorking.X = sPos.X;
                    _ps.NotWorking.Y = sPos.Y;
                    _ps.NotWorking.Width = ePos.X - sPos.X;
                    _ps.NotWorking.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctNotWorking);
                    return;
                }

                if (rdoPlayers.Checked) {
                    _ps.Players.X = sPos.X;
                    _ps.Players.Y = sPos.Y;
                    _ps.Players.Width = ePos.X - sPos.X;
                    _ps.Players.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctPlayers);
                    return;
                }

                if (rdoMiniMap.Checked) {
                    _ps.MiniMap.X = sPos.X;
                    _ps.MiniMap.Y = sPos.Y;
                    _ps.MiniMap.Width = ePos.X - sPos.X;
                    _ps.MiniMap.Height = ePos.Y - sPos.Y;

                    DrawPicture(pctCanvas, sPos.X, sPos.Y, ePos.X - sPos.X, ePos.Y - sPos.Y, pctMiniMap);
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

            SaveData();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();

        }

        private void button3_Click(object sender, EventArgs e) {
            SaveData();

            MessageBox.Show(text: "位置を保存しました。"
                , caption: "画像位置設定"
                , buttons: MessageBoxButtons.OK
                , icon: MessageBoxIcon.Information
                , defaultButton: MessageBoxDefaultButton.Button1
                , options: MessageBoxOptions.DefaultDesktopOnly
                );


        }

        private void SaveData() {
            //10,23 
            if (_ps.NotWorking.Width < 23) {
                MessageBox.Show(text: "遊び農民の幅が足りません。"
                    , caption: "画像位置設定"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;
            }

            if (_ps.NotWorking.Height < 10) {
                MessageBox.Show(text: "遊び農民の高さが足りません。"
                    , caption: "画像位置設定"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;
            }

            GlobalValues.ApplicationSetting.Wood.X = _ps.Wood.X;
            GlobalValues.ApplicationSetting.Wood.Y = _ps.Wood.Y;
            GlobalValues.ApplicationSetting.Wood.Width = _ps.Wood.Width;
            GlobalValues.ApplicationSetting.Wood.Height = _ps.Wood.Height;

            GlobalValues.ApplicationSetting.Food.X = _ps.Food.X;
            GlobalValues.ApplicationSetting.Food.Y = _ps.Food.Y;
            GlobalValues.ApplicationSetting.Food.Width = _ps.Food.Width;
            GlobalValues.ApplicationSetting.Food.Height = _ps.Food.Height;

            GlobalValues.ApplicationSetting.Gold.X = _ps.Gold.X;
            GlobalValues.ApplicationSetting.Gold.Y = _ps.Gold.Y;
            GlobalValues.ApplicationSetting.Gold.Width = _ps.Gold.Width;
            GlobalValues.ApplicationSetting.Gold.Height = _ps.Gold.Height;

            GlobalValues.ApplicationSetting.Stone.X = _ps.Stone.X;
            GlobalValues.ApplicationSetting.Stone.Y = _ps.Stone.Y;
            GlobalValues.ApplicationSetting.Stone.Width = _ps.Stone.Width;
            GlobalValues.ApplicationSetting.Stone.Height = _ps.Stone.Height;

            GlobalValues.ApplicationSetting.Housing.X = _ps.Housing.X;
            GlobalValues.ApplicationSetting.Housing.Y = _ps.Housing.Y;
            GlobalValues.ApplicationSetting.Housing.Width = _ps.Housing.Width;
            GlobalValues.ApplicationSetting.Housing.Height = _ps.Housing.Height;

            GlobalValues.ApplicationSetting.NotWorking.X = _ps.NotWorking.X;
            GlobalValues.ApplicationSetting.NotWorking.Y = _ps.NotWorking.Y;
            GlobalValues.ApplicationSetting.NotWorking.Width = _ps.NotWorking.Width;
            GlobalValues.ApplicationSetting.NotWorking.Height = _ps.NotWorking.Height;

            GlobalValues.ApplicationSetting.Players.X = _ps.Players.X;
            GlobalValues.ApplicationSetting.Players.Y = _ps.Players.Y;
            GlobalValues.ApplicationSetting.Players.Width = _ps.Players.Width;
            GlobalValues.ApplicationSetting.Players.Height = _ps.Players.Height;

            GlobalValues.ApplicationSetting.MiniMap.X = _ps.MiniMap.X;
            GlobalValues.ApplicationSetting.MiniMap.Y = _ps.MiniMap.Y;
            GlobalValues.ApplicationSetting.MiniMap.Width = _ps.MiniMap.Width;
            GlobalValues.ApplicationSetting.MiniMap.Height = _ps.MiniMap.Height;

            XmlUtilityClass<ApplicationSettingClass>.SaveXml(GlobalValues.ApplicationSetting, ConstValues.ApplicationSettingFileName);

        }

        private void rdoX_CheckedChanged(object sender, EventArgs e) {
            
            DrawWholePictures();


        }

        private void DrawWholePictures() {
            pctCanvas.Refresh();  //最後の四角形を削除
            Graphics g = pctCanvas.CreateGraphics();
            Pen bpen = new Pen(Color.Red, 1);
            bpen.DashStyle = DashStyle.Solid;

            //範囲確定の四角形を描く
            if (radioButtons[(int)PositionKind.Wood].Checked) {
                g.DrawRectangle(bpen, _ps.Wood.X, _ps.Wood.Y, _ps.Wood.Width, _ps.Wood.Height);
                return;
            }

            if (radioButtons[(int)PositionKind.Food].Checked) {
                g.DrawRectangle(bpen, _ps.Food.X, _ps.Food.Y, _ps.Food.Width, _ps.Food.Height);
                return;
            }

            if (radioButtons[(int)PositionKind.Gold].Checked) {
                g.DrawRectangle(bpen, _ps.Gold.X, _ps.Gold.Y, _ps.Gold.Width, _ps.Gold.Height);
                return;
            }

            if (radioButtons[(int)PositionKind.Stone].Checked) {
                g.DrawRectangle(bpen, _ps.Stone.X, _ps.Stone.Y, _ps.Stone.Width, _ps.Stone.Height);
                return;
            }

            if (radioButtons[(int)PositionKind.Housing].Checked) {
                g.DrawRectangle(bpen, _ps.Housing.X, _ps.Housing.Y, _ps.Housing.Width, _ps.Housing.Height);
                return;
            }

            if (radioButtons[(int)PositionKind.NotWorking].Checked) {
                g.DrawRectangle(bpen, _ps.NotWorking.X, _ps.NotWorking.Y, _ps.NotWorking.Width, _ps.NotWorking.Height);
                return;
            }

            if (radioButtons[(int)PositionKind.Players].Checked) {
                g.DrawRectangle(bpen, _ps.Players.X, _ps.Players.Y, _ps.Players.Width, _ps.Players.Height);
                return;
            }

            if (radioButtons[(int)PositionKind.MiniMap].Checked) {
                g.DrawRectangle(bpen, _ps.MiniMap.X, _ps.MiniMap.Y, _ps.MiniMap.Width, _ps.MiniMap.Height);
                return;
            }

            g.Dispose();

        }

        private void PositionSettingForm_Paint(object sender, PaintEventArgs e) {

        }

        private void pctCanvas_Paint(object sender, PaintEventArgs e) {
            //DrawWholePictures();

        }

        private void pctCanvas_Resize(object sender, EventArgs e) {

        }
    }
}
