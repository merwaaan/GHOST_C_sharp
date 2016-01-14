using System;
using System.Collections.Generic;
using ghost;

namespace BasicSums
{

    abstract class NumberConstraint : Constraint<SetVariables<Variable>, Variable>
    {
        public NumberConstraint(SetVariables<Variable> variables) : base(variables)
        {
        }
    }

    class SmallerThan : NumberConstraint
    {

        public readonly int _index; // Index of the variable in the set that is constrained
        public readonly int _value; // Value that this variable must be less than

        public SmallerThan(SetVariables<Variable> variables, int index, int value)
            : base(variables)
        {
            _index = index;
            _value = value;
        }

        public override double Cost(double[] variableCosts)
        {
            // Compute the difference between the current value of
            // the constrained variable and the reference value
            double diff = Variables.GetValue(_index) - _value;
            diff = diff < 0 ? 0 : diff + 1; // +1 because we want the value to be strictly smaller

            // Add this cost to the cost of the constrained variable
            variableCosts[_index] += diff;

            return diff;
        }

        public override Dictionary<int, double> SimulateCost(int variableIndex, Dictionary<int, double[]> variableSimulatedCosts)
        {
            var simulatedCost = new Dictionary<int, double>();

            // Save the current value of the variable
            int backup = Variables.GetValue(variableIndex);

            // Calculate the global cost for each possible value of the variable
            foreach (int value in Variables.PossibleValues(variableIndex))
            {
                Variables.SetValue(variableIndex, value);
                simulatedCost[value] = Cost(variableSimulatedCosts[value]);
            }

            // Restore the original value
            Variables.SetValue(variableIndex, backup);

            return simulatedCost;
        }
    }
}
