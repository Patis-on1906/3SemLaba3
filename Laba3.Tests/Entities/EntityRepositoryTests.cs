using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Numerics;

namespace Laba3.Tests.Core
{
    [TestClass]
    public class EntityRepositoryTests
    {
        [TestMethod]
        public void Constructor_Empty_CreatesEmptyLists()
        {
            // Arrange & Act
            var repo = new EntityRepository();

            // Assert
            Assert.IsNull(repo.Player);
            Assert.AreEqual(0, repo.MovingEnemies.Count);
            Assert.AreEqual(0, repo.StaticEnemies.Count);
            Assert.AreEqual(0, repo.Treasures.Count);
        }

        [TestMethod]
        public void SetPlayer_ValidPlayer_SetsPlayer()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);

            // Act
            repo.SetPlayer(player);

            // Assert
            Assert.AreEqual(player, repo.Player);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetPlayer_NullPlayer_ThrowsArgumentNullException()
        {
            // Arrange
            var repo = new EntityRepository();

            // Act
            repo.SetPlayer(null);
        }

        [TestMethod]
        public void AddMovingEnemy_ValidEnemy_AddsToList()
        {
            // Arrange
            var repo = new EntityRepository();
            var enemy = new MovingEnemy(5, 5);

            // Act
            repo.AddMovingEnemy(enemy);

            // Assert
            Assert.AreEqual(1, repo.MovingEnemies.Count);
            Assert.AreEqual(enemy, repo.MovingEnemies[0]);
        }

        [TestMethod]
        public void AddStaticEnemy_ValidEnemy_AddsToList()
        {
            // Arrange
            var repo = new EntityRepository();
            var enemy = new StaticEnemy(5, 5);

            // Act
            repo.AddStaticEnemy(enemy);

            // Assert
            Assert.AreEqual(1, repo.StaticEnemies.Count);
            Assert.AreEqual(enemy, repo.StaticEnemies[0]);
        }

        [TestMethod]
        public void AddTreasure_ValidTreasure_AddsToList()
        {
            // Arrange
            var repo = new EntityRepository();
            var treasure = new Treasure(5, 5);

            // Act
            repo.AddTreasure(treasure);

            // Assert
            Assert.AreEqual(1, repo.Treasures.Count);
            Assert.AreEqual(treasure, repo.Treasures[0]);
        }

        [TestMethod]
        public void RemoveTreasure_ExistingTreasure_RemovesAndReturnsTrue()
        {
            // Arrange
            var repo = new EntityRepository();
            var treasure = new Treasure(5, 5);
            repo.AddTreasure(treasure);
            var id = treasure.Id;

            // Act
            var result = repo.RemoveTreasure(id);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, repo.Treasures.Count);
        }

        [TestMethod]
        public void RemoveTreasure_NonExistingTreasure_ReturnsFalse()
        {
            // Arrange
            var repo = new EntityRepository();

            // Act
            var result = repo.RemoveTreasure("non-existing-id");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasEntityAt_OccupiedCell_ReturnsTrue()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);
            repo.SetPlayer(player);
            var enemy = new MovingEnemy(6, 5);
            repo.AddMovingEnemy(enemy);

            // Act & Assert
            Assert.IsTrue(repo.HasEntityAt(5, 5));
            Assert.IsTrue(repo.HasEntityAt(6, 5));
        }

        [TestMethod]
        public void HasEntityAt_EmptyCell_ReturnsFalse()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);
            repo.SetPlayer(player);

            // Act & Assert
            Assert.IsFalse(repo.HasEntityAt(6, 5));
        }

        [TestMethod]
        public void HasEntityAt_WithExclude_ExcludesSpecifiedEntity()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);
            repo.SetPlayer(player);

            // Act & Assert
            Assert.IsFalse(repo.HasEntityAt(5, 5, player));
        }

        [TestMethod]
        public void GetEntityAt_OccupiedCell_ReturnsEntity()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);
            repo.SetPlayer(player);

            // Act
            var entity = repo.GetEntityAt(5, 5);

            // Assert
            Assert.AreEqual(player, entity);
        }

        [TestMethod]
        public void GetEntityAt_EmptyCell_ReturnsNull()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);
            repo.SetPlayer(player);

            // Act
            var entity = repo.GetEntityAt(6, 5);

            // Assert
            Assert.IsNull(entity);
        }

        [TestMethod]
        public void GetAllEntities_ReturnsAllEntities()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);
            var movingEnemy = new MovingEnemy(6, 5);
            var staticEnemy = new StaticEnemy(7, 5);
            var treasure = new Treasure(8, 5);

            repo.SetPlayer(player);
            repo.AddMovingEnemy(movingEnemy);
            repo.AddStaticEnemy(staticEnemy);
            repo.AddTreasure(treasure);

            // Act
            var entities = repo.GetAllEntities().ToList();

            // Assert
            Assert.AreEqual(4, entities.Count);
            CollectionAssert.Contains(entities, player);
            CollectionAssert.Contains(entities, movingEnemy);
            CollectionAssert.Contains(entities, staticEnemy);
            CollectionAssert.Contains(entities, treasure);
        }

        [TestMethod]
        public void GetUpdatableEntities_ReturnsOnlyUpdatableEntities()
        {
            // Arrange
            var repo = new EntityRepository();
            var player = new Player(5, 5);
            var movingEnemy = new MovingEnemy(6, 5);
            var staticEnemy = new StaticEnemy(7, 5);
            var treasure = new Treasure(8, 5);

            repo.SetPlayer(player);
            repo.AddMovingEnemy(movingEnemy);
            repo.AddStaticEnemy(staticEnemy);
            repo.AddTreasure(treasure);

            // Act
            var updatables = repo.GetUpdatableEntities().ToList();

            // Assert
            Assert.AreEqual(2, updatables.Count);
            CollectionAssert.Contains(updatables, movingEnemy);
            CollectionAssert.Contains(updatables, staticEnemy);
            CollectionAssert.DoesNotContain(updatables, player);
            CollectionAssert.DoesNotContain(updatables, treasure);
        }
    }
}