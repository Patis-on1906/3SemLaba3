namespace Laba3;

public class EntityCollisionService : IEntityCollision
{
    private readonly IEntityRepository _repository;

    public EntityCollisionService(IEntityRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public bool HasEntityAt(int x, int y, IEntity exclude = null)
    {
        return _repository.GetAllEntities()
            .Any(e => e.X == x && e.Y == y && e != exclude && !e.IsPassable);
    }

    public IEntity GetEntityAt(int x, int y)
    {
        return _repository.GetAllEntities()
            .FirstOrDefault(e => e.X == x && e.Y == y);
    }
}