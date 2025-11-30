namespace Laba3;

public class Player : IMoveable, ISaveable
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol => 'P';

    public void Move(int dx, int dy, Map map) {}
    public string Serialize() => throw new NotImplementedException();
    public void Deserialize(string json) => throw new NotImplementedException();
}