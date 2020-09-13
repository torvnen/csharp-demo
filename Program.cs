namespace Demo
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This program creates a matrix of ASCII characters
    /// which are navigatable through keyboard input by another
    /// ASCII character which acts as the cursor.
    /// 
    /// - NO LINQ. 
    /// - ONLY.NET Core 2.0 compatible packages!
    /// - ONLY new code. No copy-paste!
    /// 
    /// @author Niko Torvinen
    /// 
    /// </summary>
    class Program
    {

        /// <summary>
        /// Which key was pressed.
        /// </summary>
        private static ConsoleKeyInfo _keyinfo;
        
        /// <summary>
        /// The matrix of ASCII characters; 
        /// this is what will be drawn on the terminal console.
        /// </summary>
        private static ASCIICanvas _view = new ASCIICanvas();

        private static string _utilityName = "Simple snake game";

        /// <summary>
        /// Possible command line arguments and mappings to their actions
        /// </summary>
        private static Dictionary<string, Action> _switches = new Dictionary<string, Action>
        {
            {
                "-h", () => 
                { 
                    // Print instructions:
                    Console.Write(_helptext);
                }
            },
            {
                "-p", () =>
                {
                    // Play the mini-game
                    PlayGame();
                }
            }
        };

        /// <summary>
        /// The instructions for the program.
        /// </summary>
        private static string _helptext {
            get
            {
                var keys = new string[_switches.Keys.Count];
                _switches.Keys.CopyTo(keys, 0);
                var sb = new StringBuilder();
                foreach (string key in keys)
                {
                    sb.Append($"[{key}]");
                }

                return 
                    $"{_utilityName}{sb}{Environment.NewLine}" +
                    "Press ESCAPE to quit.";
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new[] { "-p" };
            }

            foreach (var arg in args)
            {
                if (_switches.ContainsKey(arg))
                {
                    _switches[arg]?.Invoke();
                }
            }

            Console.WriteLine("Goodbye! :)");
            Console.ReadKey();
        }

        /// <summary>
        /// Draw a canvas and start playing!
        /// </summary>
        static void PlayGame()
        {

            using (var canvas = new ASCIICanvas())
            {
                var keybindings = new Dictionary<ConsoleKey, Action>
                {
                    { ConsoleKey.UpArrow, () => { canvas.Move(ASCIICanvas.Directions.Up); } },
                    { ConsoleKey.DownArrow, () => { canvas.Move(ASCIICanvas.Directions.Down); } },
                    { ConsoleKey.LeftArrow, () => { canvas.Move(ASCIICanvas.Directions.Left); } },
                    { ConsoleKey.RightArrow, () => { canvas.Move(ASCIICanvas.Directions.Right); } }
                };

                // Draw initial canvas!
                canvas.Initiate();
                
                do
                {
                    _keyinfo = Console.ReadKey();

                    if (keybindings.ContainsKey(_keyinfo.Key))
                    {
                        keybindings[_keyinfo.Key].Invoke();
                    }
                    else
                    {

                        // TODO handle invalid input
                    }

                }
                while (_keyinfo.Key != ConsoleKey.Escape);
            }
        }
    }
}
