using System;
using System.Collections.Generic;
using System.Linq;
using ghost;

namespace TurretPlacement
{
    class Scenario
    {

        public delegate void Setup(out int numTurrets, out int radius, out Grid grid);

        private Setup _setup;

        public Scenario(Setup setup)
        {
            _setup = setup;
        }

        public void Run()
        {
            // Setup the scenario with the provided function
            int numTurrets;
            int radius;
            Grid grid;
            _setup(out numTurrets, out radius, out grid);

            var turrets = new TurretSet(numTurrets, Team.ALLY, radius, grid);

            var constraints = new List<TurretConstraint>
            {
                new InTerritory(turrets),
                //new ProtectOwnTiles(turrets),
                new NoOverlap(turrets),
                //new CloseToEnemy(turrets)
            };

            var objective = new Coverage();

            var solver = new Solver<Turret, TurretSet, TurretConstraint>(turrets, constraints, objective);
            solver.solve(100, 500);

            grid.Draw(turrets);

            for (int i = 0; i < turrets.GetNumberVariables(); ++i)
            {
                var t = turrets[i];
                Console.WriteLine("Turret " + i + " (radius " + t.Radius + ")");
                Console.WriteLine("    Protected allied tiles " + t.ProtectedTiles().Count());
                Console.WriteLine("    Protected allied tiles (unique) " + t.UniquelyProtectedTiles().Count());
                Console.WriteLine("    Covered enemy tiles " + t.CoveredEnemyTiles().Count());
                Console.WriteLine("    Covered enemy tiles (unique) " + t.UniquelyCoveredEnemyTiles().Count());
                Console.WriteLine("    Distance to enemy " + t.DistanceToClosestEnemyTerritory());
            }
            Console.WriteLine(turrets[0].CoveredEnemyTiles().Count());
        }
    }
}
