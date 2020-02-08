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
using System.Threading;
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
        private ContextMenu _cm = new ContextMenu();
        private MenuItem _mi_AdjustVolume = new MenuItem();
        private MenuItem _mi_StartMonitoring = new MenuItem();
        private MenuItem _mi_StopMonitoring = new MenuItem();
        private MenuItem _mi_ShowPositionSettingForm = new MenuItem();
        private MenuItem _mi_ExitProgram = new MenuItem();

        //private class BackData {
        //    internal DateTime TimeStump { get; set; }
        //    internal string Message { get; set; }
        //}
        //private List<BackData> _BackData;

        public TransparentForm() {
            InitializeComponent();
            //_BackData = new List<BackData>();
        }

        private void Msg_Load(object sender, EventArgs e) {

            _mi_AdjustVolume.Text = "音量調節";
            _mi_AdjustVolume.Click += Mi_Click;
            _cm.MenuItems.Add(_mi_AdjustVolume);

            _mi_StartMonitoring.Text = "監視開始";
            _mi_StartMonitoring.Click += _mi_StartMonitoring_Click;
            _cm.MenuItems.Add(_mi_StartMonitoring);

            _mi_StopMonitoring.Text = "監視中断";
            _mi_StopMonitoring.Click += _mi_StopMonitoring_Click;
            _cm.MenuItems.Add(_mi_StopMonitoring);

            _mi_ShowPositionSettingForm.Text = "位置設定";
            _mi_ShowPositionSettingForm.Click += _mi_ShowPositionSettingForm_Click; 
            _cm.MenuItems.Add(_mi_ShowPositionSettingForm);

            _mi_ExitProgram.Text = "アプリケーション終了";
            _mi_ExitProgram.Click += _mi_ExitProgram_Click;
            _cm.MenuItems.Add(_mi_ExitProgram);

            //画面移動用
            textBox1.MouseDown += new MouseEventHandler(Msg_MouseDown);
            textBox1.MouseMove += new MouseEventHandler(Msg_MouseMove);

            //最前面
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            //SetWindowPos(this.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE);

            //ウィンドウの位置を設定ファイルより復元
            this.Location = Properties.Settings.Default.FormPosition;

            //初期メッセージ
            GlobalValues.AddMessage($"監視開始：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Run)}");
            GlobalValues.AddMessage($"監視中断：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Stop)}");
            GlobalValues.AddMessage($"位置設定：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Customise)}");

            //モニタリング開始
            Start();

            //_BackData.Add(new BackData() { TimeStump = DateTime.Now, Message = $"監視開始：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Run)}" });
            //_BackData.Add(new BackData() { TimeStump = DateTime.Now, Message = $"監視中断：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Stop)}" });
            //_BackData.Add(new BackData() { TimeStump = DateTime.Now, Message = $"位置設定：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Customise)}" });

            //テキストボックスに反映する。
            //StringBuilder sb = new StringBuilder();
            //foreach (BackData x in _BackData) {
            //    sb.AppendLine(x.Message);
            //}
            //this.textBox1.Text = sb.ToString();
        }

        private void _mi_ExitProgram_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void _mi_ShowPositionSettingForm_Click(object sender, EventArgs e) {
            Program.ShowPositionSettingForm();
        }

        private void _mi_StartMonitoring_Click(object sender, EventArgs e) {
            Program.StartMonitoring();
        }

        private void _mi_StopMonitoring_Click(object sender, EventArgs e) {
            Program.StopMonitoring();
        }

        private void Mi_Click(object sender, EventArgs e) {
            VolumeAdjustForm f = new VolumeAdjustForm();
            f.ShowDialog();
        }

        //public void SetMessage(string message) {
        //    //末尾にメッセージを追加する。
        //    _BackData.Add(new BackData() { TimeStump = DateTime.Now, Message = message });
        //}

        //public void Start(string message) {
        public void Start() {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            //末尾にメッセージを追加する。
            //_BackData.Add(new BackData() {TimeStump= DateTime.Now, Message= message });

            this.Opacity = 0.9;
            Task.Run(async () => {
                while (!token.IsCancellationRequested) {
                    //Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                    this.Invoke((MethodInvoker)delegate {
                        //古いメッセージは削除する。
                        GlobalValues.BackDataList.RemoveAll(s => DateTime.Now - s.TimeStump > new TimeSpan(0, 0, 10));

                        //Listオブジェクトのメッセージをテキストボックスに反映する。
                        StringBuilder sb = new StringBuilder();
                        foreach (BackDataClass x in GlobalValues.BackDataList) {
                            sb.AppendLine(x.Message);
                        }
                        if (sb.Length > 0) {
                            //不透明度
                            this.Opacity = 0.90;
                        }
                        this.textBox1.Text = sb.ToString();
                    });

                    //数秒間待機する。
                    await Task.Delay(300);

                    this.Invoke((MethodInvoker)delegate {
                        if (this.Opacity > 0.25) {
                            //不透明度を下げる。
                            this.Opacity -= 0.05;
                        }
                    });

                }
            }, token);

        }

        private void Msg_MouseDown(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }

            if (e.Button == MouseButtons.Right) {
                _cm.Show(this, new Point(this.Width/2, this.Height/2));
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

            if (rtn == DialogResult.Cancel) {
                e.Cancel = true;
                return;
            }


            try {
                GlobalValues.AlarmTokenSource?.Cancel();
            } catch (Exception) {
            }

            Application.Exit();

        }

        private void TransparentForm_Activated(object sender, EventArgs e) {
            //アラーム画面にフォーカスがあることに気が付かずに、ゲーム画面の操作を行わなないようにするための対策。
            this.FormBorderStyle = FormBorderStyle.Fixed3D;

        }

        private void TransparentForm_Deactivate(object sender, EventArgs e) {
            //アラーム画面にフォーカスがあることに気が付かずに、ゲーム画面の操作を行わなないようにするための対策。
            this.FormBorderStyle = FormBorderStyle.None;

        }

        private void TransparentForm_Click(object sender, EventArgs e) {
        }
    }
}
