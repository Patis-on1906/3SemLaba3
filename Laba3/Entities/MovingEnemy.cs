using System.Text.Json.Serialization;

namespace Laba3
{
    public class MovingEnemy : BaseEntity, IUpdatable, IMoveable
    {
        private static readonly Random _random = new();

        [JsonPropertyName("moveCounter")]
        public int MoveCounter { get; set; } = 0;

        [JsonPropertyName("damage")]
        public int Damage { get; set; }

        [JsonPropertyName("moveSpeed")]
        public int MoveSpeed { get; set; }

        [JsonIgnore]
        public override char Symbol => 'M';

        [JsonIgnore]
        public override EntityType EntityType => EntityType.MovingEnemy;

        [JsonIgnore]
        public override bool IsPassable => false;

        [JsonConstructor]
        public MovingEnemy(string id, int x, int y, int damage, int moveSpeed, int moveCounter) : base()
        {
            Id = id;
            X = x;
            Y = y;
            Damage = Math.Max(damage, 0);
            MoveSpeed = Math.Max(moveSpeed, 1);
            MoveCounter = moveCounter;
        }

        public MovingEnemy() : base()
        {
            Damage = 10;
            MoveSpeed = 2;
        }

        public MovingEnemy(int x, int y, int damage = 10, int moveSpeed = 2) : base(x, y)
        {
            Damage = damage;
            MoveSpeed = moveSpeed;
        }

        public void Update(IMapCollision map, IGameState gameState)
        {
            if (gameState.Player == null) return;

            int dist = Math.Abs(gameState.PlayerX - X) + Math.Abs(gameState.PlayerY - Y);
            if (dist > 10) return;

            if (_random.NextDouble() < 0.2) return;

            // Атака
            if (Math.Abs(gameState.PlayerX - X) <= 1 && Math.Abs(gameState.PlayerY - Y) <= 1)
            {
                if (gameState.Player is IDamageable damageable)
                    damageable.TakeDamage(Damage);
                return;
            }

            // Движение
            MoveCounter++;
            if (MoveCounter < MoveSpeed) return;
            MoveCounter = 0;

            int dx = 0, dy = 0;
            if (gameState.PlayerX > X) dx = 1;
            else if (gameState.PlayerX < X) dx = -1;

            if (gameState.PlayerY > Y) dy = 1;
            else if (gameState.PlayerY < Y) dy = -1;

            if (dx != 0 && TryMove(dx, 0, map, gameState.EntityRepository)) return;
            if (dy != 0 && TryMove(0, dy, map, gameState.EntityRepository)) return;

            var directions = new (int dx, int dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
            var shuffled = directions.OrderBy(d => _random.Next()).ToList();

            foreach (var dir in shuffled)
            {
                if (TryMove(dir.dx, dir.dy, map, gameState.EntityRepository)) return;
            }
        }

        public bool TryMove(int dx, int dy, IMapCollision map, IEntityRepository entities)
        {
            int newX = X + dx;
            int newY = Y + dy;

            if (!map.IsWalkable(newX, newY)) return false;

            var entityAtTarget = entities.GetEntityAt(newX, newY);
            if (entityAtTarget != null)
            {
                if (!entityAtTarget.IsPassable || entityAtTarget.EntityType == EntityType.Treasure)
                    return false;
            }

            SetPosition(newX, newY);
            return true;
        }
    }
}