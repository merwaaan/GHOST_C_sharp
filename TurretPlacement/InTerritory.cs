using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurretPlacement
{

    /**
     * This constraint ensures that turrets are placed on friendly tiles only.
     */
    class InTerritory : TurretConstraint
    {

        public InTerritory(TurretSet turrets) : base(turrets)
        {
        }

        public override double Cost(double[] variableCost)
        {
            // Accumulate cost for each turret out of its territory
            var costs = Enumerable
                .Range(0, Variables.GetNumberVariables())
                .Select(i => Variables[i].InOwnTerritory() ? 0.0 : 1.0f);

            double total = costs.Sum();

            // TODO should fill variableCost???

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
