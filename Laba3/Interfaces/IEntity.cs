namespace Laba3;

public interface IEntity
{
    string Id { get; }
    int X { get; }
    int Y { get; }
    char Symbol { get; }
    EntityType EntityType { get; }
    bool IsPassable { get; }
}