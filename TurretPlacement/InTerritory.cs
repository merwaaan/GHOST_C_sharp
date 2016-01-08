using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurretPlacement
{
    class InTerritory : TurretConstraint
    {

        public InTerritory(TurretSet turrets) : base(turrets)
        {
        }

        public override double Cost(double[] variableCost)
        {
            var costs = Enumerable
                .Range(0, Variables.GetNumberVariables())
                .Select(i => Variables[i].InOwnTerritory() ? 0.0 : int.MaxValue);

            double total = costs.Sum();

            /*if (total <= Resource)
                return 0;
            else
            {
                double surplus = total - Resource;
                for (int i = 0; i < Variables.GetNumberVariables(); ++i)
                    variableCost[i] = costs[i] >= surplus ? costs[i] : 0;
            }*/

            return total;
        }

        public override Dictionary<int, double> SimulateCost(int currentVariableIndex, Dictionary< int, double[] > variableSimCost)
        {
          var simCosts = new Dictionary<int, double>();

          int backup = Variables.GetValue( currentVariableIndex );

          foreach( var pos in Variables.PossibleValues( currentVariableIndex ) )
          {
            Variables.SetValue( currentVariableIndex, pos );
            simCosts[ pos ] = Cost( variableSimCost[pos]);
          }

          Variables.SetValue( currentVariableIndex, backup );
          return simCosts;
        }
    }
}
