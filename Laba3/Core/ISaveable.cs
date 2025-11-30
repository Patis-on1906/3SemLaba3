namespace Laba3;

public interface ISaveable
{
    string Serialize();
    void Deserialize(string json);
}