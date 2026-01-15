using System;
using System.Text.Json.Serialization;

namespace Laba3
{
    public class StaticEnemy : BaseEntity, IUpdatable
    {
        [JsonPropertyName("damage")]
        public int Damage { get; set; }

        [JsonPropertyName("attackRange")]
        public int AttackRange { get; set; }

        [JsonPropertyName("attackCooldown")]
        public int AttackCooldown { get; set; }

        [JsonPropertyName("attackCounter")]
        public int AttackCounter { get; set; } = 0;

        [JsonIgnore]
        public override char Symbol => 'S';
        
        [JsonIgnore]
        public override EntityType EntityType => EntityType.StaticEnemy;
        
        [JsonIgnore]
        public override bool IsPassable => false;

        [JsonConstructor]
        public StaticEnemy(string id, int x, int y, int damage, int attackRange, int attackCooldown, int attackCounter) : base()
        {
            Id = id;
            X = x;
            Y = y;
            Damage = Math.Max(damage, 0);
            AttackRange = Math.Max(attackRange, 1);
            AttackCooldown = Math.Max(attackCooldown, 1);
            AttackCounter = attackCounter;
        }

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
                if (AttackCounter >= AttackCooldown)
                {
                    AttackPlayer(playerLocator.Player);
                    AttackCounter = 0;
                }
                else
                {
                    AttackCounter++;
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