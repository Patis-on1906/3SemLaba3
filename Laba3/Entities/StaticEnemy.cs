using System;
using System.Text.Json;

namespace Laba3
{
    public class StaticEnemy : IEntity, IUpdatable, ISaveable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int X { get; set; } 
        public int Y { get; set; }
        public char Symbol => 'S';
        public EntityType EntityType => EntityType.StaticEnemy;
        
        public int Damage { get; set; } = 15;
        public int AttackRange { get; set; } = 2;
        
        public void Update(IMapCollision map, IPlayerLocator playerLocator, IEntityCollision entities)
        {
            if (Math.Abs(playerLocator.PlayerX - X) <= AttackRange && 
                Math.Abs(playerLocator.PlayerY - Y) <= AttackRange &&
                playerLocator.Player is IDamageable player)
            {
                player.TakeDamage(Damage);
            }
        }
       
        public string Serialize() => JsonSerializer.Serialize(this);

        public static StaticEnemy Deserialize(string json)
        {
            var obj = JsonSerializer.Deserialize<StaticEnemy>(json);
            if (obj == null)
                throw new ArgumentException("Invalid JSON for StaticEnemy");
            return obj;
        }
    }
}