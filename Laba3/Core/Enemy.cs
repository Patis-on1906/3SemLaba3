namespace Laba3;

public class Enemy : IEntity, ISaveable
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol => 'E';
    
    public string Serialize() => throw new NotImplementedException();
    public void Deserialize(string json) => throw new NotImplementedException();
}