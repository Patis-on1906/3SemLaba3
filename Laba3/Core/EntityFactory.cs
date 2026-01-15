namespace Laba3;

public class EntityFactory : IEntityFactory
{
    public Player CreatePlayer(int x, int y, int health = 100)
    {
        return new Player(x, y, health);
    }

    public MovingEnemy CreateMovingEnemy(int x, int y, int damage = 10)
    {
        return new MovingEnemy(x, y, damage, 6);
    }

    public StaticEnemy CreateStaticEnemy(int x, int y, int damage = 15, int attackRange = 2, int attackCooldown = 3)
    {
        return new StaticEnemy(x, y, damage, attackRange, attackCooldown);
    }

    public Treasure CreateTreasure(int x, int y, int value = 10)
    {
        return new Treasure(x, y, value);
    }
}