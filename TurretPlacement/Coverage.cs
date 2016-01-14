using System.Linq;
using ghost;

namespace TurretPlacement
{
    /**
     * This objective function maximizes the coverage of a set of turrets
     * in order to protect as much friendly territory as possible.
     */
    class Coverage : Objective<TurretSet, Turret>
    {

        public Coverage() : base("best coverage")
        {
        }

        public override double Cost(TurretSet turrets)
        {
            // The score is the number of tiles that are uniquely protected (no overlap)
            // plus the number of enemy tiles covered
            var cost = Enumerable
                .Range(0, turrets.GetNumberVariables())
                .Sum(i => turrets[i].UniquelyProtectedTiles().Count() * 1 +
                          turrets[i].UniquelyCoveredEnemyTiles().Count() * 2);

            return cost == 0 ? double.MaxValue : 1.0 / cost; // Inverse sum because we want to maximize the coverage
        }
        /*
        public override int HeuristicVariable(List<int> indexes, TurretSet turrets)
        {
            if (turrets.GetNumberVariables() == 1)
                return base.HeuristicVariable(indexes, turrets);

            // Change in priority the turret that is the closest from another
            return indexes.OrderBy(i => turrets[i].DistanceToClosestTurret()).First();
        }

        public override int HeuristicValue(List<int> valueIndexes, int variableIndex, TurretSet turrets)
        {
            // Go back to the default behavior when there is only one turret
            if (turrets.GetNumberVariables() == 1)
                return base.HeuristicValue(valueIndexes, variableIndex, turrets);

            // Look for the first value with a better distance to the closest turret
            var turret = turrets[variableIndex];
            var better = valueIndexes.OrderBy(i => turret.Grid.Distance(i, turret.GetValue()));
            if (better.Count() > 0)
                return better.Last();

            // If not possible, default behavior
            return base.HeuristicValue(valueIndexes, variableIndex, turrets);
        }
         */
    }
}
