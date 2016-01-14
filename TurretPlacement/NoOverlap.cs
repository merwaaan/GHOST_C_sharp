using System.Linq;

namespace TurretPlacement
{

    /**
     * This constraint ensures that turrets are placed on friendly tiles only.
     */
    class NoOverlap : TurretConstraint
    {

        private int _distance;

        public NoOverlap(TurretSet turrets, int distance = 0)
            : base(turrets)
        {
            _distance = distance;
        }

        public override double Cost(double[] variableCosts)
        {
            int overlaps = 0;

            for (int i = 0; i < Variables.GetNumberVariables(); ++i)
            {
                var turretI = Variables[i];

                for (int j = 0; j < Variables.GetNumberVariables(); ++j)
                {
                    if (i == j)
                        continue;

                    var turretJ = Variables[j];

                    if (turretI.IsCovering(turretJ.GetValue()))
                    {
                        ++variableCosts[i];
                        ++overlaps;
                    }
                }
            }

            return overlaps;
        }
    }
}
