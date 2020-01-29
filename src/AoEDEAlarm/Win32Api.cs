using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AoEDEAlarm {
    class Win32Api {

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public static string GetActiveWindowProductName() {
            int processid;
            string r = "";

            try {
                GetWindowThreadProcessId(GetForegroundWindow(), out processid);
                if (0 != processid) {
                    Process p = Process.GetProcessById(processid);
                    //System.Diagnostics.Debug.Write(processid + ":");
                    //System.Diagnostics.Debug.WriteLine(p.MainModule.FileVersionInfo.ProductName);//アプリケーションの名前が出てきます
                    r = p.MainModule.FileVersionInfo.ProductName;
                }
            } catch (System.ComponentModel.Win32Exception e) {
            }

            return r;


        }

    }
}
