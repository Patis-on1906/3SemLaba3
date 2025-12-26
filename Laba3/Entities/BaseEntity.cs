using System.Text.Json.Serialization;

namespace Laba3;

public abstract class BaseEntity : IEntity
{
    private int _x;
    private int _y;
    
    public string Id { get; } = Guid.NewGuid().ToString();
    
    public int X 
    { 
        get => _x;
        protected set => _x = Math.Max(value, 0);
    }
    
    public int Y 
    { 
        get => _y;
        protected set => _y = Math.Max(value, 0);
    }
    
    [JsonIgnore]
    public abstract char Symbol { get; }
    
    [JsonIgnore]
    public abstract EntityType EntityType { get; }
    
    [JsonIgnore]
    public abstract bool IsPassable { get; }

    protected BaseEntity() { }

    protected BaseEntity(int x, int y)
    {
        X = x;
        Y = y;
    }

    protected void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}