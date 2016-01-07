using System;
using System.Collections.Generic;
using System.Linq;
using ghost;

namespace BasicSums
{

    using NumberConstraint = Constraint<SetVariables<Variable>, Variable>;
    using NumberSolver = Solver<Variable, SetVariables<Variable>, Constraint<SetVariables<Variable>, Variable>>;

    class Program
    {
        static void Main(string[] args)
        {
            // Values in [0, 9]
            var domain = new Domain(10, 0);

            // Three variables
            var x = new Variable("x", domain, 0);
            var y = new Variable("y", domain, 0);
            var z = new Variable("z", domain, 0);
            var set = new SetVariables<Variable>(x, y, z);
            
            var constraints = new List<NumberConstraint>
            {
                new SmallerThanFixedValue(set, 1, 7), // y (at index 1) should be smaller than 7
                new SmallerThanFixedValue(set, 2, 3) // z (at index 2) should be smaller than 5
            };

            // Aim for the greatest sum of the three numbers
            var objective = new GreatestSum();

            var solver = new NumberSolver(set, constraints, objective);
            solver.Solve(20, 150);

            Console.ReadLine();
        }
    }

    class SmallerThanFixedValue : NumberConstraint
    {

        public readonly int _index;
        public readonly int _value;

        public SmallerThanFixedValue(SetVariables<Variable> variables, int index, int value)
            : base(variables)
        {
            _index = index;
            _value = value;
        }

        public override double Cost(double[] variableCosts)
        {
            // Compute the difference between the constrained variable
            // and the reference value
            double diff = Variables.GetValue(_index) - _value;
            diff = diff < 0 ? 0 : diff;

            // Set the individual cost of each variable to 0 since they 
            // are not included in the constraint, except for the 
            // targeted variable
            for (int i = 0; i < variableCosts.Length; ++i)
                variableCosts[i] = i == _index ? diff : 0.0;

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

    class GreatestSum : Objective<SetVariables<Variable>, Variable>
    {

        public GreatestSum() : base("greatest sum")
        {
        }

        public override double Cost(SetVariables<Variable> variables)
        {
            var cost = Enumerable
                .Range(0, variables.GetNumberVariables())
                .Sum(i => variables.GetValue(i));

            return -cost; // Negative because we want to maximize the cost
        }
    }

}
