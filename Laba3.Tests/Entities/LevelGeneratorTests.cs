using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Laba3.Tests.Core
{
    [TestClass]
    public class LevelGeneratorTests
    {
        [TestMethod]
        public void CreateRandomLevel_ValidDimensions_ReturnsGameState()
        {
            // Arrange
            var generator = new LevelGenerator();

            // Act
            var state = generator.CreateRandomLevel(46, 21);

            // Assert
            Assert.IsNotNull(state);
            Assert.IsNotNull(state.Map);
            Assert.IsNotNull(state.EntityRepository);
            Assert.AreEqual(46, state.Map.Width);
            Assert.AreEqual(21, state.Map.Height);
        }

        [TestMethod]
        public void CreateRandomLevel_PlayerPlaced_PlayerExists()
        {
            // Arrange
            var generator = new LevelGenerator();

            // Act
            var state = generator.CreateRandomLevel(46, 21);

            // Assert
            Assert.IsNotNull(state.Player);
            Assert.IsTrue(state.Map.IsWalkable(state.Player.X, state.Player.Y));
        }

        [TestMethod]
        public void CreateRandomLevel_TreasuresPlaced_TreasuresExist()
        {
            // Arrange
            var generator = new LevelGenerator();

            // Act
            var state = generator.CreateRandomLevel(46, 21);

            // Assert
            Assert.IsTrue(state.EntityRepository.Treasures.Count > 0);
            Assert.IsTrue(state.EntityRepository.Treasures.Count >= 3);
            Assert.IsTrue(state.EntityRepository.Treasures.Count <= 8);

            foreach (var treasure in state.EntityRepository.Treasures)
            {
                Assert.IsTrue(state.Map.IsWalkable(treasure.X, treasure.Y));
                Assert.IsFalse(treasure.Collected);
            }
        }

        [TestMethod]
        public void CreateRandomLevel_MovingEnemiesPlaced_MovingEnemiesExist()
        {
            // Arrange
            var generator = new LevelGenerator();

            // Act
            var state = generator.CreateRandomLevel(46, 21);

            // Assert
            Assert.IsTrue(state.EntityRepository.MovingEnemies.Count > 0);
            Assert.IsTrue(state.EntityRepository.MovingEnemies.Count >= 2);
            Assert.IsTrue(state.EntityRepository.MovingEnemies.Count <= 3);

            foreach (var enemy in state.EntityRepository.MovingEnemies)
            {
                Assert.IsTrue(state.Map.IsWalkable(enemy.X, enemy.Y));
            }
        }

        [TestMethod]
        public void CreateRandomLevel_StaticEnemiesPlaced_StaticEnemiesExist()
        {
            // Arrange
            var generator = new LevelGenerator();

            // Act
            var state = generator.CreateRandomLevel(46, 21);

            // Assert
            Assert.IsTrue(state.EntityRepository.StaticEnemies.Count > 0);
            Assert.IsTrue(state.EntityRepository.StaticEnemies.Count >= 1);
            Assert.IsTrue(state.EntityRepository.StaticEnemies.Count <= 3);

            foreach (var enemy in state.EntityRepository.StaticEnemies)
            {
                Assert.IsTrue(state.Map.IsWalkable(enemy.X, enemy.Y));
            }
        }

        [TestMethod]
        public void CreateRandomLevel_EntitiesDontOverlap_NoTwoEntitiesOnSameCell()
        {
            // Arrange
            var generator = new LevelGenerator();

            // Act
            var state = generator.CreateRandomLevel(46, 21);

            // Get all positions
            var positions = new HashSet<(int x, int y)>();

            positions.Add((state.Player.X, state.Player.Y));

            foreach (var enemy in state.EntityRepository.MovingEnemies)
            {
                var pos = (enemy.X, enemy.Y);
                Assert.IsFalse(positions.Contains(pos));
                positions.Add(pos);
            }

            foreach (var enemy in state.EntityRepository.StaticEnemies)
            {
                var pos = (enemy.X, enemy.Y);
                Assert.IsFalse(positions.Contains(pos));
                positions.Add(pos);
            }

            foreach (var treasure in state.EntityRepository.Treasures)
            {
                var pos = (treasure.X, treasure.Y);
                Assert.IsFalse(positions.Contains(pos));
                positions.Add(pos);
            }
        }
    }
}