using System;
using System.Text.Json;

namespace Laba3
{

    /// Неподвижный враг, который атакует при приближении
   
    public class StaticEnemy : IEnemy
    {
        
       
        
        public int X { get; set; } 
        public int Y { get; set; }


        public int Damage { get; set; } = 5;
        public int AttackRange { get; set; } = 2;
        public int ContactDamage { get; set; } = 15;

       
        /// Символ для отображения
        public char Symbol => 'S';

    
        // Направление "взгляда" врага (для визуализации)
        public Direction FacingDirection { get; private set; } = Direction.North;

    
        /// Обновление состояния врага
      
        public void Update(IMapCollision map, IPlayerLocator playerLocator)
        {
            // Меняем направление "взгляда" для визуализации
            RotateFacingDirection();

            // Атака обрабатывается в GameController через IsPlayerInRange
        }

        
        /// Проверяет, находится ли игрок в зоне атаки
        
        public bool IsPlayerInRange(int playerX, int playerY)
        {
            // Проверяем контакт (игрок на той же клетке)
            if (X == playerX && Y == playerY)
                return true;

            // Проверяем дистанционную атаку (Манхэттенское расстояние)
            int distance = Math.Abs(X - playerX) + Math.Abs(Y - playerY);
            return distance <= AttackRange;
        }

     
        /// Наносит урон игроку

        public void AttackPlayer(Player player)
        {
            int damageToDeal = Damage;

            // Если игрок на той же клетке - контактный урон
            if (X == player.X && Y == player.Y)
            {
                damageToDeal = ContactDamage;
            }

            player.TakeDamage(damageToDeal);
        }

        /// <summary>
        /// Меняет направление "взгляда" врага
        /// </summary>
        private void RotateFacingDirection()
        {
            FacingDirection = FacingDirection switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => Direction.North
            };
        }

       
        /// Сериализация врага
       
        public string Serialize() => JsonSerializer.Serialize(this);

     
        /// Десериализация врага
     
        public void Deserialize(string json)
        {
            var obj = JsonSerializer.Deserialize<StaticEnemy>(json);
            if (obj == null)
                throw new ArgumentException("Invalid JSON for StaticEnemy");

            X = obj.X;
            Y = obj.Y;
            Damage = obj.Damage;
            AttackRange = obj.AttackRange;
            ContactDamage = obj.ContactDamage;
        }

   
        /// Возвращает строковое представление врага
      
        public override string ToString()
        {
            return $"StaticEnemy at ({X},{Y}), Damage: {Damage}, Contact: {ContactDamage}, Range: {AttackRange}";
        }
    }


    /// Направления для статичных врагов
    
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
}