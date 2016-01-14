using System.Collections.Generic;
using ghost;

namespace TurretPlacement
{
    abstract class TurretConstraint : Constraint<TurretSet, Turret>
    {
        protected TurretConstraint(TurretSet turrets) : base(turrets)
        {
        }

        public override Dictionary<int, double> SimulateCost(int variableIndex, Dictionary<int, double[]> variableSimCost)
        {
            var simCosts = new Dictionary<int, double>();

            int backup = Variables.GetValue(variableIndex);

            foreach (int value in Variables.PossibleValues(variableIndex))
            {
                Variables.SetValue(variableIndex, value);
                simCosts[value] = Cost(variableSimCost[value]);
            }

            Variables.SetValue(variableIndex, backup);

            return simCosts;
        }
    }
}
