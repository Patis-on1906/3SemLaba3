using System.Text.Json.Serialization;
using System.Text.Json;

namespace Laba3
{
    public class Player : MovingUnit, IDamageable
    {
        public override char Symbol => 'P';
        public override EntityType EntityType => EntityType.Player;
        
        public int Health { get; set; } = 100;
        public int MaxHealth { get; set; } = 100;
        public int Score { get; set; } = 0;
        
        [JsonIgnore] public bool IsAlive => Health > 0;
       
        public void TakeDamage(int damage)
        {
            Health = Math.Max(Health - damage, 0);
        }
    
        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, MaxHealth);
        }
        
        public void AddScore(int points)
        {
            Score += points;
        }
    }
}
