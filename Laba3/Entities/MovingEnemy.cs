using System;
using System.Text.Json;

namespace Laba3
{
    public class MovingEnemy : MovingUnit, IUpdatable, ISaveable
    {
        public override char Symbol => 'M';
        public override EntityType EntityType => EntityType.MovingEnemy;

        public int Damage { get; set; } = 10;
        
        public void Update(IMapCollision map, IPlayerLocator playerLocator, IEntityCollision entities)
        {
            int dx = 0;
            int dy = 0;

            // Определяем направление к игроку
            if (playerLocator.PlayerX > X) dx = 1;
            else if (playerLocator.PlayerX < X) dx = -1;

            if (playerLocator.PlayerY > Y) dy = 1;
            else if (playerLocator.PlayerY < Y) dy = -1;

            // Пытаемся двигаться: диагональ -> по X -> по Y
            if (!Move(dx, dy, map, entities))
            {
                if (!Move(dx, 0, map, entities))
                {
                    Move(0, dy, map, entities);
                }
            }

            // Проверяем атаку
            if (Math.Abs(playerLocator.PlayerX - X) <= 1 && 
                Math.Abs(playerLocator.PlayerY - Y) <= 1 &&
                playerLocator.Player is IDamageable player)
            {
                player.TakeDamage(Damage);
            }
        }

        public string Serialize() => JsonSerializer.Serialize(this);

        public static MovingEnemy Deserialize(string json)
        {
            var obj = JsonSerializer.Deserialize<MovingEnemy>(json);
            if (obj == null)
                throw new ArgumentException("Invalid JSON for MovingEnemy");
            return obj;
        }
    }
}