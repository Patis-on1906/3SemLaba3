using System.Text.Json.Serialization;

namespace Laba3
{
    public class StaticEnemy : BaseEntity, IUpdatable
    {
        private int _damage;
        private int _attackRange;
        private int _attackCooldown;
        private int _attackCounter = 0;

        public override char Symbol => 'S';
        public override EntityType EntityType => EntityType.StaticEnemy;
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

        public int AttackRange
        {
            get => _attackRange;
            set
            {
                if (value < 1)
                    throw new ArgumentException("AttackRange must be at least 1", nameof(value));
                _attackRange = value;
            }
        }

        public int AttackCooldown
        {
            get => _attackCooldown;
            set
            {
                if (value < 1)
                    throw new ArgumentException("AttackCooldown must be at least 1", nameof(value));
                _attackCooldown = value;
            }
        }

        [JsonConstructor]
        public StaticEnemy() : base()
        {
            Damage = 15;
            AttackRange = 1;
            AttackCooldown = 3;
        }

        public StaticEnemy(int x, int y, int damage = 15, int attackRange = 1, int attackCooldown = 3)
            : base(x, y)
        {
            Damage = damage;
            AttackRange = attackRange; 
            AttackCooldown = attackCooldown;
        }

        public void Update(IMapCollision map, IPlayerLocator playerLocator, IEntityCollision entities)
        {
            if (playerLocator.Player == null) return;

            if (IsPlayerInRange(playerLocator))
            {
                if (_attackCounter >= AttackCooldown)
                {
                    AttackPlayer(playerLocator.Player);
                    _attackCounter = 0;
                }
                else
                {
                    _attackCounter++;
                }
            }
        }

        private bool IsPlayerInRange(IPlayerLocator playerLocator)
        {
            return Math.Abs(playerLocator.PlayerX - X) <= AttackRange &&
                   Math.Abs(playerLocator.PlayerY - Y) <= AttackRange;
        }

        private void AttackPlayer(Player player)
        {
            if (player is IDamageable damageable)
            {
                damageable.TakeDamage(Damage);
            }
        }
    }
}