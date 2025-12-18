namespace Laba3;

public interface IDamageable
{
    int Health { get; set; }
    int MaxHealth { get; }
    bool IsAlive { get; }
    void TakeDamage(int damage);
}