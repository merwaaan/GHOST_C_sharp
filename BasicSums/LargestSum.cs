using System;
using System.Collections.Generic;
using System.Linq;
using ghost;

namespace BasicSums
{

    class LargestSum : Objective<SetVariables<Variable>, Variable>
    {

        public LargestSum()
            : base("largest sum")
        {
        }

        public override double Cost(SetVariables<Variable> variables)
        {
            // Compute the sum for the current variable assignments
            var cost = Enumerable
                .Range(0, variables.GetNumberVariables())
                .Sum(i => variables.GetValue(i));

            // Return the inverse sum since we want to maximize it
            return 1.0f/cost;
        }

        public virtual int HeuristicValue(List<int> valuesIndex, int variableIndex, SetVariables<Variable> variables)
        {
            // Choose the best new value within all best values
            // such that the variable is the largest possible
            return valuesIndex.Max(i => variables.GetValue(i));
        }
    }
}
