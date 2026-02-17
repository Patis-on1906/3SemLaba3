namespace Laba3;

public class LevelGenerator : ILevelGenerator
{
    private readonly IEntityFactory _entityFactory;
    private readonly MazeGenerator _mazeGenerator;

    public LevelGenerator(IEntityFactory entityFactory = null)
    {
        _entityFactory = entityFactory ?? new EntityFactory();
        _mazeGenerator = new MazeGenerator(_entityFactory);
    }

    public GameState CreateRandomLevel(int width, int height)
    {
        return _mazeGenerator.CreateRandomMazeLevel(width, height);
    }
}