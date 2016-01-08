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
        public readonly int Radius;
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

        public bool InOwnTerritory()
        {
            var position = Grid.IndexToPosition(IndexDomain);
            return Grid[position.x, position.y] == (int) Team;
        }

        public bool InRadius(int x, int y)
        {
            var position = Grid.IndexToPosition(IndexDomain);
            return x >= position.x - Radius && x <= position.x + Radius &&
                   y >= position.x - Radius && y <= position.y + Radius;
        }

        public bool IsProtectingTile(int x, int y)
        {
            return InRadius(x, y) && Grid[x, y] == (int) Team;
        }

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
