using System;
using System.Linq;

namespace TurretPlacement
{

    /**
     * This constraint ensures that turrets are placed on friendly tiles only.
     */
    class CloseToEnemy : TurretConstraint
    {

        public CloseToEnemy(TurretSet turrets)
            : base(turrets)
        {
        }

        public override double Cost(double[] variableCosts)
        {
            // Cancel the constraint if there is no enemy tiles in the map
            int enemyTiles = Variables[0]
                .PossibleValues()
                .Count(i => Variables[0].Grid[i] == Team.ENEMY);

            if (enemyTiles == 0)
                return 0;

            // Accumulate cost when a turret is not next to an anemy tile
            var costs = Enumerable
                .Range(0, Variables.GetNumberVariables())
                .Select(i => Math.Max(0, Variables[i].DistanceToClosestEnemyTerritory() - Variables[i].Radius))
                .ToArray();

            for (int i = 0; i < variableCosts.Length; ++i)
                variableCosts[i] += costs[i];

            return costs.Sum();
        }
    }
}
