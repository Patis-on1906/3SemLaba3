namespace Laba3;

public class Treasure : IEntity, ISaveable
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol => 'T';
    public bool Collected { get; set; } = false;
    
    public string Serialize() => throw new NotImplementedException();
    public void Deserialize(string json) => throw new NotImplementedException();
}