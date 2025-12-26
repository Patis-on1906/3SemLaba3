namespace Laba3;

public interface IDamageable
{
    int Health { get; }
    int MaxHealth { get; }
    bool IsAlive { get; }
    void TakeDamage(int damage);
    void Heal(int amount);
}
