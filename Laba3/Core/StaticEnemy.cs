using System.Text.Json;
namespace Laba3;

public class StaticEnemy : IEntity, ISaveable, IUpdatable
{
    public int X { get; set; }
    public int Y { get; set; }                                                                                                                                                 
    public char Symbol => 'S';
    
    public void Update(IMapCollision map, IPlayerLocator playerLocator)
    {
        if (Math.Abs(playerLocator.PlayerX - X) <= 1 && Math.Abs(playerLocator.PlayerY - Y) <= 1)
        {
            Console.WriteLine("Static Enemy attacks you!");
            // логика атаки (отнимаем жизни)
        }
    }
    
    public string Serialize() => JsonSerializer.Serialize(this);

    public void Deserialize(string json)
    {
        var obj = JsonSerializer.Deserialize<StaticEnemy>(json);
        if (obj == null) 
            throw new ArgumentException("Invalid JSON for StaticEnemy");
        
        X = obj.X;
        Y = obj.Y;
    }
}