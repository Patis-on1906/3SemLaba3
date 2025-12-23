using System;
using System.Text.Json;

namespace Laba3
{
    public class MovingEnemy : MovingUnit, IUpdatable
    {
        private static readonly Random _random = new Random();

        public override char Symbol => 'M';
        public override EntityType EntityType => EntityType.MovingEnemy;

        public int Damage { get; set; } = 10;
        public int MoveSpeed { get; set; } = 2;
        private int _moveCounter = 0;

        public void Update(IMapCollision map, IPlayerLocator playerLocator, IEntityCollision entities)
        {
            // FIX LOGIC: Если игрок далеко (>10 клеток), враг не реагирует
            int dist = Math.Abs(playerLocator.PlayerX - X) + Math.Abs(playerLocator.PlayerY - Y);
            if (dist > 10) return;

            // FIX LOGIC: 20% шанс протупить, чтобы игрок мог убежать
            if (_random.NextDouble() < 0.2) return;

            // Атака
            if (Math.Abs(playerLocator.PlayerX - X) <= 1 && Math.Abs(playerLocator.PlayerY - Y) <= 1)
            {
                if (playerLocator.Player is IDamageable p) p.TakeDamage(Damage);
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

            // Попытка пойти по одной из осей
            if (dx != 0 && Move(dx, 0, map, entities)) return;
            if (dy != 0 && Move(0, dy, map, entities)) return;
        }
    }
}