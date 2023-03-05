using System.Runtime.InteropServices;
using NAudio.Midi;

namespace GenshinMidiBinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var keyMap = new Dictionary<int, char>
            {
                { 60, 'A' },
                { 62, 'S' },
                { 64, 'D' },
                { 65, 'J' },
                { 67, 'K' },
                { 69, 'L' },
                { 57, ' ' }
            };

            var midiIn = new MidiIn(0);
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
                        Console.WriteLine("Unknown key: " + note.ToString());
                    }
                } 
                else
                {
                    Console.WriteLine("Not a NodeEvent");
                }
            };

            midiIn.Start();
            Console.WriteLine("Listener launched. Press any key to exit.");
            Console.ReadKey();
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
