using System.Text.Json.Serialization;

namespace Laba3;

public class Treasure : BaseEntity
{
    private int _value;
    private bool _collected;

    public override char Symbol => 'T';
    public override EntityType EntityType => EntityType.Treasure;
    public override bool IsPassable => true;

    public int Value
    {
        get => _value;
        set
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative", nameof(value));
            _value = value;
        }
    }

    public bool Collected
    {
        get => _collected;
        private set => _collected = value;
    }

    [JsonConstructor]
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