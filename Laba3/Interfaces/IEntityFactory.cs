namespace Laba3;

public interface IEntityFactory
{
    Player CreatePlayer(int x, int y, int health = 100);
    MovingEnemy CreateMovingEnemy(int x, int y, int damage = 10);
    StaticEnemy CreateStaticEnemy(int x, int y, int damage = 15, int attackRange = 2);
    Treasure CreateTreasure(int x, int y, int value = 10);
}
