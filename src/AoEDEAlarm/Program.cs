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
using OpenCvSharp.Extensions;
using OpenCvSharp;
using System.IO;
using System.Drawing.Imaging;

namespace AoEDEAlarm {
    class Program {

        //public static GlobalValues _g;
        public List<string> glist = new List<string>();

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

                    GlobalValues.ApplicationSetting = XmlUtilityClass<ApplicationSettingClass>.LoadXml(ConstValues.ApplicationSettingFileName);
                    GlobalValues.SoundSetting = XmlUtilityClass<SoundSettingClass>.LoadXml(ConstValues.SoundSettingFileName);

                    StartHotkeys();
                    NotifyIcon ni = SetNotifyIcon();

                    GlobalValues.Console.ContextMenuClick += Console_ContextMenuClick; ;

                    GlobalValues.IsRunning = false;
                    //AlarmMessageList = new List<AlarmMessageClass>();
                    GlobalValues.Scale = GetScale();
                    GlobalValues.NotWorkingImageArray = GetNotWorkingImageArray(GlobalValues.Scale);


                    //初期メッセージ
                    GlobalValues.Console.Add($"監視開始：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Run)}");
                    GlobalValues.Console.Add($"監視中断：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Stop)}");
                    GlobalValues.Console.Add($"位置設定：{HotKey.GetKeysString((Keys)GlobalValues.ApplicationSetting.Hotkey_Customise)}");
                    GlobalValues.Console.Show();
                    GlobalValues.Console.Start();

                    System.Windows.Forms.Application.Run();

                    ni.Dispose();
                    XmlUtilityClass<ApplicationSettingClass>.SaveXml(GlobalValues.ApplicationSetting, ConstValues.ApplicationSettingFileName);
                    XmlUtilityClass<SoundSettingClass>.SaveXml(GlobalValues.SoundSetting, ConstValues.SoundSettingFileName);

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

        private static void Console_ContextMenuClick(object sender, TransparentMessage.ContextMenuClickedEventArgs e) {
            if (e.contextMenuItem == TransparentMessage.ContextMenuItems.AdjustVolume) {
                VolumeAdjustForm f = new VolumeAdjustForm();
                f.ShowDialog();
            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.StartMonitoring) {
                Program.StartMonitoring();
            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.StopMonitoring) {
                Program.StopMonitoring();
            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.ShowPositionSettingForm) {
                Program.ShowPositionSettingForm();
            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.ExitProgram) {
                GlobalValues.Console.Finish();
            }
        }

        private static NotifyIcon SetNotifyIcon() {
            NotifyIcon ni;

            ni = new NotifyIcon();
            ni.Icon = Properties.Resources.Leaf32;
            ni.Text = "AoEDEAlarm";
            ni.Visible = true;

            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem item_start_monitoring = new ToolStripMenuItem() {
                Text = "監視開始",
                Image = null,
                Name = "監視開始ToolStripMenuItem",
            };
            item_start_monitoring.Click += item_start_monitoring_Click;

            ToolStripMenuItem item_finish_monitoring = new ToolStripMenuItem() {
                Text = "監視終了",
                Image = null,
                Name = "監視終了ToolStripMenuItem",
            };
            item_finish_monitoring.Click += Item_finish_monitoring_Click;

            ToolStripMenuItem item_hotkey_setting = new ToolStripMenuItem() {
                Text = "ホットキー設定",
                Image = null,
                Name = "ホットキー設定ToolStripMenuItem",
            };
            item_hotkey_setting.Click += Item2_Click;

            ToolStripMenuItem item_help = new ToolStripMenuItem() {
                Text = "ヘルプ",
                Image = null,
                Name = "ヘルプToolStripMenuItem",
            };
            item_help.Click += Item3_Click;

            ToolStripMenuItem item_exit_application = new ToolStripMenuItem() {
                Text = "終了",
                Image = null,
                Name = "終了ToolStripMenuItem",
            };
            item_exit_application.Click += Item1_Click;

            cms.Items.Add(item_start_monitoring);
            cms.Items.Add(item_finish_monitoring);
            cms.Items.Add(item_hotkey_setting);
            cms.Items.Add(item_help);
            cms.Items.Add(item_exit_application);
            ni.ContextMenuStrip = cms;

            return ni;

        }

