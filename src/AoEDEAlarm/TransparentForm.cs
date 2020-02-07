using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public partial class TransparentForm : Form {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd,
            int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_SHOWWINDOW = 0x0040;

        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;

        private Point mousePoint;

        private class BackData {
            internal DateTime TimeStump { get; set; }
            internal string Message { get; set; }
        }
        private List<BackData> _BackData;

        public TransparentForm() {
            InitializeComponent();
            _BackData = new List<BackData>();
        }

        public void Start(string message) {
            //末尾にメッセージを追加する。
            _BackData.Add(new BackData() {TimeStump= DateTime.Now, Message= message });

            this.Opacity = 0.9;
            Task.Run(async () => {
                while (this.Opacity > 0.15) {
                    this.Invoke((MethodInvoker)delegate {
                        this.Opacity -= 0.05;

                        //3秒経過したメッセージは削除する。
                        _BackData.RemoveAll(s => DateTime.Now - s.TimeStump > new TimeSpan(0, 0, 3));

                        //テキストボックスに反映する。
                        StringBuilder sb = new StringBuilder();
                        foreach (BackData x in _BackData) {
                            sb.AppendLine(x.Message);
                        }
                        this.textBox1.Text = sb.ToString();
                    });

                    //0.3秒待機
                    await Task.Delay(300);
                }
            });

        }

        private void Msg_Load(object sender, EventArgs e) {
            textBox1.MouseDown += new MouseEventHandler(Msg_MouseDown);
            textBox1.MouseMove += new MouseEventHandler(Msg_MouseMove);

            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            //SetWindowPos(this.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE);

            _BackData.Add(new BackData() { TimeStump = DateTime.Now, Message = $"監視開始：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Run)}" });
            _BackData.Add(new BackData() { TimeStump = DateTime.Now, Message = $"監視中断：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Stop)}" });
            _BackData.Add(new BackData() { TimeStump = DateTime.Now, Message = $"位置設定：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Customise)}" });
            //テキストボックスに反映する。
            StringBuilder sb = new StringBuilder();
            foreach (BackData x in _BackData) {
                sb.AppendLine(x.Message);
            }
            this.textBox1.Text = sb.ToString();


            this.Location = Properties.Settings.Default.FormPosition;
        }

        private void Msg_MouseDown(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }

        }

        private void Msg_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }

        }

        private void Msg_Paint(object sender, PaintEventArgs e) {
            //Graphics tbxRect = e.Graphics;
            //tbxRect.DrawRectangle(Pens.Black, textBox1.Left, textBox1.Top, textBox1.Width, textBox1.Height);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void textBox1_DoubleClick(object sender, EventArgs e) {
            if (this.Opacity > 0.3) {
                this.Opacity = 0.1;
            } else {
                this.Opacity = 0.5;
            }
        }

        private void Msg_DoubleClick(object sender, EventArgs e) {
            if (this.Opacity > 0.3) {
                this.Opacity = 0.1;
            } else {
                this.Opacity = 0.5;
            }

        }

        private void TransparentForm_FormClosing(object sender, FormClosingEventArgs e) {

            //フォーム位置の保存
            Properties.Settings.Default.FormPosition = this.Location;
            Properties.Settings.Default.Save();

            if (e.CloseReason == CloseReason.ApplicationExitCall) return;

            //最前面
            var rtn = MessageBox.Show(text: $"アプリケーションを終了してよろしいですか？"
                , caption: "AoEDEAlarm"
                , buttons: MessageBoxButtons.OKCancel
                , icon: MessageBoxIcon.Information
                , defaultButton: MessageBoxDefaultButton.Button1
                , options: MessageBoxOptions.DefaultDesktopOnly
                );

            if (rtn==DialogResult.Cancel) {
                e.Cancel = true;
                return;
            }


            try {
                GlobalValues.tokenSource?.Cancel();
            } catch (Exception) {
            }

            Application.Exit();

        }

        //private void SetTopMost() {
        //    const int HWND_TOPMOST = -1;
        //    const uint SWP_NOSIZE = 0x0001;
        //    const uint SWP_NOMOVE = 0x0002;
        //    const uint SWP_NOACTIVATE = 0x0010;
        //    const uint SWP_SHOWWINDOW = 0x0040;
        //    const uint SWP_NOSENDCHANGING = 0x0400;
        //    // SetWindowPosはどこかでDllImportする
        //    SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSENDCHANGING | SWP_NOSIZE | SWP_SHOWWINDOW);
        //}


    }
}
