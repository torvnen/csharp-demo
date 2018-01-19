namespace Demo
{
    using System;
    using System.Drawing;

    /// <summary>
    /// This shall be the viewable part
    /// </summary>
    class ASCIICanvas : IDisposable
    {

        #region Settings class

        public class ASCIICanvasSettings
        {

            /// <summary>
            /// Basic instance creation. 
            /// </summary>
            /// <param name="background"><see cref="Background"/></param>
            /// <param name="foreground"><see cref="Foreground"/></param>
            /// <param name="border"><see cref="Border"/></param>
            public ASCIICanvasSettings(
                char background = '.',
                char foreground = 'X',
                char border = 'O',
                short width = 28,
                short height = 10)
            {
                Background = background;
                Foreground = foreground;
                Border = border;
                Width = width;
                Height = height;
            }

            /// <summary>
            /// Symbol to be used as the texture of the background
            /// </summary>
            public readonly char Background;

            /// <summary>
            /// Symbol to be used as the texture of the foreground,
            /// meaning the 'cursor' character
            /// </summary>
            public readonly char Foreground;

            /// <summary>
            /// Symbol to be used as the border of the canvas
            /// </summary>
            public readonly char Border;

            /// <summary>
            /// Height of the canvas
            /// </summary>
            public readonly short Height;

            /// <summary>
            /// Width of the canvas
            /// </summary>
            public readonly short Width;

        }

        #endregion Settings class

        /// <summary>
        /// Which directions the characters can move towards
        /// </summary>
        [Flags]
        public enum Directions
        {
            Up,
            Down,
            Left,
            Right
        }

        /// <summary>
        /// Parameters for drawing the picture.
        /// </summary>
        public readonly ASCIICanvasSettings Settings;

        /// <summary>
        /// Character (cursor) location
        /// </summary>
        private int _cursorPositionX = 1;

        /// <summary>
        /// Character (cursor) location
        /// </summary>
        private int _cursorPositionY = 1;

        /// <summary>
        /// Where the 'monster' or 'apple' is located
        /// </summary>
        private Point _monsterLocation;

        /// <summary>
        /// <see cref="_cursorPositionX"/>
        /// TODO Use Point struct.
        /// </summary>
        public int CursorPositionX
        {
            get { return _cursorPositionX; }
            set
            {
                _cursorPositionX =
                    // Value inside allowed bounds? 
                    (value > Settings.Width - 2) ? (Settings.Width - 2) :
                    (value < 1) ? 1 : value;
            }
        }

        /// <summary>
        /// <see cref="_cursorPositionY"></see>
        /// TODO Use Point struct.
        /// </summary>
        public int CursorPositionY
        {
            get { return _cursorPositionY; }
            set
            {
                _cursorPositionY =
                    // Value inside allowed bounds? 
                    (value > Settings.Height - 2) ? (Settings.Height - 2) :
                    (value < 1) ? 1 : value;
            }
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">[Optional, will use default values if null or empty]</param>
        public ASCIICanvas(ASCIICanvasSettings settings = null)
        {
            Settings = settings ?? new ASCIICanvasSettings();
        }

        /// <summary>
        /// Use this to draw the initial form
        /// </summary>
        public void Initiate()
        {
            Console.Clear();
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            for (var y = 0; y < Settings.Height; y++)
            {
                for (var x = 0; x < Settings.Width; x++)
                {

                    // All right! Let's do the drawing!
                    Console.Write
                    (
                        (
                            // Is this the first or the last column?
                            x == 0 || x == (Settings.Width - 1)
                            ||
                            // Is this the first or the last row?
                            y == 0 || y == (Settings.Height - 1)
                        ) ? Settings.Border : Settings.Background
                    );
                }
                Console.Write(Environment.NewLine);
            }

            // Spawn something to 'catch' to the game
            _monsterLocation = SpawnMonster();

            Console.CursorLeft = CursorPositionX;
            Console.CursorTop = CursorPositionY;
        }

        /// <summary>
        /// Adds something on the playing field for the player
        /// </summary>
        /// <returns>Location of new monster</returns>
        private Point SpawnMonster()
        {
            var rng = new Random();
            var randomX = rng.Next(1, Settings.Width - 2);
            var randomY = rng.Next(1, Settings.Height - 2);

            if (randomX != CursorPositionX || randomY != CursorPositionY)
            {
                Console.CursorLeft = randomX;
                Console.CursorTop = randomY;
                Console.Write('*');

                return new Point(randomX, randomY);
            }
            else
            {
                // Try again, bad luck
                return SpawnMonster();
            }
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        public void Dispose()
        {
            Console.Clear();
        }

        /// <summary>
        /// Perform a single-character write and move cursor back one unit
        /// </summary>
        /// <param name="c">Char to write</param>
        private void Write(char c)
        {
            // Take previous or initial cursor position:
            Console.CursorLeft = _cursorPositionX;
            Console.CursorTop = _cursorPositionY;

            // Perform writing, move cursor back to compensate for the movement.
            Console.Write(c);
            Console.CursorLeft -= 1;
        }

        /// <summary>
        /// Moves the cursor
        /// </summary>
        /// <param name="direction"></param>
        internal void Move(Directions direction)
        {
            // Overwrite current cursor
            Write(Settings.Background);

            // Perform the actual requested movement
            switch (direction)
            {
                case Directions.Up:
                    CursorPositionY -= 1;
                    break;
                case Directions.Down:
                    CursorPositionY += 1;
                    break;
                case Directions.Left:
                    CursorPositionX -= 1;
                    break;
                case Directions.Right:
                    CursorPositionX += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Did we catch the apple?
            if (CursorPositionX == _monsterLocation.X && CursorPositionY == _monsterLocation.Y)
            {
                _monsterLocation = SpawnMonster();
            }

            // Write character
            Write(Settings.Foreground);
        }
    }
}
