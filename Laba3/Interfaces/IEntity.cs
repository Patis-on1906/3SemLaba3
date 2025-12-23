namespace Laba3;

public interface IEntity
{
    int X { get; set; }
    int Y { get; set; }
    char Symbol { get; }
    EntityType EntityType { get; }
    bool IsPassable { get; }
}