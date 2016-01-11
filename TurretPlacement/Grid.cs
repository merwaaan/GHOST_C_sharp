using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ghost;

namespace TurretPlacement
{
    class Grid : Domain
    {
        public struct Position
        {
            public int x;
            public int y;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        protected int[] _data;

        public int this[int x, int y]
        {
            get { return _data[PositionToIndex(x, y)]; }
            set { _data[PositionToIndex(x, y)] = value; }
        }

        public Grid(int width, int height) : base(width * height, 0)
        {
            Width = width;
            Height = height;

            _data = Enumerable.Repeat(-1, Width * Height).ToArray();
        }

        /**
         * Fill a square of the grid with a specific value.
         * Useful for quickly adding patches of territory.
         */
        public void Square(int left, int top, int width, int height, int value)
        {
            for (int x = left; x < left + width && x < Width; ++x)
                for (int y = top; y < top + height && y < Height; ++y)
                    this[x, y] = value;
        }

        /**
         * Convert a domain index to the corresponding (x,y) position.
         */
        public Position IndexToPosition(int index)
        {
            return new Position
            {
                x = index % Width,
                y = index / Width
            };
        }

        /**
         * Convert an (x,y) position to the corresponding domain index.
         */
        public int PositionToIndex(int x, int y)
        {
            return y*Width + x;
        }

        /**
         * Check if the position (x,y) is in the grid.
         */
        public bool ContainsPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /**
         * Draw the grid in the console.
         * Values stored in the grid and turrets are printed on the map.
         */
        public void Draw(TurretSet turrets)
        {
            string line = new string('-', Width + 2);
            Console.WriteLine(line);

            for (int i = 0; i < _data.Length; ++i)
            {
                if (i % Width == 0)
                    Console.Write("|");

                Turret turret = turrets.At(i);
                if (turret != null)
                    Console.Write("X");
                else
                    Console.Write(_data[i] >= 0 ? _data[i].ToString() : " ");

                if (i % Width == Width -1)
                    Console.WriteLine('|');
            }

            Console.WriteLine(line);
        }
    }
}