        private static void Item_finish_monitoring_Click(object sender, EventArgs e) {
            Hk_Stop_HotkeyEvent(sender, e);
        }

        private static void item_start_monitoring_Click(object sender, EventArgs e) {
            Hk_Run_HotkeyEvent(sender, e);
        }

        private static void StartHotkeys() {
            GlobalValues.Hotkey_Run = new HotKey((Keys)GlobalValues.ApplicationSetting.Hotkey_Run);
            GlobalValues.Hotkey_Stop = new HotKey((Keys)GlobalValues.ApplicationSetting.Hotkey_Stop);
            GlobalValues.Hotkey_Customise = new HotKey((Keys)GlobalValues.ApplicationSetting.Hotkey_Customise);

            GlobalValues.Hotkey_Run.HotkeyEvent += Hk_Run_HotkeyEvent;
            GlobalValues.Hotkey_Stop.HotkeyEvent += Hk_Stop_HotkeyEvent;
            GlobalValues.Hotkey_Customise.HotkeyEvent += Hk_Setting_HotkeyEvent;

        }

        private static void FinishHotkeys() {
            GlobalValues.Hotkey_Run.Dispose();
            GlobalValues.Hotkey_Stop.Dispose();
            GlobalValues.Hotkey_Customise.Dispose();
        }

        /// <summary>
        /// 監視処理開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hk_Run_HotkeyEvent(object sender, EventArgs e) {
            StartMonitoring();
        }

        public static void StartMonitoring() {
            if (GlobalValues.IsRunning) {
                MessageBox.Show(text: $"既に監視中です。"
                    , caption: "AoEDEAlarm"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;

            }

            if (Check() == false) {
                MessageBox.Show(text: $"位置設定が行われていません。"
                    , caption: "AoEDEAlarm"
                    , buttons: MessageBoxButtons.OK
                    , icon: MessageBoxIcon.Error
                    , defaultButton: MessageBoxDefaultButton.Button1
                    , options: MessageBoxOptions.DefaultDesktopOnly
                    );
                return;
            }

            GlobalValues.IsRunning = true;

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

            GlobalValues.AlarmTokenSource = new CancellationTokenSource();
            CancellationToken token = GlobalValues.AlarmTokenSource.Token;

            using (AoEDEAlarm a = new AoEDEAlarm()) {

                Task.Run(async () => {
                    //Global.IsRunning = true;
                    bool rtn = await a.Run(token: token);
                    //最前面
                    //MessageBox.Show(text: $"監視処理を中断しました。"
                    //    , caption: "AoEDEAlarm"
                    //    , buttons: MessageBoxButtons.OK
                    //    , icon: MessageBoxIcon.Information
                    //    , defaultButton: MessageBoxDefaultButton.Button1
                    //    , options: MessageBoxOptions.DefaultDesktopOnly
                    //    );
                    GlobalValues.IsRunning = false;
                    //AoedeGlobal.Console.Start("監視処理を中断しました。");
                }, token);

                //最前面
                //GlobalValues.AddMessage("監視処理を開始しました。");
                GlobalValues.Console.Add("監視処理を開始しました。");

                //MessageBox.Show(text: $"アラーム処理を開始しました。"
                //    , caption: "AoEDEAlarm"
                //    , buttons: MessageBoxButtons.OK
                //    , icon: MessageBoxIcon.Information
                //    , defaultButton: MessageBoxDefaultButton.Button1
                //    , options: MessageBoxOptions.DefaultDesktopOnly
                //    );

            }
        }

        private static bool Check() {
            if (GlobalValues.ApplicationSetting.Housing.Height == 0) return false;
            if (GlobalValues.ApplicationSetting.Housing.Width == 0) return false;
            return true;
        }

        /// <summary>
        /// 監視処理中断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hk_Stop_HotkeyEvent(object sender, EventArgs e) {
            StopMonitoring();

        }

