namespace Laba3;

public interface IRepository<T>
{
    T GetData();
    void SetData(T data);
    void Load();
    void Save();
}