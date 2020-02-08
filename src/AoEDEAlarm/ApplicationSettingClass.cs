using System.Windows.Forms;

namespace AoEDEAlarm {
    public class ApplicationSettingClass {
        public int CheckInterval { get; set; }
        public int MasterVolume { get; set; }

        public int Hotkey_Run { get; set; }
        public int Hotkey_Stop { get; set; }
        public int Hotkey_Customise { get; set; }

        public Rectangle Wood { get; set; }
        public Rectangle Food { get; set; }
        public Rectangle Gold { get; set; }
        public Rectangle Stone { get; set; }
        public Rectangle Housing { get; set; }
        public Rectangle NotWorking { get; set; }
        public Rectangle Players { get; set; }
        public Rectangle MiniMap { get; set; }

        public class Rectangle {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public ApplicationSettingClass() {
            CheckInterval = 3000;
            MasterVolume = 100;
            Hotkey_Run = (int)(Keys.R | Keys.Control | Keys.Shift);
            Hotkey_Stop = (int)(Keys.S | Keys.Control | Keys.Shift);
            Hotkey_Customise = (int)(Keys.C | Keys.Control | Keys.Shift);
            Wood = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };
            Food = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };
            Gold = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };
            Stone = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };
            Housing = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };
            NotWorking = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };
            Players = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };
            MiniMap = new ApplicationSettingClass.Rectangle { X = 0, Y = 0, Width = 0, Height = 0, };

        }

    }
}
