using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghost;

namespace TurretPlacement
{
    enum Team {A, B}

    class Program
    {
        static void Main(string[] args)
        {
            // See the ScenarioN() methods for various setups
            int numTurrets, radius;
            Grid grid;
            Scenario1(out numTurrets, out radius, out grid);

            var turrets = new TurretSet(numTurrets, Team.A, radius, grid);

            var constraints = new List<TurretConstraint>
            {
                //new InTerritory(turrets), // TODO: remove, should be naturally handled by the obj function
                //new ReducedOverlap(), // TODO: same
                //new CloseToEnemy() // TODO: should be close to potential threats (objective or constraint?)
            };

            var objective = new Coverage();

            var solver = new Solver<Turret, TurretSet, TurretConstraint>(turrets, constraints, objective);
            solver.solve(20, 150);

            grid.Draw(turrets);
            Console.WriteLine(turrets[0].ProtectedTiles());
            Console.Read();
        }

        static void Scenario1(out int numTurrets, out int radius, out Grid grid)
        {
            numTurrets = 2;
            radius = 4;

            grid = new Grid(20, 20);
            grid.Square(5, 5, 7, 3, (int)Team.A);
            grid.Square(10, 7, 5, 5, (int)Team.A);
            grid.Square(8, 15, 4, 4, (int)Team.A);
        }
    }
}
