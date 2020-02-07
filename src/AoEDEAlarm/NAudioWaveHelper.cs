using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoEDEAlarm {
    public sealed class NAudioWaveHelper :IDisposable{

        public string FileName { get; set; }
        public float Volume { get; set; }
        public WaveOut Wave { get; set; }
        public WaveFileReader Reader { get; set; }

        public NAudioWaveHelper(string file, float volume) {
            Console.WriteLine($"--->{file}<---");
            FileName = file;
            Volume = volume;
            Reader = new NAudio.Wave.WaveFileReader(new FileStream(file, FileMode.Open,FileAccess.Read));
        }

        public void Play() {
            try {
                Wave = new NAudio.Wave.WaveOut();
                Reader.Position = 0;
                Wave.Init(Reader);
                Wave.Volume = Volume;
                Wave.Play();

            } catch (Exception ex) {
                Console.WriteLine($"{ex.Message}");
            }

        }

        public void Dispose() {
            ((IDisposable)Wave).Dispose();
            ((IDisposable)Reader).Dispose();
        }

    }
}
