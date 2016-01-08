using System.Linq;
using ghost;

namespace TurretPlacement
{
    class Coverage : Objective<TurretSet, Turret>
    {

        public Coverage() : base("best coverage")
        {
        }

        public override double Cost(TurretSet turrets)
        {
            var cost = Enumerable
                .Range(0, turrets.GetNumberVariables())
                .Sum(i => turrets[i].ProtectedTiles());

            return -cost; // Negative because we want to maximize the coverage
        }
    }
}
