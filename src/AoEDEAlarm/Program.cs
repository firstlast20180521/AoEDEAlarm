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
                    GlobalValues.NotWorkingImageArray = GetNotWorkingImageArray(GlobalValues.ApplicationSetting.UiScale);


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

                } finally {
                    if (hasHandle) {
                        mutex.ReleaseMutex();
                    }
                    mutex.Close();

                }
            }


        }

        public static void StartHotkeys() {
            GlobalValues.Hotkey_Run = new HotKey((Keys)GlobalValues.ApplicationSetting.Hotkey_Run);
            GlobalValues.Hotkey_Stop = new HotKey((Keys)GlobalValues.ApplicationSetting.Hotkey_Stop);
            GlobalValues.Hotkey_Customise = new HotKey((Keys)GlobalValues.ApplicationSetting.Hotkey_Customise);

            GlobalValues.Hotkey_Run.HotkeyEvent += Hotkey_Run_HotkeyEvent;
            GlobalValues.Hotkey_Stop.HotkeyEvent += Hotkey_Stop_HotkeyEvent;
            GlobalValues.Hotkey_Customise.HotkeyEvent += Hotkey_Customise_HotkeyEvent;

        }

        public static void FinishHotkeys() {
            GlobalValues.Hotkey_Run.Dispose();
            GlobalValues.Hotkey_Stop.Dispose();
            GlobalValues.Hotkey_Customise.Dispose();
        }

        /// <summary>
        /// ホットキー押下時処理（監視処理開始）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hotkey_Run_HotkeyEvent(object sender, EventArgs e) {
            StartMonitoring();
        }

        /// <summary>
        /// ホットキー押下時処理（監視処理中断）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hotkey_Stop_HotkeyEvent(object sender, EventArgs e) {
            StopMonitoring();
        }

        /// <summary>
        /// ホットキー押下時処理（設定画面表示）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hotkey_Customise_HotkeyEvent(object sender, EventArgs e) {
            ShowPositionSettingForm();
        }

        /// <summary>
        /// 右クリックメニュー（監視開始）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TsmItemFinishMonitoring_Click(object sender, EventArgs e) {
            StopMonitoring();
        }

        /// <summary>
        /// 右クリックメニュー（監視終了）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TsmItemStartMonitoring_Click(object sender, EventArgs e) {
            StartMonitoring();
        }

        /// <summary>
        /// 右クリックメニュー（設定画面表示）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TsmItemHotkeySetting_Click(object sender, EventArgs e) {
            ShowHotkeySettingForm();
        }

        /// <summary>
        /// 右クリックメニュー（ヘルプ画面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TsmItemHelp_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("file://" + ConstValues.HelpFileName);
        }

        /// <summary>
        /// 右クリックメニュー（アプリケーション終了）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TsmItemExitApplication_Click(object sender, EventArgs e) {

            try {
                GlobalValues.AlarmTokenSource?.Cancel();
            } catch (Exception) {
            }

            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static NotifyIcon SetNotifyIcon() {
            NotifyIcon ni;

            ni = new NotifyIcon();
            ni.Icon = Properties.Resources.Leaf32;
            ni.Text = "AoEDEAlarm";
            ni.Visible = true;

            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem TsmItemStartMonitoring = new ToolStripMenuItem() {
                Text = "監視開始",
                Image = null,
                Name = "監視開始ToolStripMenuItem",
            };

            ToolStripMenuItem TsmItemFinishMonitoring = new ToolStripMenuItem() {
                Text = "監視終了",
                Image = null,
                Name = "監視終了ToolStripMenuItem",
            };

            ToolStripMenuItem TsmItemShowHotkeySettingForm = new ToolStripMenuItem() {
                Text = "ホットキー設定",
                Image = null,
                Name = "ホットキー設定ToolStripMenuItem",
            };

            ToolStripMenuItem TsmItemHelp = new ToolStripMenuItem() {
                Text = "ヘルプ",
                Image = null,
                Name = "ヘルプToolStripMenuItem",
            };

            ToolStripMenuItem TsmItemExitApplication = new ToolStripMenuItem() {
                Text = "終了",
                Image = null,
                Name = "終了ToolStripMenuItem",
            };

            cms.Items.Add(TsmItemStartMonitoring);
            cms.Items.Add(TsmItemFinishMonitoring);
            cms.Items.Add(TsmItemShowHotkeySettingForm);
            cms.Items.Add(TsmItemHelp);
            cms.Items.Add(TsmItemExitApplication);
            ni.ContextMenuStrip = cms;

            TsmItemStartMonitoring.Click += TsmItemStartMonitoring_Click;
            TsmItemFinishMonitoring.Click += TsmItemFinishMonitoring_Click;
            TsmItemShowHotkeySettingForm.Click += TsmItemHotkeySetting_Click;
            TsmItemHelp.Click += TsmItemHelp_Click;
            TsmItemExitApplication.Click += TsmItemExitApplication_Click;

            return ni;
        }

        public static void ShowHotkeySettingForm() {
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

            /*
            string zz = Win32Api.GetActiveWindowProductName();
            Console.WriteLine(zz);
            //プロセス名: AoEDE
            //ID: 21444
            //ファイル名: C:\Program Files\WindowsApps\Microsoft.Darwin_100.1.34483.0_x64__8wekyb3d8bbwe\AoEDE.exe
            //合計プロセッサ時間: 00:04:25.8125000
            //物理メモリ使用量: 1801117696
            if (Win32Api.GetActiveWindowProductName() != "AoEDE") return;
            //if (Win32Api.GetActiveWindowProductName() != "AoEDEAlarm") return;
            */

            GlobalValues.AlarmTokenSource = new CancellationTokenSource();
            CancellationToken token = GlobalValues.AlarmTokenSource.Token;

            using (AoEDEAlarm a = new AoEDEAlarm()) {
                Task.Run(async () => {
                    bool rtn = await a.Run(token: token);
                    GlobalValues.IsRunning = false;
                }, token);
                GlobalValues.Console.Add("監視処理を開始しました。");
            }
        }

        private static bool Check() {
            if (GlobalValues.ApplicationSetting.Housing.Height == 0) return false;
            if (GlobalValues.ApplicationSetting.Housing.Width == 0) return false;
            return true;
        }

        public static void StopMonitoring() {
            GlobalValues.AlarmTokenSource?.Cancel();
            GlobalValues.Console.Add("監視処理を中断します。");
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

            PositionSettingForm f = new PositionSettingForm();
            f.TopMost = true;
            f.Show();

        }

        /// <summary>
        /// 遊休農民数画像マッチング用
        /// </summary>
        /// <param name="uiScale"></param>
        /// <returns></returns>
        public static Mat[] GetNotWorkingImageArray(int uiScale) {

            double double_scale= (double)uiScale / 100d;
            string[] files = System.IO.Directory.GetFiles(ConstValues.NotWorkingImagePath, "*", System.IO.SearchOption.TopDirectoryOnly);
            Mat[] notWorkingImageArray = new Mat[files.Length];
            for (int i = 0; i < files.Length; i++) {
                using (Mat mat_original = new Mat(files[i], ImreadModes.Unchanged)) {
                    using (Mat mat_rotation = Cv2.GetRotationMatrix2D(new Point2f(0, 0), 0d, double_scale)) {
                        Mat mat_affined = new Mat();
                        Cv2.WarpAffine(mat_original, mat_affined, mat_rotation, new OpenCvSharp.Size((double)mat_original.Width * double_scale, (double)mat_original.Height * double_scale));
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

        /// <summary>
        /// ＵＩスケール取得用
        /// </summary>
        /// <returns></returns>
        public static int GetScale() {

            double max_value = 0;
            int ret_i = 100;

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
                                        //max_scalse = scale;
                                        ret_i = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ret_i;
        }

        /// <summary>
        /// 半透明フォーム上の右クリックメニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Console_ContextMenuClick(object sender, TransparentMessage.ContextMenuClickedEventArgs e) {
            if (e.contextMenuItem == TransparentMessage.ContextMenuItems.MiscellaneousSettings) {
                MiscellaneousSettingsForm f = new MiscellaneousSettingsForm();
                f.ShowDialog();

            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.StartMonitoring) {
                Program.StartMonitoring();

            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.StopMonitoring) {
                Program.StopMonitoring();

            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.ShowPositionSettingForm) {
                Program.ShowPositionSettingForm();

            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.HotkeySetting) {
                Program.ShowHotkeySettingForm();

            } else if (e.contextMenuItem == TransparentMessage.ContextMenuItems.ExitProgram) {
                GlobalValues.Console.Finish();

            }
        }
    }
}
