using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace AoEDEAlarm {
    class Program {

        public static AoedeGlobal _g = new AoedeGlobal();

        static void Main(string[] args) {

            var assem = System.Reflection.Assembly.GetExecutingAssembly();
            var attr = (System.Runtime.InteropServices.GuidAttribute)Attribute.GetCustomAttribute(assem, typeof(System.Runtime.InteropServices.GuidAttribute));
            var guid = attr.Value;

            System.Threading.Mutex mutex;
            bool hasHandle = false;

            using (mutex = new System.Threading.Mutex(false, guid)) {
                try {
                    try {
                        // Mutexの所有権を要求
                        hasHandle = mutex.WaitOne(0, false);

                    } catch (System.Threading.AbandonedMutexException) {
                        // 別アプリがMutexオブジェクトを開放しないで終了した場合
                        hasHandle = true;

                        //throw;
                    }
                    if (hasHandle == false) {
                        // Mutexの所有権が得られなかったため、起動済みと判断して終了
                        MessageBox.Show("既に起動されています。");
                        return;
                    }

                    //---------------------------------------------------------
                    // アプリケーション開始
                    //---------------------------------------------------------
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    AoedeGlobal.Settings = AoEDEAlarmSettings.LoadXml();
                    StartHotkeys();
                    NotifyIcon ni = SetNotifyIcon();

                    System.Windows.Forms.Application.Run();

                    ni.Dispose();
                    AoEDEAlarmSettings.SaveXml(AoedeGlobal.Settings);

                } catch (Exception ex) {
                    // アプリケーション例外処理
                    Console.WriteLine(ex.Message);

                    //throw;
                } finally {
                    if (hasHandle) {
                        mutex.ReleaseMutex();
                    }
                    mutex.Close();

                }
            }


        }

        private static NotifyIcon SetNotifyIcon() {
            NotifyIcon ni;

            //NotifyIcon ni = new NotifyIcon();
            ni = new NotifyIcon();
            ni.Icon = Properties.Resources.Leaf32;
            ni.Text = "AoEDEAlarm";
            ni.Visible = true;

            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem item1 = new ToolStripMenuItem() {
                Text = "終了",
                Image = null,
                //Font = new System.Drawing.Font("游明朝", 14),
                Name = "終了ToolStripMenuItem",
            };
            item1.Click += Item1_Click;

            ToolStripMenuItem item2 = new ToolStripMenuItem() {
                Text = "ホットキー設定",
                Image = null,
                //Font = new System.Drawing.Font("游明朝", 14),
                Name = "ホットキー設定ToolStripMenuItem",
            };
            item2.Click += Item2_Click;

            ToolStripMenuItem item3 = new ToolStripMenuItem() {
                Text = "ヘルプ",
                Image = null,
                //Font = new System.Drawing.Font("游明朝", 14),
                Name = "ヘルプToolStripMenuItem",
            };
            item3.Click += Item3_Click;

            cms.Items.Add(item1);
            cms.Items.Add(item2);
            cms.Items.Add(item3);
            ni.ContextMenuStrip = cms;

            return ni;

        }

        private static void StartHotkeys() {
            _g.Hotkey_Run = new HotKey((Keys)AoedeGlobal.Settings.Hotkey_Run);
            _g.Hotkey_Stop = new HotKey((Keys)AoedeGlobal.Settings.Hotkey_Stop);
            _g.Hotkey_Customise = new HotKey((Keys)AoedeGlobal.Settings.Hotkey_Customise);

            _g.Hotkey_Run.HotkeyEvent += Hk_Run_HotkeyEvent;
            _g.Hotkey_Stop.HotkeyEvent += Hk_Stop_HotkeyEvent;
            _g.Hotkey_Customise.HotkeyEvent += Hk_Setting_HotkeyEvent;

        }

        private static void FinishHotkeys() {
            _g.Hotkey_Run.Dispose();
            _g.Hotkey_Stop.Dispose();
            _g.Hotkey_Customise.Dispose();
        }

        /// <summary>
        /// 監視処理開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hk_Run_HotkeyEvent(object sender, EventArgs e) {

            if (AoedeGlobal.IsRunning) {
                MessageBox.Show(text: $"既に監視中です。"
                    , caption: "AoEDEAlarm"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;

            }

            if (Check()==false) {
                MessageBox.Show(text: $"位置設定が行われていません。"
                    , caption: "AoEDEAlarm"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;
            }

            AoedeGlobal.IsRunning = true;

            //string zz = Win32Api.GetActiveWindowProductName();
            //Debug.WriteLine(zz);
            //Console.WriteLine(zz);
            //Util.Xxx();
            /*
            プロセス名: AoEDE
            ID: 21444
            ファイル名: C:\Program Files\WindowsApps\Microsoft.Darwin_100.1.34483.0_x64__8wekyb3d8bbwe\AoEDE.exe
            合計プロセッサ時間: 00:04:25.8125000
            物理メモリ使用量: 1801117696
            */
            //Application.

            //if (Win32Api.GetActiveWindowProductName() != "AoEDE") return;

            AoedeGlobal.tokenSource = new CancellationTokenSource();
            CancellationToken token = AoedeGlobal.tokenSource.Token;

            using (AoEDEAlarm a = new AoEDEAlarm()) {

                Task.Run(async () => {
                    //Global.IsRunning = true;
                    bool rtn = await a.Run(token: token);
                    //最前面
                    MessageBox.Show(text: $"中断しました。"
                        , caption: "AoEDEAlarm"
                        , buttons: MessageBoxButtons.OK
                        , icon: MessageBoxIcon.Information
                        , defaultButton: MessageBoxDefaultButton.Button1
                        , options: MessageBoxOptions.DefaultDesktopOnly
                        );
                    AoedeGlobal.IsRunning = false;
                }, token);

                //最前面
                MessageBox.Show(text: $"アラーム処理を開始しました。"
                    , caption: "AoEDEAlarm"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Information
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );

            }

        }

        private static bool Check() {
            if (AoedeGlobal.Settings.Population.Height == 0) return false;
            if (AoedeGlobal.Settings.Population.Width == 0) return false;
            return true;
        }

        /// <summary>
        /// 監視処理中断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hk_Stop_HotkeyEvent(object sender, EventArgs e) {
            //if (Win32Api.GetActiveWindowProductName() != "AoEDEAlarm") return;
            AoedeGlobal.tokenSource.Cancel();


        }

        /// <summary>
        /// 設定画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hk_Setting_HotkeyEvent(object sender, EventArgs e) {

            if (AoedeGlobal.IsRunning) {
                MessageBox.Show(text: $"監視処理を終了する必要があります。"
                    , caption: "AoEDEAlarm"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;

            }

            //if (Win32Api.GetActiveWindowProductName() != "AoEDEAlarm") return;
            PositionSettingForm f = new PositionSettingForm();
            f.TopMost = true;
            f.Show();


        }

        private static void Item1_Click(object sender, EventArgs e) {

            try {
                AoedeGlobal.tokenSource.Cancel();
            } catch (Exception) {
            }

            //最前面
            MessageBox.Show(text: $"アプリケーションを終了します。"
                , caption: "AoEDEAlarm"
                , buttons: MessageBoxButtons.OK
                , icon: MessageBoxIcon.Information
                , defaultButton: MessageBoxDefaultButton.Button1
                , options: MessageBoxOptions.DefaultDesktopOnly
                );

            Application.Exit();

        }

        private static void Item2_Click(object sender, EventArgs e) {
            if (AoedeGlobal.IsRunning) {
                MessageBox.Show(text: $"監視処理を終了する必要があります。"
                    , caption: "AoEDEAlarm"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;

            }

            FinishHotkeys();
            HotkeySettingForm f = new HotkeySettingForm();
            f.TopMost = true;
            f.ShowDialog();
            StartHotkeys();

        }

        private static void Item3_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("file://" + AoedeGlobal.HelpFileName);
        }



    }
}
