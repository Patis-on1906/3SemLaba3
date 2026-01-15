using System;
using System.Text.Json.Serialization;

namespace Laba3;

public abstract class BaseEntity : IEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [JsonPropertyName("x")]
    public int X { get; set; }
    
    [JsonPropertyName("y")]
    public int Y { get; set; }
    
    [JsonIgnore]
    public abstract char Symbol { get; }
    
    [JsonIgnore]
    public abstract EntityType EntityType { get; }
    
    [JsonIgnore]
    public abstract bool IsPassable { get; }

    protected BaseEntity() { }

    protected BaseEntity(int x, int y)
    {
        X = Math.Max(x, 0);
        Y = Math.Max(y, 0);
    }

    protected void SetPosition(int x, int y)
    {
        X = Math.Max(x, 0);
        Y = Math.Max(y, 0);
    }
}