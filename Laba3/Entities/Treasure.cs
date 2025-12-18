using System.Text.Json;

namespace Laba3;

public class Treasure : IEntity, ISaveable
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol => 'T';
    public EntityType EntityType => EntityType.Treasure;
    
    public int Value { get; } = 10;
    public bool Collected { get; set; } = false;
    
    public void Collect(Player player)
    {
        if (!Collected)
        {
            Collected = true;
            player.AddScore(Value);
        }
    }
    
    public string Serialize() => JsonSerializer.Serialize(this);

    public static Treasure Deserialize(string json)
    {
        var obj = JsonSerializer.Deserialize<Treasure>(json);
        if (obj == null) 
            throw new ArgumentException("Invalid JSON for Treasure");

        return obj;
    }
}