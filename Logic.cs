using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using NAudio.Midi;

namespace GenshinMidiBinder
{
    class Logic
    {
        public enum Mode
        {
            Lyre,
            Game
        }

        public static void Run(TextBox box, Mode mode, MidiIn midiIn)
        {
            box.AppendText("Starting..." + Environment.NewLine);
            Dictionary<int, char> keyMap;

            if (mode == Mode.Lyre)
            {
                keyMap = new Dictionary<int, char>
                {
                    {48, 'Z'},
                    {50, 'X'},
                    {52, 'C'},
                    {53, 'V'},
                    {55, 'B'},
                    {57, 'N'},
                    {59, 'M'},
                    {60, 'A'},
                    {62, 'S'},
                    {64, 'D'},
                    {65, 'F'},
                    {67, 'G'},
                    {69, 'H'},
                    {71, 'J'},
                    {72, 'Q'},
                    {74, 'W'},
                    {76, 'E'},
                    {77, 'R'},
                    {79, 'T'},
                    {81, 'Y'},
                    {83, 'U'}
                };
            } else
            {
                keyMap = new Dictionary<int, char>
                {
                    { 60, 'A' },
                    { 62, 'S' },
                    { 64, 'D' },
                    { 65, 'J' },
                    { 67, 'K' },
                    { 69, 'L' },
                    { 57, ' ' }
                };
            }

            midiIn.MessageReceived += (sender, e) =>
            {
                var commandCode = e.MidiEvent.CommandCode;
                if (commandCode == MidiCommandCode.NoteOn || commandCode == MidiCommandCode.NoteOff)
                {
                    var note = ((NoteEvent)e.MidiEvent).NoteNumber;
                    if (keyMap.ContainsKey(note))
                    {
                        var key = keyMap[note];
                        if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
                        {
                            GameInput.KeyDown(key);
                        }
                        else if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOff)
                        {
                            GameInput.KeyUp(key);
                        }
                    }
                    else
                    {
                        box.Invoke(new Action(() => box.AppendText("Unknown key: " + note.ToString() + Environment.NewLine)));
                    }
                }
                else
                {
                    box.Invoke(new Action(() => box.AppendText("Not a NodeEvent" + Environment.NewLine)));
                }
            };


            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            midiIn.Start();
        }

        public static void Stop(MidiIn midiIn)
        {
            midiIn.Stop();
        }
    }

    static class GameInput
    {
        public static void KeyDown(char key)
        {
            keybd_event((byte)key, 0, 0, (UIntPtr)0);
        }

        public static void KeyUp(char key)
        {
            keybd_event((byte)key, 0, 2, (UIntPtr)0);
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    }

}
