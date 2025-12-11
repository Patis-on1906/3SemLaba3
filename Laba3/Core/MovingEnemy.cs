using System;
using System.Text.Json;

namespace Laba3
{
    // Подвижный враг, который преследует игрока

    public class MovingEnemy : MovingUnit, IEnemy
    {
       
        // Урон, наносимый врагом
        
        public int Damage { get; set; } = 10;

        
        // Дистанция атаки
       
        public int AttackRange { get; set; } = 1;

      
 
          // Скорость движения    
        public int MoveSpeed { get; set; } = 1;

        private int _moveCounter = 0;
        private Random _random = new Random();

       
        // Символ для отображения
       
        public override char Symbol => 'E';

   
        // Обновление состояния врага
      
        public void Update(IMapCollision map, IPlayerLocator playerLocator)
        {
            // Проверяем, может ли враг атаковать
            if (IsPlayerInRange(playerLocator.PlayerX, playerLocator.PlayerY))
            {
                // Атака обрабатывается в GameController
                return;
            }

            // Двигаемся только с определенной скоростью
            _moveCounter++;
            if (_moveCounter < MoveSpeed)
                return;

            _moveCounter = 0;

            // Пытаемся двигаться к игроку
            MoveTowardsPlayer(map, playerLocator);
        }


        // Двигается в сторону игрока с проверкой препятствий
      
        private void MoveTowardsPlayer(IMapCollision map, IPlayerLocator playerLocator)
        {
            int playerX = playerLocator.PlayerX;
            int playerY = playerLocator.PlayerY;

            // Определяем направление к игроку
            int dx = 0, dy = 0;

            if (Math.Abs(X - playerX) > Math.Abs(Y - playerY))
            {
                // Пытаемся двигаться по X
                if (X < playerX) dx = 1;
                else if (X > playerX) dx = -1;
            }
            else
            {
                // Пытаемся двигаться по Y
                if (Y < playerY) dy = 1;
                else if (Y > playerY) dy = -1;
            }

            // Пробуем основное направление
            if (TryMove(map, dx, dy, playerLocator))
                return;

            // Если не получилось, пробуем альтернативное направление
            if (dx != 0)
            {
                // Пробуем двигаться по Y
                if (TryMove(map, 0, 1, playerLocator) || TryMove(map, 0, -1, playerLocator))
                    return;
            }
            else if (dy != 0)
            {
                // Пробуем двигаться по X
                if (TryMove(map, 1, 0, playerLocator) || TryMove(map, -1, 0, playerLocator))
                    return;
            }

            // Если все направления заблокированы, остаемся на месте
        }

 
        // Пытается переместиться в указанном направлении
        
        private bool TryMove(IMapCollision map, int dx, int dy, IPlayerLocator playerLocator)
        {
            int newX = X + dx;
            int newY = Y + dy;

            // Проверяем, можно ли пройти в эту клетку
            if (map.IsWalkable(newX, newY))
            {
                // Проверяем, не стоит ли там игрок или другой враг
                // (эта проверка должна быть в GameController.IsCellFree)
                X = newX;
                Y = newY;
                return true;
            }

            return false;
        }

      
        // Проверяет, находится ли игрок в зоне атаки
      
        public bool IsPlayerInRange(int playerX, int playerY)
        {
            // расстояние (по горизонтали и вертикали)
            int distance = Math.Abs(X - playerX) + Math.Abs(Y - playerY);
            return distance <= AttackRange;
        }

    
        // Наносит урон игроку
  
        public void AttackPlayer(Player player)
        {
            player.TakeDamage(Damage);
        }

        
        // Случайное движение (используется при потере игрока)
      
        private void RandomMove(IMapCollision map, IPlayerLocator playerLocator)
        {
            // Случайное направление
            int[,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            int index = _random.Next(0, 4);

            for (int i = 0; i < 4; i++)
            {
                int dirIndex = (index + i) % 4;
                int dx = directions[dirIndex, 0];
                int dy = directions[dirIndex, 1];

                if (TryMove(map, dx, dy, playerLocator))
                    break;
            }
        }

        
        // Сериализация врага
       
        public string Serialize() => JsonSerializer.Serialize(this);

    
        // Десериализация врага
        
        public void Deserialize(string json)
        {
            var obj = JsonSerializer.Deserialize<MovingEnemy>(json);
            if (obj == null)
                throw new ArgumentException("Invalid JSON for MovingEnemy");

            X = obj.X;
            Y = obj.Y;
            Damage = obj.Damage;
            AttackRange = obj.AttackRange;
            MoveSpeed = obj.MoveSpeed;
        }

    
        // Возвращает строковое представление врага
      
        public override string ToString()
        {
            return $"MovingEnemy at ({X},{Y}), Damage: {Damage}, Range: {AttackRange}";
        }
    }
}