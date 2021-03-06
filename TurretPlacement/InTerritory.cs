﻿using System.Linq;

namespace TurretPlacement
{

    /**
     * This constraint ensures that turrets are placed on friendly tiles only.
     */
    class InTerritory : TurretConstraint
    {

        public InTerritory(TurretSet turrets)
            : base(turrets)
        {
        }

        public override double Cost(double[] variableCosts)
        {
            // Accumulate cost for each turret out of its territory
            var costs = Enumerable
                .Range(0, Variables.GetNumberVariables())
                .Select(i => Variables[i].InOwnTerritory() ? 0.0 : 1.0f)
                .ToArray();

            for (int i = 0; i < variableCosts.Length; ++i)
                variableCosts[i] += costs[i];

            return costs.Sum();
        }
    }
}
