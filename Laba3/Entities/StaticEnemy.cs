using System;
using System.Text.Json;

namespace Laba3
{
    public class StaticEnemy : IEntity, IUpdatable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol => 'S';
        public EntityType EntityType => EntityType.StaticEnemy;
        public bool IsPassable => false;

        public int Damage { get; set; } = 15;
        public int AttackRange { get; set; } = 2;

        // Cooldown на атаку
        public int AttackCooldown { get; set; } = 3;
        private int _attackCounter = 0;

        public void Update(IMapCollision map, IPlayerLocator playerLocator, IEntityCollision entities)
        {
            if (IsPlayerInRange(playerLocator))
            {
                // Атакуем с учетом cooldown
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