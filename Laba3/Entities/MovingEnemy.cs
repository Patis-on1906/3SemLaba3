using System.Text.Json.Serialization;

namespace Laba3
{
    public class MovingEnemy : MovingUnit, IUpdatable
    {
        private static readonly Random _random = new();
        private int _moveCounter = 0;

        private int _damage;
        private int _moveSpeed;

        public override char Symbol => 'M';
        public override EntityType EntityType => EntityType.MovingEnemy;
        public override bool IsPassable => false;

        public int Damage
        {
            get => _damage;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Damage cannot be negative", nameof(value));
                _damage = value;
            }
        }

        public int MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                if (value < 1)
                    throw new ArgumentException("MoveSpeed must be at least 1", nameof(value));
                _moveSpeed = value;
            }
        }

        [JsonConstructor]
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

        public void Update(IMapCollision map, IPlayerLocator playerLocator, IEntityCollision entities)
        {
            if (playerLocator.Player == null) return;

            int dist = Math.Abs(playerLocator.PlayerX - X) + Math.Abs(playerLocator.PlayerY - Y);
            if (dist > 10) return;

            if (_random.NextDouble() < 0.2) return;

            // Атака
            if (Math.Abs(playerLocator.PlayerX - X) <= 1 && Math.Abs(playerLocator.PlayerY - Y) <= 1)
            {
                if (playerLocator.Player is IDamageable damageable)
                    damageable.TakeDamage(Damage);
                return;
            }

            // Движение
            _moveCounter++;
            if (_moveCounter < MoveSpeed) return;
            _moveCounter = 0;

            int dx = 0, dy = 0;
            if (playerLocator.PlayerX > X) dx = 1;
            else if (playerLocator.PlayerX < X) dx = -1;

            if (playerLocator.PlayerY > Y) dy = 1;
            else if (playerLocator.PlayerY < Y) dy = -1;

            if (dx != 0 && TryMove(dx, 0, map, entities)) return;
            if (dy != 0 && TryMove(0, dy, map, entities)) return;

            var directions = new (int dx, int dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
            var shuffled = directions.OrderBy(d => _random.Next()).ToList();

            foreach (var dir in shuffled)
            {
                if (TryMove(dir.dx, dir.dy, map, entities)) return;
            }
        }

        public new bool TryMove(int dx, int dy, IMapCollision map, IEntityCollision entities)
        {
            int newX = X + dx;
            int newY = Y + dy;

            if (!map.IsWalkable(newX, newY)) return false;

            var entityAtTarget = entities.GetEntityAt(newX, newY);

            if (entityAtTarget != null && !entityAtTarget.IsPassable)
                return false;

            SetPosition(newX, newY);
            return true;
        }
    }
}