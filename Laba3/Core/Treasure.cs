using System.Text.Json;

namespace Laba3;

public class Treasure : IEntity, ISaveable
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol => 'T';
    public bool Collected { get; set; } = false;
    
    public string Serialize() => JsonSerializer.Serialize(this);

    public void Deserialize(string json)
    {
        var obj = JsonSerializer.Deserialize<Treasure>(json);
        if (obj == null) 
            throw new ArgumentException("Invalid JSON for Treasure");
        
        X = obj.X;
        Y = obj.Y;
    }
}