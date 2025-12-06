using System.Text.Json;

namespace Laba3;

public class MovingEnemy : MovingUnit, ISaveable, IUpdatable
{
    public override char Symbol => 'E';

    public void Update(IMapCollision map, IPlayerLocator playerLocator)
    {
        int dx = 0;
        int dy = 0;

        if (playerLocator.PlayerX > X) dx = 1;
        else if (playerLocator.PlayerX < X) dx = -1;
        
        if (playerLocator.PlayerY > Y) dy = 1;
        else if (playerLocator.PlayerY < Y) dy = -1;
        
        Move(dx, 0, map);
        if (dx == 0) Move(0, dy, map);
    }

    public string Serialize() => JsonSerializer.Serialize(this);

    public void Deserialize(string json)
    {
        var obj = JsonSerializer.Deserialize<MovingEnemy>(json);
        if (obj == null)
            throw new ArgumentException("Invalid JSON for MovingEnemy");
            
        X = obj.X;
        Y = obj.Y;
    }
}