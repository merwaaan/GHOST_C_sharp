using System;
using System.Collections.Generic;
using System.Linq;
using ghost;

namespace TurretPlacement
{
    enum Team {NONE, ALLY, ENEMY}

    class Program
    {
        static void Main(string[] args)
        {
            Scenario[] scenarios =
            {
                // Linear territory (should be placed somewhere in the middle)
                new Scenario((out int numTurrets, out int radius, out Grid grid) =>
                {
                    numTurrets = 1;
                    radius = 3;
                    grid = new Grid(9, 3);
                    grid.Square(1, 1, 7, 1, Team.ALLY);
                }),

                // Long linear territory (should be evenly distributed)
                new Scenario((out int numTurrets, out int radius, out Grid grid) =>
                {
                    numTurrets = 5;
                    radius = 2;
                    grid = new Grid(30, 3);
                    grid.Square(1, 1, 28, 1, Team.ALLY);
                }),

                // Big chunk of territory containing enemy territory
                new Scenario((out int numTurrets, out int radius, out Grid grid) =>
                {
                    numTurrets = 3;
                    radius = 2;
                    grid = new Grid(12, 12);
                    grid.Square(1, 1, 10, 10, Team.ALLY);
                    grid.Square(5, 5, 8, 2, Team.ENEMY);
                }),

                // Separated chunks of territory
                new Scenario((out int numTurrets, out int radius, out Grid grid) =>
                {
                    numTurrets = 3;
                    radius = 2;
                    grid = new Grid(12, 12);
                    grid.Square(1, 1, 7, 3, Team.ALLY);
                    grid.Square(6, 3, 5, 5, Team.ALLY);
                    grid.Square(3, 8, 4, 4, Team.ALLY);
                }),

                // Side-to-side territories
                new Scenario((out int numTurrets, out int radius, out Grid grid) =>
                {
                    numTurrets = 2;
                    radius = 3;
                    grid = new Grid(10, 10);
                    grid.Square(0, 0, 7, 10, Team.ALLY);
                    grid.Square(8, 0, 2, 10, Team.ENEMY);
                }),

                // Criss-crossed territory
                new Scenario((out int numTurrets, out int radius, out Grid grid) =>
                {
                    numTurrets = 3;
                    radius = 2;
                    grid = new Grid(15, 15);
                    grid.Square(6, 0, 8, 8, Team.ENEMY);
                    grid.Square(0, 7, 8, 8, Team.ENEMY);
                    grid.Square(1, 1, 4, 4, Team.ALLY);
                    grid.Square(8, 8, 5, 7, Team.ALLY);
                }),

                // Fragmented territory
                new Scenario((out int numTurrets, out int radius, out Grid grid) =>
                {
                    numTurrets = 3;
                    radius = 2;
                    grid = new Grid(8, 8);

                    var rnd = new Random(666);
                    for (int x = 0; x < 8; ++x)
                        for (int y = 0; y < 8; ++y)
                            grid.Square(x, y, 1, 1, rnd.Next(0, 2) == 0 ? Team.NONE : Team.ALLY);
                })
            };
            
            scenarios.ToList().ForEach(s => s.Run());
            //scenarios.Last().Run();

            Console.Read();
        }
    }
}
