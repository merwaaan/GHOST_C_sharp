using System.Linq.Expressions;
using ghost;

namespace TurretPlacement
{
    class TurretSet : SetVariables<Turret>
    {

        public Turret this[int index]
        {
            get { return Variables[index]; }
        }

        public TurretSet(int numTurrets, Team team, int radius, Grid grid)
        {
            for (int i = 0; i < numTurrets; ++i)
                Variables.Add(new Turret(team, radius, grid, this));
        }

        public Turret At(int value)
        {
            return Variables.Find(turret => turret.GetValue() == value);
        }

        public bool IsTileProtected(int x, int y, Turret exclude = null)
        {
            return Variables.Exists(turret =>
            {
                if (exclude != null && turret == exclude)
                    return false;

                return turret.IsProtectingTile(x, y);
            });
        }
    }
}
