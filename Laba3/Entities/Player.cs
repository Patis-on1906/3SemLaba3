using System;
using System.Text.Json.Serialization;

namespace Laba3
{
    public class Player : MovingUnit, IDamageable
    {
        [JsonPropertyName("health")]
        public int Health { get; set; }

        [JsonPropertyName("maxHealth")]
        public int MaxHealth { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonIgnore]
        public override char Symbol => 'P';
        
        [JsonIgnore]
        public override EntityType EntityType => EntityType.Player;
        
        [JsonIgnore]
        public override bool IsPassable => false;

        [JsonIgnore]
        public bool IsAlive => Health > 0;

        [JsonConstructor]
        public Player(string id, int x, int y, int health, int maxHealth, int score) : base()
        {
            Id = id;
            X = x;
            Y = y;
            MaxHealth = maxHealth > 0 ? maxHealth : 100;
            Health = Math.Clamp(health, 0, MaxHealth);
            Score = Math.Max(score, 0);
        }

        public Player() : base()
        {
            MaxHealth = 100;
            Health = MaxHealth;
            Score = 0;
        }

        public Player(int x, int y, int health = 100) : base(x, y)
        {
            MaxHealth = health;
            Health = MaxHealth;
            Score = 0;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException("Damage cannot be negative", nameof(damage));
            Health = Math.Max(0, Health - damage);
        }

        public void Heal(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Heal amount cannot be negative", nameof(amount));
            Health = Math.Min(MaxHealth, Health + amount);
        }

        public void AddScore(int points)
        {
            if (points < 0)
                throw new ArgumentException("Score points cannot be negative", nameof(points));
            Score += points;
        }
    }
}