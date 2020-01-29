using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AoEDEAlarm {
    public static class Util {

        public static void Xxx() {
            //ローカルコンピュータ上で実行されているすべてのプロセスを取得
            System.Diagnostics.Process[] ps =
                System.Diagnostics.Process.GetProcesses();
            //"machinename"という名前のコンピュータで実行されている
            //すべてのプロセスを取得するには次のようにする。
            //System.Diagnostics.Process[] ps =
            //    System.Diagnostics.Process.GetProcesses("machinename");

            //配列から1つずつ取り出す
            foreach (System.Diagnostics.Process p in ps) {
                try {
                    //プロセス名を出力する
                    Console.WriteLine("プロセス名: {0}", p.ProcessName);
                    //ID
                    Console.WriteLine("ID: {0}", p.Id);
                    //メインモジュールのパス
                    Console.WriteLine("ファイル名: {0}", p.MainModule.FileName);
                    //合計プロセッサ時間
                    Console.WriteLine("合計プロセッサ時間: {0}", p.TotalProcessorTime);
                    //物理メモリ使用量
                    Console.WriteLine("物理メモリ使用量: {0}", p.WorkingSet64);
                    //.NET Framework 1.1以前では次のようにする
                    //Console.WriteLine("物理メモリ使用量: {0}", p.WorkingSet);

                    Console.WriteLine();
                } catch (Exception ex) {
                    Console.WriteLine("エラー: {0}", ex.Message);
                }
            }

        }


    }
}
