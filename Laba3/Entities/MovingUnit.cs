using System.Text.Json.Serialization;

namespace Laba3;

public abstract class MovingUnit : IEntity, IMoveable 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int X { get; set; }
    public int Y { get; set; }
    
    [JsonIgnore]
    public abstract char Symbol { get; }
    
    [JsonIgnore]
    public abstract EntityType EntityType { get; }
    
    public bool Move(int dx, int dy, IMapCollision map, IEntityCollision entities)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (!map.IsWalkable(newX, newY)) return false;
        if (!entities.HasEntityAt(newX, newY, this)) return false;
        
        X = newX;
        Y = newY;
        return true;
    }
}