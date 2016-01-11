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

            // Return the negative sum since we want to maximize the cost
            return -cost;
        }
    }
}
