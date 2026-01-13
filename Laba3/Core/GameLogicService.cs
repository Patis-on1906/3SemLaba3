namespace Laba3;

public class GameLogicService : IGameLogicService
{
    private readonly IEntityFactory _entityFactory;
    
    public GameLogicService(IEntityFactory entityFactory = null)
    {
        _entityFactory = entityFactory ?? new EntityFactory();
    }
    
    public void ProcessPlayerMovement(GameState state, int dx, int dy)
    {
        if (state.Player == null) return;
        
        var collisionDetector = new EntityCollisionService(state.EntityRepository);
        
        if (state.Player.TryMove(dx, dy, state.Map, collisionDetector))
        {
            CollectTreasuresAtPosition(state, state.Player.X, state.Player.Y);
        }
    }
    
    public void CollectTreasuresAtPosition(GameState state, int x, int y)
    {
        var treasures = state.EntityRepository.Treasures
            .Where(t => !t.Collected && t.X == x && t.Y == y)
            .ToList();
            
        foreach (var treasure in treasures)
        {
            treasure.Collect(state.Player);
        }
    }
    
    public void UpdateWorld(GameState state)
    {
        var collisionDetector = new EntityCollisionService(state.EntityRepository);
        
        foreach (var entity in state.EntityRepository.GetUpdatableEntities())
        {
            entity.Update(state.Map, state, collisionDetector);
        }
    }
    
    public void CheckGameOver(GameState state)
    {
        if (state.Player?.IsAlive == false)
        {
            throw new GameOverException("Игрок погиб");
        }
    }
}

public class GameOverException : Exception
{
    public GameOverException(string message) : base(message) { }
}
public class GameVictoryException : Exception
{
    public GameVictoryException() : base("Победа!") { }
}