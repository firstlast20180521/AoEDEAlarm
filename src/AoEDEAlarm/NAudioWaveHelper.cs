using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoEDEAlarm {
    //public class NAudioWaveHelper : IDisposable {
    public static class NAudioWaveHelper {
        //public NAudio.Wave.WaveOut Wave { get; set; }
        //public WaveFileReader Reader { get; set; }
        //public string FileName { get; set; }
        //public float Volume { get; set; }

        //public NAudioWaveHelper() {

        //}

        //public NAudioWaveHelper(string file, float volume) {
        //    FileName = file;
        //    Volume = volume;

        //    Wave = new NAudio.Wave.WaveOut();

        //    WaveFileReader Reader = new NAudio.Wave.WaveFileReader(file);
        //    Wave.Volume = volume;

        //}

        //public void Play() {
        //    try {
        //        Reader.Position = 0;
        //        Wave.Init(Reader);
        //        Wave.Play();

        //    } catch (Exception ex) {
        //        Console.WriteLine($"{ex.Message}");
        //    }

        //}

        public static void Play_Sound_x(string file, float volume) {
            NAudio.Wave.WaveOut wave = new NAudio.Wave.WaveOut();
            string[] x = file.Split(new char[] { '.' });
            if (x.Last() == "aiff") {
                wave.Init(new NAudio.Wave.AiffFileReader(file));
            } else if (x.Last() == "mp3") {
                wave.Init(new NAudio.Wave.Mp3FileReader(file));
            } else if (x.Last() == "wav") {
                wave.Init(new NAudio.Wave.WaveFileReader(file));
            } else {
                wave.Init(
                    new NAudio.Wave.BlockAlignReductionStream(
                            NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(
                                new NAudio.Wave.AudioFileReader(file)
                            )
                        )
                    );
            }

            try {
                wave.Volume = volume;
                wave.Play();

            } catch (Exception ex) {
                Console.WriteLine($"{ex.Message}");
            }

        }

        //public void Dispose() {
        //    Wave.Dispose();
        //    this.Dispose();
        //}
    }
}
