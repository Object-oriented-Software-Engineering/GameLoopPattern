using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLoopPattern {

    public class TimeElapsedEventArgs : EventArgs {
        public double TimeElapsed { get; set; }
        public double TotalTime { get; set; }
        public uint Frame { get; set; }
    }

    public delegate void TimeElapsedEventHandler(object sender, TimeElapsedEventArgs e);

    internal class CGameLoop {
        public event TimeElapsedEventHandler Update;
        public event TimeElapsedEventHandler Draw;
        List<CGameObject> gameObjects = new List<CGameObject>();
        private uint _frame;
        public List<CGameObject> GameObjects {
            get {
                return gameObjects;
            }
        }

        public CGameLoop() {

        }

        async public void Start() {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            _frame = 0;
            long TimerFreqency = System.Diagnostics.Stopwatch.Frequency;
            long nanoSecPerTick = (1000L * 1000L * 1000L) / TimerFreqency;
            // ElapsedMilliseconds return 0 if the time is less than 1ms thus we need to use ElapsedTicks
            // With ElaplsedMilliseconds no progress will be made
            //long lastTime = sw.ElapsedMilliseconds;
            long lastTime = sw.ElapsedTicks * nanoSecPerTick;
            Task.Run(() => {
                while (true) {
                    long time = sw.ElapsedTicks * nanoSecPerTick;
                    double elapsed = (double)(time - lastTime) / 1000000000;
                    lastTime = time;
                    Trace.WriteLine("Elapsed (ms): " + (int)(elapsed / 1000000000));
                    Update?.Invoke(this, new TimeElapsedEventArgs {
                        TimeElapsed = elapsed,
                        TotalTime = time / 1000000000,
                        Frame = _frame
                    });
                    _frame++;

                    Draw?.Invoke(this, new TimeElapsedEventArgs {
                        TimeElapsed = elapsed,
                        TotalTime = time / 1000000000,
                        Frame = _frame
                    });
                    Thread.Sleep(1);

                    /*if (Console.KeyAvailable) {
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape) {
                            break;
                        }
                    }*/
                }
            });
        }
        public void AddGameObject(CGameObject gameObject) {
            gameObjects.Add(gameObject);
            Update += gameObject.Update;
        }
    }
}
