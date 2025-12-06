namespace Laba3;
using System.Text.Json;

public class Player : MovingUnit, ISaveable
{
    public override char Symbol => 'P';
    
    public string Serialize() => JsonSerializer.Serialize(this);

    public void Deserialize(string json)
    {
        var obj = JsonSerializer.Deserialize<Player>(json);
        if (obj == null) 
            throw new ArgumentException("Invalid JSON for Player");
        
        X = obj.X;
        Y = obj.Y;
    }
}