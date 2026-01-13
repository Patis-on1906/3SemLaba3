using System.Text.Json.Serialization;

namespace Laba3
{
    public class Player : MovingUnit, IDamageable
    {
        private int _health;
        private int _maxHealth;
        private int _score;

        public override char Symbol => 'P';
        public override EntityType EntityType => EntityType.Player;
        public override bool IsPassable => false;

        public int Health
        {
            get => _health;
            private set => _health = Math.Clamp(value, 0, MaxHealth);
        }

        public int MaxHealth
        {
            get => _maxHealth;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("MaxHealth must be positive", nameof(value));
                _maxHealth = value;
            }
        }

        public int Score
        {
            get => _score;
            private set => _score = Math.Max(value, 0);
        }

        [JsonIgnore]
        public bool IsAlive => Health > 0;

        [JsonConstructor]
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
            Health -= damage;
        }

        public void Heal(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Heal amount cannot be negative", nameof(amount));
            Health += amount;
        }

        public void AddScore(int points)
        {
            if (points < 0)
                throw new ArgumentException("Score points cannot be negative", nameof(points));
            Score += points;
        }
    }
}