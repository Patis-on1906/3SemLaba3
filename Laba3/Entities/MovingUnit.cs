using System.Text.Json.Serialization;

namespace Laba3;

public abstract class MovingUnit : BaseEntity, IMoveable 
{
    [JsonConstructor]
    protected MovingUnit() : base() { }
    
    protected MovingUnit(int x, int y) : base(x, y) { }
    
    public bool TryMove(int dx, int dy, IMapCollision map, IEntityCollision entities)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (!map.IsWalkable(newX, newY)) return false;
        if (entities.HasEntityAt(newX, newY, this)) return false;
        
        SetPosition(newX, newY);
        return true;
    }
}