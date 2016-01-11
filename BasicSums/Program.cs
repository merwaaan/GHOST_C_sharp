using System;
using System.Collections.Generic;
using System.Linq;
using ghost;

/**
 * Basic example in which we look for the largest sum of a set of integer
 * given some constraints regarding their maximum value. 
 */

namespace BasicSums
{

    using SumSolver = Solver<Variable, SetVariables<Variable>, NumberConstraint>;

    class Program
    {
        static void Main(string[] args)
        {
            // The values will be in [0, 9]
            var domain = new Domain(10, 0);

            // Three variables
            var x = new Variable("x", domain, 0);
            var y = new Variable("y", domain, 0);
            var z = new Variable("z", domain, 0);
            var set = new SetVariables<Variable>(x, y, z);
            
            var constraints = new List<NumberConstraint>
            {
                new SmallerThan(set, 1, 7), // y (at index 1) should be smaller than 7
                new SmallerThan(set, 2, 3) // z (at index 2) should be smaller than 3
            };

            // Look for the largest sum of the three numbers
            // that satisfies the constraints
            var objective = new LargestSum();

            var solver = new SumSolver(set, constraints, objective);
            solver.solve(200, 1500); // There is no rush so take the time to find a good solution

            int total = Enumerable
                .Range(0, set.GetNumberVariables())
                .Sum(i => set.GetValue(i));

            // The optimal result is 17 (x=9, y=6, z=2)
            Console.WriteLine("Computed solution = {0}, Best solution = {1}", total, 9 + 6 + 2);
            Console.ReadLine();
        }
    }

    

    

}
