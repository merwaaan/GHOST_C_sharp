using System.Linq;

namespace TurretPlacement
{

    /**
     * This constraint ensures that turrets are placed on friendly tiles only.
     */
    class ProtectOwnTiles : TurretConstraint
    {

        public ProtectOwnTiles(TurretSet turrets)
            : base(turrets)
        {
        }

        public override double Cost(double[] variableCosts)
        {
            // Count the allied tiles that are protected by each turret
            var tiles = Enumerable
                .Range(0, Variables.GetNumberVariables())
                .Select(i => Variables[i].UniquelyProtectedTiles().Count())
                .ToArray();

            for (int i = 0; i < variableCosts.Length; ++i)
                variableCosts[i] += 1.0f/tiles[i];

            return 1.0f/tiles.Sum();
        }
    }
}
