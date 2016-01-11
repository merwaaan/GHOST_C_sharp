using System.Linq;
using ghost;

namespace TurretPlacement
{
    /**
     * Ths objective function maximizes the coverage of a set of turrets
     * in order to protect as much friendly territory as possible.
     */
    class Coverage : Objective<TurretSet, Turret>
    {

        public Coverage() : base("best coverage")
        {
        }

        public override double Cost(TurretSet turrets)
        {
            // Count the tiles that are uniquely protected (no overlap)
            // by all the turrets
            var cost = Enumerable
                .Range(0, turrets.GetNumberVariables())
                .Sum(i => turrets[i].ProtectedTiles());

            return 1.0f/cost; // Inverse sum because we want to maximize the coverage
        }
    }
}
