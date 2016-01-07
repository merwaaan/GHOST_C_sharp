using System;
using System.Collections.Generic;

namespace ghost
{

    using CostFunction = Func<double, double[]>;
    using SimulateCostFunction = Func<Dictionary<int, double>, int, Dictionary<int, double>>;
    
    class Problem<TypeSetVariables, TypeVariable>
        where TypeSetVariables : SetVariables<TypeVariable>
        where TypeVariable : Variable
    {

        private List<Variable> _variables;
        private Domain _domain;
 
        public Problem<TypeSetVariables, TypeVariable> Variables(params TypeVariable[] variables)
        {

            return this;
        }

        public Problem<TypeSetVariables, TypeVariable> Domain(Domain domain)
        {
            _domain = domain;

            return this;
        }

        public Problem<TypeSetVariables, TypeVariable> Constraints(params Constraint<TypeSetVariables, TypeVariable>[] constraints)
        {

            return this;
        }

        public Problem<TypeSetVariables, TypeVariable> Objective(Objective<TypeSetVariables, TypeVariable> objective)
        {

            return this;
        }

        public void Solve()
        {
            // ...
        }
        /*
        internal class ConstraintWrapper : Constraint<TypeSetVariables, TypeVariable>
        {

            private readonly CostFunction _costFunction;
            private readonly SimulateCostFunction _simulateCostFunction;

            public ConstraintWrapper(Func<double, double[]> costFunction, Func<Dictionary<int, double>, int, Dictionary<int, double>> simulateCostFunction)
            {
                
            }

            protected double Cost(double[] variableCost, string resourceType)
            {
                _costFunction.Invoke(variableCost);
            }

            protected double SimulateCost(double[] variableCost, string resourceType)
            {
                _simulateCostFunction.Invoke(...);
            }
        }*/
    }

}
