using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghost;

namespace TurretPlacement
{
    abstract class TurretConstraint : Constraint<TurretSet, Turret>
    {

        protected TurretConstraint(TurretSet turrets) : base(turrets)
        {
        }

    }
}
