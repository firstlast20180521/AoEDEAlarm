using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AoEDEAlarm {
    public partial class HotkeySettingForm : Form {

        Keys[] KeysArr;
        bool IsEditting;
        //CancellationTokenSource _tokenSource = new CancellationTokenSource();
        CancellationTokenSource _tokenSource;

        public HotkeySettingForm() {
            InitializeComponent();
            KeysArr = new Keys[3];
        }

        private void HotkeySettingForm_Load(object sender, EventArgs e) {

            KeysArr = new Keys[3];
            KeysArr[0] = (Keys)AoedeGlobal.Settings.Hotkey_Run;
            KeysArr[1] = (Keys)AoedeGlobal.Settings.Hotkey_Stop;
            KeysArr[2] = (Keys)AoedeGlobal.Settings.Hotkey_Customise;

            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            // カラム数を指定
            dataGridView1.ColumnCount = 2;

            // カラム名を指定
            dataGridView1.Columns[0].HeaderText = "処理内容";
            dataGridView1.Columns[1].HeaderText = "ホットキー";

            // データを追加
            dataGridView1.Rows.Add("監視開始", GetKeysString(KeysArr[0]));
            dataGridView1.Rows.Add("監視終了", GetKeysString(KeysArr[1]));
            dataGridView1.Rows.Add("画像位置設定", GetKeysString(KeysArr[2]));

            //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Ivory;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Rows[0].Selected = true;
            dataGridView1.Focus();

        }

        private void HotkeySettingForm_KeyDown(object sender, KeyEventArgs e) {

            if (this.ActiveControl.Name == dataGridView1.Name) {

                int i = dataGridView1.CurrentCell.RowIndex;
                int j = dataGridView1.CurrentCell.ColumnIndex;
                if (j != 1) return;

                if (e.KeyCode.Equals("")) return;
                if (e.KeyCode == Keys.ControlKey) return;
                if (e.KeyCode == Keys.ShiftKey) return;
                if (e.KeyCode == Keys.Alt) return;
                if (e.KeyCode == Keys.Menu) return;
                if (e.KeyCode == Keys.Packet) return;
                if (e.KeyData == Keys.Escape) {
                    FinishEdit(i, j);
                    return;
                }

                if (IsEditting) {
                    string x = GetKeysString(e.KeyCode | e.Modifiers);
                    dataGridView1.Rows[i].Cells[1].Value = x;
                    KeysArr[i] = e.KeyCode | e.Modifiers;
                    FinishEdit(i,j);

                } else {
                    if (e.KeyData == (Keys.Control | Keys.Enter)) {
                        StartEdit(i, j);
                    }
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e) {
            int i = dataGridView1.CurrentCell.RowIndex;
            int j = dataGridView1.CurrentCell.ColumnIndex;
            if (j != 1) return;
            StartEdit(i, j);
        }

        private void FinishEdit(int i, int j) {
            if (_tokenSource != null) _tokenSource.Cancel();
            dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = dataGridView1.DefaultCellStyle.SelectionBackColor;
            IsEditting = false;

        }

        private void StartEdit(int i, int j) {
            for (int x = 0; x < dataGridView1.Rows.Count; x++) {
                dataGridView1.Rows[x].Cells[j].Style.SelectionBackColor = dataGridView1.DefaultCellStyle.SelectionBackColor;
            }
            StartBlink(i, j);
            IsEditting = true;
        }

        private void StartBlink(int i, int j) {
            _tokenSource = new CancellationTokenSource();
            CancellationToken token = _tokenSource.Token;
            bool flag = false;
            Task.Run(async () => {
                while (true) {
                    if (token.IsCancellationRequested) break;
                    if (flag) {
                        dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightBlue;
                    } else {
                        dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = Color.SkyBlue;
                    }
                    flag = !flag;
                    await Task.Delay(300);
                }
            }, token);

            //bool rtn = await a.Run(token: token);

        }

        private void button1_Click(object sender, EventArgs e) {

            AoedeGlobal.Settings.Hotkey_Run = (int)KeysArr[0];
            AoedeGlobal.Settings.Hotkey_Stop = (int)KeysArr[1];
            AoedeGlobal.Settings.Hotkey_Customise = (int)KeysArr[2];

            AoEDEAlarmSettings.SaveXml(AoedeGlobal.Settings);

            MessageBox.Show(text: "位置を保存しました。"
                , caption: "画像位置設定"
                , buttons: MessageBoxButtons.OK
                , icon: MessageBoxIcon.Information
                , defaultButton: MessageBoxDefaultButton.Button1
                , options: MessageBoxOptions.DefaultDesktopOnly
                );

            this.Close();

        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();

        }

        private void HotkeySettingForm_FormClosing(object sender, FormClosingEventArgs e) {

        }

        private string GetKeysString(Keys keys) {
            StringBuilder sb = new StringBuilder();
            //Keys k = 0;

            if ((keys & Keys.Alt) == Keys.Alt) {
                sb.Append("ALT");
                //k |= Keys.Alt;
            }

            if ((keys & Keys.Control) == Keys.Control) {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append("CONTROL");
                //k |= Keys.Control;
            }

            if ((keys & Keys.Shift) == Keys.Shift) {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append("SHIFT");
                //k |= Keys.Shift;
            }

            Keys k = keys & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;

            if (k != 0) {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append(k);
                //k |= e.KeyCode;

                //Keys k = x & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
                //Keys k = e.KeyCode;
            }
            return sb.ToString();
        }

        private void button3_Click(object sender, EventArgs e) {
            int i = dataGridView1.CurrentCell.RowIndex;
            int j = dataGridView1.CurrentCell.ColumnIndex;
            if (j != 1) return;
            StartEdit(i, j);

        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e) {
            int i = e.RowIndex;
            int j = e.ColumnIndex;
            if (j != 1) return;
            FinishEdit(i, j);
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e) {

        }
    }
}
