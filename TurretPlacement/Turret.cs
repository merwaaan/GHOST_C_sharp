using System.Collections.Generic;
using System.Linq;
using ghost;

namespace TurretPlacement
{
    class Turret : Variable
    {

        public readonly Team Team;
        public readonly int Radius; // The turret protects tiles within this radius
        public readonly Grid Grid;
        public readonly TurretSet AllTurrets;

        public Team OtherTeam
        {
            get { return Team == Team.ALLY ? Team.ENEMY : Team.ALLY;  }
        }

        public Turret(Team team, int radius, Grid domain, TurretSet allTurrets)
            : base("a turret", domain, 0)
        {
            Team = team;
            Radius = radius;
            Grid = domain;
            AllTurrets = allTurrets;
        }

        public Grid.Position Position
        {
            get { return Grid.IndexToPosition(IndexDomain); }
        }

        /**
         * Check if the turret is currently placed at a position that belongs
         * to its own team.
         */
        public bool InOwnTerritory()
        {
            var pos = Position;
            return Grid[pos.x, pos.y] == Team.ALLY;
        }

        /**
         * Check if the position (x,y) lies in the radius of the turret.
         */
        public bool IsCovering(int x, int y)
        {
            // Older square version
            var pos = Position;
            return x >= pos.x - Radius && x <= pos.x + Radius &&
                   y >= pos.y - Radius && y <= pos.y + Radius;

            return Grid.Distance(IndexDomain, Grid.PositionToIndex(x, y)) < Radius;
        }

        public bool IsCovering(int index)
        {
            var pos = Grid.IndexToPosition(index);
            return IsCovering(pos.x, pos.y);
        }

        /**
         * Check if the tile at position (x,y) is currently protected by the turret
         * (if it is in its radius and belongs to the same team).
         */
        public bool IsProtecting(int x, int y)
        {
            return IsCovering(x, y) && Grid[x, y] == Team.ALLY;
        }

        public bool IsProtecting(int index)
        {
            var pos = Grid.IndexToPosition(index);
            return IsProtecting(pos.x, pos.y);
        }

        public IEnumerable<int> ProtectedTiles()
        {
            return PossibleValues().Where(index => IsProtecting(index));
        }

        /**
         * Count the number of tiles protected by the turret.
         */
        public IEnumerable<int> UniquelyProtectedTiles()
        {
            var protectedTiles = ProtectedTiles().ToList();

            for (int i = 0; i < AllTurrets.GetNumberVariables(); ++i)
                if (AllTurrets[i] != this)
                {
                    var coveredByOther = AllTurrets[i].ProtectedTiles();
                    protectedTiles.RemoveAll(tile => coveredByOther.Contains(tile));
                }

            return protectedTiles;
        }

        public IEnumerable<int> CoveredEnemyTiles()
        {
            return PossibleValues().Where(index =>
            {
                var pos = Grid.IndexToPosition(index);
                
                if (!IsCovering(pos.x, pos.y))
                    return false;

                return Grid[pos.x, pos.y] == Team.ENEMY;
            });
        }

        public IEnumerable<int> UniquelyCoveredEnemyTiles()
        {
            var coveredTiles = CoveredEnemyTiles().ToList();

            for (int i = 0; i < AllTurrets.GetNumberVariables(); ++i)
                if (AllTurrets[i] != this)
                {
                    var coveredByOther = AllTurrets[i].CoveredEnemyTiles();
                    coveredTiles.RemoveAll(tile => coveredByOther.Contains(tile));
                }

            return coveredTiles;
        }

        public int ClosestEnemyTerritory()
        {
            var enemyTiles = PossibleValues().Where(index => Grid[index] == Team.ENEMY);

            if (enemyTiles.Count() == 0)
                return -1;

            return enemyTiles
                .OrderBy(index => Grid.Distance(index, IndexDomain))
                .First();
        }

        public double DistanceToClosestEnemyTerritory()
        {
            int closest = ClosestEnemyTerritory();
            if (closest < 0)
                return 0.0f;

            return Grid.Distance(IndexDomain, closest);
        }

        public int ClosestTurret()
        {
            return Enumerable
                .Range(0, AllTurrets.GetNumberVariables())
                .Where(i => this != AllTurrets[i])
                .OrderBy(i => Grid.Distance(IndexDomain, AllTurrets[i].IndexDomain))
                .First();
        }

        public double DistanceToClosestTurret()
        {
            int closest = ClosestTurret();
            if (closest < 0)
                return 0.0f;

            return Grid.Distance(IndexDomain, closest);
        }
    }
}
