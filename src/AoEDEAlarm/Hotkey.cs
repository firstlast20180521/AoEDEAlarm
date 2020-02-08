using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public sealed class HotKey : IDisposable {
        private const int MOD_ALT = 0x01;
        private const int MOD_CONTROL = 0x02;
        private const int MOD_SHIFT = 0x04;
        private const int WM_HOTKEY = 0x312;

        [DllImport("user32")]
        static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, Keys vk);

        [DllImport("user32")]
        static extern int UnregisterHotKey(IntPtr hwnd, int id);

        [DllImport("kernel32", EntryPoint = "GlobalAddAtomA")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", Justification = "<•Û—¯’†>")]
        static extern short GlobalAddAtom(string lpString);

        [DllImport("kernel32")]
        static extern short GlobalDeleteAtom(short nAtom);

        private HotkeyForm hWnd = null;

        public int ID { get; private set; }
        public Keys Keys { get; private set; }
        public bool Enabled { get; private set; }
        public event EventHandler HotkeyEvent;

        public HotKey() { }
        public HotKey(Keys key) {
            this.Keys = key;

            Register();
        }
        ~HotKey() {
            this.Dispose();
        }
        public void Dispose() {
            this.Unregister();
        }

        public static int GetHotkeyValue(bool altFlg, bool ctrlFlg, bool shiftFlg) {
            int x = 0;
            if (altFlg) x += MOD_ALT;
            if (ctrlFlg) x += MOD_CONTROL;
            if (shiftFlg) x += MOD_SHIFT;
            return x;
        }

        private void Register() {

            if (hWnd == null) {
                hWnd = new HotkeyForm();
                hWnd.CreateHandle(new CreateParams());
            }

            if (!this.Enabled) {

                int modifiers = 0;

                if ((this.Keys & Keys.Alt) == Keys.Alt)
                    modifiers = modifiers | MOD_ALT;
                if ((this.Keys & Keys.Control) == Keys.Control)
                    modifiers = modifiers | MOD_CONTROL;
                if ((this.Keys & Keys.Shift) == Keys.Shift)
                    modifiers = modifiers | MOD_SHIFT;
                Keys k = this.Keys & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;

                this.ID = (new Random()).Next(0x1000, 0xbfff);

                RegisterHotKey(hWnd.Handle, this.ID, modifiers, k);
                this.Enabled = true;

                hWnd.Handler += this.hWndHotkeyEvent;
            }
        }
        private void Unregister() {
            if (this.Enabled) {
                UnregisterHotKey(hWnd.Handle, this.ID);
                this.ID = 0;
                this.Enabled = false;

                hWnd.Handler -= this.hWndHotkeyEvent;
            }
        }

        private void hWndHotkeyEvent(int id) {
            if (this.ID == id && HotkeyEvent != null) {
                HotkeyEvent(this, EventArgs.Empty);
            }
        }

        internal class HotkeyForm : NativeWindow {
            internal HotkeyForm() {
            }

            internal delegate void HotkeyHandler(int id);
            internal event HotkeyHandler Handler;

            protected override void WndProc(ref Message m) {
                if (m.Msg == (int)WM_HOTKEY) {
                    if (Handler != null)
                        Handler((int)m.WParam);
                }

                base.WndProc(ref m);
            }
        }

        public static string GetKeysString(Keys keys) {
            StringBuilder sb = new StringBuilder();

            if ((keys & Keys.Alt) == Keys.Alt) {
                sb.Append("ALT");
            }

            if ((keys & Keys.Control) == Keys.Control) {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append("CTRL");
            }

            if ((keys & Keys.Shift) == Keys.Shift) {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append("SHIFT");
            }

            Keys k = keys & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;

            if (k != 0) {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append(k);
            }
            return sb.ToString();
        }

    }
}
