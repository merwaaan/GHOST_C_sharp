using System;
using System.Linq;
using ghost;

namespace TurretPlacement
{
    class Grid : Domain
    {
        public struct Position
        {
            public int x;
            public int y;

            public override string ToString()
            {
                return "(" + x + "," + y +")";
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        protected Team[] _data;

        public Team this[int x, int y]
        {
            get { return _data[PositionToIndex(x, y)]; }
            set { _data[PositionToIndex(x, y)] = value; }
        }

        public Team this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public Grid(int width, int height) : base(width * height, 0)
        {
            Width = width;
            Height = height;

            _data = Enumerable.Repeat(Team.NONE, Width * Height).ToArray();
        }

        /**
         * Fill a square of the grid with a specific value.
         * Useful for quickly adding patches of territory.
         */
        public void Square(int left, int top, int width, int height, Team team)
        {
            for (int x = left; x < left + width && x < Width; ++x)
                for (int y = top; y < top + height && y < Height; ++y)
                    this[x, y] = team;
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

        public double Distance(int index1, int index2)
        {
            var pos1 = IndexToPosition(index1);
            var pos2 = IndexToPosition(index2);
            return Math.Sqrt(Math.Pow(pos1.x - pos2.x, 2) + Math.Pow(pos1.y - pos2.y, 2));
        }

        public double DistanceToClosestTerritory(int posIndex, Team team)
        {
            return Enumerable.Range(0, Width*Height)
                .Where(index => this[index] == team)
                .OrderBy(index => Distance(posIndex, index))
                .First();
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
                    Console.Write(turrets.GetIndex(turret));
                else 
                    switch (_data[i])
                    {
                        case Team.ALLY:
                            Console.Write("A");
                            break;
                        case Team.ENEMY:
                            Console.Write("B");
                            break;
                        case Team.NONE:
                            Console.Write(" ");
                            break;
                    }


                if (i % Width == Width - 1)
                    Console.WriteLine('|');
            }

            Console.WriteLine(line);
        }
    }
}