        public static void StopMonitoring() {
            GlobalValues.AlarmTokenSource?.Cancel();
            //GlobalValues.AddMessage("監視処理を中断します。");
            GlobalValues.Console.Add("監視処理を中断します。");
        }

        /// <summary>
        /// 設定画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hk_Setting_HotkeyEvent(object sender, EventArgs e) {
            ShowPositionSettingForm();
        }

        public static void ShowPositionSettingForm() {
            if (GlobalValues.IsRunning) {
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
                GlobalValues.AlarmTokenSource?.Cancel();
            } catch (Exception) {
            }

            ////最前面
            //MessageBox.Show(text: $"アプリケーションを終了します。"
            //    , caption: "AoEDEAlarm"
            //    , buttons: MessageBoxButtons.OK
            //    , icon: MessageBoxIcon.Information
            //    , defaultButton: MessageBoxDefaultButton.Button1
            //    , options: MessageBoxOptions.DefaultDesktopOnly
            //    );

            Application.Exit();

        }

        private static void Item2_Click(object sender, EventArgs e) {
            if (GlobalValues.IsRunning) {
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
            System.Diagnostics.Process.Start("file://" + ConstValues.HelpFileName);
        }

        private static Mat[] GetNotWorkingImageArray(double scale) {
            string[] files = System.IO.Directory.GetFiles(ConstValues.NotWorkingImagePath, "*", System.IO.SearchOption.TopDirectoryOnly);
            Mat[] notWorkingImageArray = new Mat[files.Length];
            for (int i = 0; i < files.Length; i++) {
                using (Mat mat_original = new Mat(files[i], ImreadModes.Unchanged)) {
                    using (Mat mat_rotation = Cv2.GetRotationMatrix2D(new Point2f(0, 0), 0d, scale)) {
                        //using (Mat mat_affined = new Mat()) {
                        //    Cv2.WarpAffine(mat_original, mat_affined, mat_rotation, new OpenCvSharp.Size((double)mat_original.Width * scale, (double)mat_original.Height * scale));
                        //    int idx;
                        //    bool rtn = int.TryParse(Path.GetFileNameWithoutExtension(files[i]), out idx);
                        //    if (rtn) {
                        //        NotWorkingImageArray[idx] = mat_affined;
                        //    }
                        //}

                        Mat mat_affined = new Mat();
                        Cv2.WarpAffine(mat_original, mat_affined, mat_rotation, new OpenCvSharp.Size((double)mat_original.Width * scale, (double)mat_original.Height * scale));
                        int idx;
                        bool rtn = int.TryParse(Path.GetFileNameWithoutExtension(files[i]), out idx);
                        if (rtn) {
                            notWorkingImageArray[idx] = mat_affined;
                        }
                    }
                }
            }
            return notWorkingImageArray;
        }

        private static double GetScale() {

            double max_value = 0;
            double max_scalse = 0;

            Rectangle rect = Screen.PrimaryScreen.Bounds;
            using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb)) {
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
                using (Mat mat_full_screen = BitmapConverter.ToMat(bmp)) {
                    using (Mat mat_stone_age = new Mat(ConstValues.RulerImageFileName, ImreadModes.Unchanged)) {

                        for (int i = 75; i <= 150; i++) {

                            double scale = ((double)i / (double)100);

                            using (Mat mat_rotation = Cv2.GetRotationMatrix2D(new Point2f(0, 0), 0d, scale)) {
                                using (Mat mat_affined = new Mat()) {
                                    Cv2.WarpAffine(mat_stone_age, mat_affined, mat_rotation, new OpenCvSharp.Size((double)mat_stone_age.Width * scale, (double)mat_stone_age.Height * scale));

                                    double v = OpenCvUtil.FindImage(mat_full_screen, mat_affined, 0.7d);
                                    if (v > max_value) {
                                        max_value = v;
                                        max_scalse = scale;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return max_scalse;
        }


    }
}
