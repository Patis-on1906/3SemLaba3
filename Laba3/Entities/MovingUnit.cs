using System.Text.Json.Serialization;

namespace Laba3;

public abstract class MovingUnit : BaseEntity, IMoveable
{
    [JsonConstructor]
    protected MovingUnit() : base() { }

    protected MovingUnit(int x, int y) : base(x, y) { }

    public virtual bool TryMove(int dx, int dy, IMapCollision map, IEntityCollision entities)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (!map.IsWalkable(newX, newY)) return false;

        // Получаем сущность на целевой клетке
        var entityAtTarget = entities.GetEntityAt(newX, newY);

        // Если там есть сущность и она непроходима - не можем двигаться
        if (entityAtTarget != null && !entityAtTarget.IsPassable)
            return false;

        SetPosition(newX, newY);
        return true;
    }
}