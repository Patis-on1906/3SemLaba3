using System.Text.Json;

namespace Laba3;

public class Treasure : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol => 'T';
    public EntityType EntityType => EntityType.Treasure;
    public bool IsPassable => true;
    
    public int Value { get; set; } = 10;
    public bool Collected { get; set; } = false;
    
    public void Collect(Player player)
    {
        if (!Collected)
        {
            Collected = true;
            player.AddScore(Value);
        }
    }
}