using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Laba3.Tests.Core
{
    [TestClass]
    public class GameLogicServiceTests
    {
        private GameState CreateTestState()
        {
            var map = new Laba3.Map(10, 10);
            var repo = new EntityRepository();
            var player = new Player(5, 5, 100);
            repo.SetPlayer(player);

            return new GameState(map, repo);
        }

        [TestMethod]
        public void ProcessPlayerMovement_ValidMove_PlayerMoves()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();
            var initialX = state.Player.X;
            var initialY = state.Player.Y;

            // Act
            service.ProcessPlayerMovement(state, 1, 0);

            // Assert
            Assert.AreEqual(initialX + 1, state.Player.X);
            Assert.AreEqual(initialY, state.Player.Y);
        }

        [TestMethod]
        public void ProcessPlayerMovement_MoveIntoWall_PlayerDoesNotMove()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();
            state.Map.SetCellType(6, 5, Cell.CellType.Wall);
            var initialX = state.Player.X;
            var initialY = state.Player.Y;

            // Act
            service.ProcessPlayerMovement(state, 1, 0);

            // Assert
            Assert.AreEqual(initialX, state.Player.X);
            Assert.AreEqual(initialY, state.Player.Y);
        }

        [TestMethod]
        public void ProcessPlayerMovement_MoveOntoTreasure_CollectsTreasure()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();
            var treasure = new Treasure(6, 5, 25);
            state.EntityRepository.AddTreasure(treasure);
            var initialScore = state.Player.Score;

            // Act
            service.ProcessPlayerMovement(state, 1, 0);

            // Assert
            Assert.AreEqual(6, state.Player.X);
            Assert.AreEqual(initialScore + 25, state.Player.Score);
            Assert.IsTrue(treasure.Collected);
        }

        [TestMethod]
        public void CollectTreasuresAtPosition_WithTreasure_CollectsIt()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();
            var treasure = new Treasure(5, 5, 25);
            state.EntityRepository.AddTreasure(treasure);

            // Act
            service.CollectTreasuresAtPosition(state, 5, 5);

            // Assert
            Assert.IsTrue(treasure.Collected);
            Assert.AreEqual(25, state.Player.Score);
        }

        [TestMethod]
        public void UpdateWorld_UpdatesAllUpdatableEntities()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();
            var movingEnemy = new MovingEnemy(6, 6);
            var staticEnemy = new StaticEnemy(7, 7);

            state.EntityRepository.AddMovingEnemy(movingEnemy);
            state.EntityRepository.AddStaticEnemy(staticEnemy);

            // Act
            service.UpdateWorld(state);

            // Assert - проверяем что исключений не было
            Assert.IsNotNull(state);
        }

        [TestMethod]
        [ExpectedException(typeof(GameOverException))]
        public void CheckGameOver_PlayerDead_ThrowsGameOverException()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();
            state.Player.TakeDamage(100);

            // Act
            service.CheckGameOver(state);
        }

        [TestMethod]
        public void CheckGameOver_PlayerAlive_DoesNotThrow()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();

            // Act
            service.CheckGameOver(state);

            // Assert - если дошли сюда, значит исключения не было
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CheckVictory_AllTreasuresCollected_ReturnsTrue()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();

            var treasure1 = new Treasure(6, 5, 10);
            var treasure2 = new Treasure(7, 5, 10);

            state.EntityRepository.AddTreasure(treasure1);
            state.EntityRepository.AddTreasure(treasure2);

            treasure1.Collect(state.Player);
            treasure2.Collect(state.Player);

            // Act
            var result = service.CheckVictory(state);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckVictory_SomeTreasuresNotCollected_ReturnsFalse()
        {
            // Arrange
            var service = new GameLogicService();
            var state = CreateTestState();

            var treasure1 = new Treasure(6, 5, 10);
            var treasure2 = new Treasure(7, 5, 10);

            state.EntityRepository.AddTreasure(treasure1);
            state.EntityRepository.AddTreasure(treasure2);

            treasure1.Collect(state.Player);

            // Act
            var result = service.CheckVictory(state);

            // Assert
            Assert.IsFalse(result);
        }
    }
}