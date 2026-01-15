using System;
using System.Text.Json.Serialization;

namespace Laba3;

public class Treasure : BaseEntity
{
    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("collected")]
    public bool Collected { get; set; }

    [JsonIgnore]
    public override char Symbol => 'T';
    
    [JsonIgnore]
    public override EntityType EntityType => EntityType.Treasure;
    
    [JsonIgnore]
    public override bool IsPassable => true;

    [JsonConstructor]
    public Treasure(string id, int x, int y, int value, bool collected) : base()
    {
        Id = id;
        X = x;
        Y = y;
        Value = Math.Max(value, 0);
        Collected = collected;
    }

    public Treasure() : base()
    {
        Value = 10;
        Collected = false;
    }

    public Treasure(int x, int y, int value = 10) : base(x, y)
    {
        Value = value;
        Collected = false;
    }

    public void Collect(Player player)
    {
        if (!Collected && player != null)
        {
            Collected = true;
            player.AddScore(Value);
        }
    }
}