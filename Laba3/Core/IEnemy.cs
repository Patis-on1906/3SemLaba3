using System.Text.Json;

namespace Laba3
{
  
    // Интерфейс для всех врагов в игре

    public interface IEnemy : IEntity, IUpdatable, ISaveable
    {
  
        // Урон, наносимый врагом
  
        int Damage { get; }

        // Дистанция атаки врага
       
        int AttackRange { get; }

        /// Проверяет, находится ли игрок в зоне атаки
      
        bool IsPlayerInRange(int playerX, int playerY);

        /// Наносит урон игроку
       
        void AttackPlayer(Player player);
    }
}