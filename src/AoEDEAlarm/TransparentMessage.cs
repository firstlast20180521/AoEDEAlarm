using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public class TransparentMessage {
        private System.Windows.Forms.Control control1;
        private Point _mousePoint;
        private MouseMoveMode _mouseMoveMode;
        private Size _formSize;

        private enum MouseMoveMode {
            Size,
            Location,
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd,
            int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_SHOWWINDOW = 0x0040;

        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;

        public delegate void ClickHandler(object sender, ContextMenuClickedEventArgs e);
        public event ClickHandler ContextMenuClick;

        public enum ContextMenuItems {
            AdjustVolume,
            StartMonitoring,
            StopMonitoring,
            ShowPositionSettingForm,
            ExitProgram,
        }
        public class ContextMenuClickedEventArgs : EventArgs {
            public ContextMenuItems contextMenuItem { get; set; }
        }

        private class AlarmMessageClass {
            public string Message { get; set; }
            public DateTime TimeStump { get; set; }
        }

        private TransparentForm _console;

        private ContextMenu _cm = new ContextMenu();
        private MenuItem _mi_AdjustVolume = new MenuItem();
        private MenuItem _mi_StartMonitoring = new MenuItem();
        private MenuItem _mi_StopMonitoring = new MenuItem();
        private MenuItem _mi_ShowPositionSettingForm = new MenuItem();
        private MenuItem _mi_ExitProgram = new MenuItem();

        private List<AlarmMessageClass> _messageList { get; set; }

        public TransparentMessage() {
            _messageList = new List<AlarmMessageClass>();

            _mi_AdjustVolume.Text = "音量調節";
            _mi_AdjustVolume.Click += _mi_AdjustVolume_Click;
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

            _console = new TransparentForm();
            _console.KeyPreview = true;
            _console.ShowIcon = false;
            _console.ShowInTaskbar = false;
            _console.AutoSize = true;
            _console.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            _console.MouseDown += _console_MouseDown;
            _console.MouseMove += _console_MouseMove;
            //_console.MouseUp += _console_MouseUp;  ;
            _console.Activated += _console_Activated;
            _console.Deactivate += _console_Deactivate;
            _console.FormClosing += _console_FormClosing;
            _console.DoubleClick += _console_DoubleClick;

            _console.FormBorderStyle = FormBorderStyle.None;
            _console.label1.Text = "";
            //_console.label1.Enabled = false;
            //_console..BackColor = SystemColors.Control;
            //_console.label1.ForeColor = SystemColors.WindowText;


            this.control1 = new System.Windows.Forms.Control();
            this.control1.Location = new System.Drawing.Point(300, 0);
            this.control1.Size = new System.Drawing.Size(0, 0);
            //this.control1.AllowDrop = true;
            //this.control1.BackColor = Color.Green;
            _console.Controls.Add(this.control1);

        }

        private void _console_DoubleClick(object sender, EventArgs e) {
            if (_console.Opacity > 0.3d) {
                _console.Opacity = 0.1d;
            } else {
                _console.Opacity = 0.9d;
            }
        }

        private void _console_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                _cm.Show(_console, new Point(e.X, e.Y));
            }

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                //位置を記憶する
                _mousePoint = new Point(e.X, e.Y);
                _formSize = new Size(_console.Width, _console.Height);

                if (_console.Width - e.X < 10 && _console.Height - e.Y < 10 ) {
                    _mouseMoveMode = MouseMoveMode.Size;
                } else {
                    _mouseMoveMode = MouseMoveMode.Location;
                }
            }

        }

        private void _console_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                if (_mouseMoveMode == MouseMoveMode.Size) {
                    _console.Width = _formSize.Width + e.X - _mousePoint.X;
                    _console.Height = _formSize.Height + e.Y - _mousePoint.Y;
                    this.control1.Left = e.X > 30 ? e.X:30;
                    this.control1.Top = e.Y;

                } else {
                    _console.Left += e.X - _mousePoint.X;
                    _console.Top += e.Y - _mousePoint.Y;

                }
            }
        }

        //private void _console_MouseUp(object sender, MouseEventArgs e) {
        //    if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
        //        if (_mouseMoveMode == MouseMoveMode.Size) {
        //            this.control1.Left = _console.Width;
        //        }
        //    }
        //}


        private void _console_FormClosing(object sender, FormClosingEventArgs e) {
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

            Application.Exit();

        }

        private void _console_Activated(object sender, EventArgs e) {
            //アラーム画面にフォーカスがあることに気が付かずに、ゲーム画面の操作を行わなないようにするための対策。
            //_console.FormBorderStyle = FormBorderStyle.Fixed3D;
            _console.AutoSizeMode = AutoSizeMode.GrowOnly;
            //_console.BackColor = SystemColors.Control;
            _console.label1.Enabled = false;
            _console.Opacity = 0.9d;
        }

        private void _console_Deactivate(object sender, EventArgs e) {
            //アラーム画面にフォーカスがあることに気が付かずに、ゲーム画面の操作を行わなないようにするための対策。
            //_console.FormBorderStyle = FormBorderStyle.None;
            _console.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //_console.BackColor = SystemColors.Control;
            _console.label1.Enabled = true;
            _console.Opacity = 0.5d;
        }

        public void Add(string message) {
            _messageList.Add(new AlarmMessageClass { Message = message, TimeStump = DateTime.Now });
        }

        public void Show() {
            //最前面
            SetWindowPos(_console.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            _console.Show();

            //Listオブジェクトのメッセージをリストボックスに反映する。
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _messageList.Count; i++) {
                sb.AppendLine(_messageList[i].Message);
            }
            _console.label1.Text = sb.ToString();
            _console.label1.Enabled = true;
            _console.Opacity = 0.5;

        }

        public void Start() {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            //末尾にメッセージを追加する。
            //_BackData.Add(new BackData() {TimeStump= DateTime.Now, Message= message });

            _console.Opacity = 0.5;
            Task.Run(async () => {
                while (!token.IsCancellationRequested) {
                    _console.Invoke((MethodInvoker)delegate {
                        //古いメッセージは削除する。
                        _messageList.RemoveAll(x => (DateTime.Now - x.TimeStump > new TimeSpan(0, 0, 10)));

                        //Listオブジェクトのメッセージをリストボックスに反映する。
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < _messageList.Count; i++) {
                            sb.AppendLine(_messageList[i].Message);
                        }
                        _console.label1.Text = sb.ToString();
                        if (sb.Length > 0) {
                            //不透明度
                            _console.Opacity = 0.5d;
                        }
                    });

                    //数秒間待機する。
                    await Task.Delay(300);

                    _console.Invoke((MethodInvoker)delegate {
                        if (_console.Opacity > 0.25) {
                            //不透明度を下げる。
                            _console.Opacity -= 0.05;
                        }
                    });

                }
            }, token);


        }

        public void Finish() {
            _console.Close();
        }

        private void _mi_AdjustVolume_Click(object sender, EventArgs e) {
            ContextMenuClick(sender, new ContextMenuClickedEventArgs() { contextMenuItem = ContextMenuItems.AdjustVolume });
        }
        private void _mi_StartMonitoring_Click(object sender, EventArgs e) {
            ContextMenuClick(sender, new ContextMenuClickedEventArgs() { contextMenuItem = ContextMenuItems.StartMonitoring });
        }
        private void _mi_StopMonitoring_Click(object sender, EventArgs e) {
            ContextMenuClick(sender, new ContextMenuClickedEventArgs() { contextMenuItem = ContextMenuItems.StopMonitoring });
        }
        private void _mi_ShowPositionSettingForm_Click(object sender, EventArgs e) {
            ContextMenuClick(sender, new ContextMenuClickedEventArgs() { contextMenuItem = ContextMenuItems.ShowPositionSettingForm });
        }
        private void _mi_ExitProgram_Click(object sender, EventArgs e) {
            ContextMenuClick(sender, new ContextMenuClickedEventArgs() { contextMenuItem = ContextMenuItems.ExitProgram });
        }

    }
}
