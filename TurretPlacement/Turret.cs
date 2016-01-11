using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghost;

namespace TurretPlacement
{
    class Turret : Variable
    {

        public readonly Team Team;
        public readonly int Radius; // The turret protects tiles within this radius
        public readonly Grid Grid;
        public readonly TurretSet AllTurrets;

        public Turret(Team team, int radius, Grid domain, TurretSet allTurrets)
            : base("a turret", domain, 0)
        {
            Team = team;
            Radius = radius;
            Grid = domain;
            AllTurrets = allTurrets;
        }

        /**
         * Check if the turret is currently placed at a position that belongs
         * to its own team.
         */
        public bool InOwnTerritory()
        {
            var position = Grid.IndexToPosition(IndexDomain);
            return Grid[position.x, position.y] == (int) Team;
        }

        /**
         * Check if the position (x,y) lies in the radius of the turret.
         */
        public bool InRadius(int x, int y)
        {
            var position = Grid.IndexToPosition(IndexDomain);
            return x >= position.x - Radius && x <= position.x + Radius &&
                   y >= position.x - Radius && y <= position.y + Radius;
        }

        /**
         * Check if the tile at position (x,y) is currently protected by the turret
         * (if it is in its radius and belongs to the same team).
         */
        public bool IsProtectingTile(int x, int y)
        {
            return InRadius(x, y) && Grid[x, y] == (int) Team;
        }

        /**
         * Count the number of tiles protected by the turret.
         * 
         * @param sharing limits the count to tiles that are not already protected
         * by another turret when set.
         */
        public int ProtectedTiles(bool sharing = false)
        {
            int count = 0;

            var position = Grid.IndexToPosition(IndexDomain);

            for (int x = position.x - Radius; x <= position.x + Radius; ++x)
                for (int y = position.y - Radius; y <= position.y + Radius; ++y)
                    if (Grid.ContainsPosition(x, y) && Grid[x, y] == (int) Team)
                        if (sharing || !AllTurrets.IsTileProtected(x, y, this))
                            ++count;

            return count;
        }
    }
}
