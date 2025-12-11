namespace Laba3
{
    using System.Text.Json;

    public class Player : MovingUnit, ISaveable
    {
   
        /// Здоровье игрока
       
        public int Health { get; set; } = 100;

        
        /// Максимальное здоровье
        
        public int MaxHealth { get; set; } = 100;

        public override char Symbol => 'P';

  
        /// Получает урон
       
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }

        
        /// Восстанавливает здоровье
     
    
        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }

        public string Serialize() => JsonSerializer.Serialize(this);

        public void Deserialize(string json)
        {
            var obj = JsonSerializer.Deserialize<Player>(json);
            if (obj == null)
                throw new ArgumentException("Invalid JSON for Player");

            X = obj.X;
            Y = obj.Y;
            Health = obj.Health;
            MaxHealth = obj.MaxHealth;
        }
    }
}
