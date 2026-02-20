using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace Laba3.Tests.Entities
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Constructor_DefaultValues_HealthAndScoreSetCorrectly()
        {
            // Arrange & Act
            var player = new Player(5, 5);

            // Assert
            Assert.AreEqual(5, player.X);
            Assert.AreEqual(5, player.Y);
            Assert.AreEqual(100, player.MaxHealth);
            Assert.AreEqual(100, player.Health);
            Assert.AreEqual(0, player.Score);
            Assert.IsTrue(player.IsAlive);
            Assert.AreEqual('P', player.Symbol);
            Assert.AreEqual(EntityType.Player, player.EntityType);
            Assert.IsFalse(player.IsPassable);
        }

        [TestMethod]
        public void Constructor_CustomHealth_HealthSetCorrectly()
        {
            // Arrange & Act
            var player = new Player(5, 5, 50);

            // Assert
            Assert.AreEqual(50, player.MaxHealth);
            Assert.AreEqual(50, player.Health);
        }

        [TestMethod]
        public void TakeDamage_ValidDamage_HealthDecreases()
        {
            // Arrange
            var player = new Player(5, 5, 100);

            // Act
            player.TakeDamage(30);

            // Assert
            Assert.AreEqual(70, player.Health);
            Assert.IsTrue(player.IsAlive);
        }

        [TestMethod]
        public void TakeDamage_DamageMoreThanHealth_HealthBecomesZero()
        {
            // Arrange
            var player = new Player(5, 5, 100);

            // Act
            player.TakeDamage(150);

            // Assert
            Assert.AreEqual(0, player.Health);
            Assert.IsFalse(player.IsAlive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TakeDamage_NegativeDamage_ThrowsArgumentException()
        {
            // Arrange
            var player = new Player(5, 5);

            // Act
            player.TakeDamage(-10);
        }

        [TestMethod]
        public void Heal_ValidAmount_HealthIncreases()
        {
            // Arrange
            var player = new Player(5, 5, 100);
            player.TakeDamage(50);

            // Act
            player.Heal(30);

            // Assert
            Assert.AreEqual(80, player.Health);
        }

        [TestMethod]
        public void Heal_AmountExceedsMaxHealth_HealthCappedAtMax()
        {
            // Arrange
            var player = new Player(5, 5, 100);
            player.TakeDamage(20);

            // Act
            player.Heal(50);

            // Assert
            Assert.AreEqual(100, player.Health);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Heal_NegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            var player = new Player(5, 5);

            // Act
            player.Heal(-10);
        }

        [TestMethod]
        public void AddScore_ValidPoints_ScoreIncreases()
        {
            // Arrange
            var player = new Player(5, 5);

            // Act
            player.AddScore(50);

            // Assert
            Assert.AreEqual(50, player.Score);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddScore_NegativePoints_ThrowsArgumentException()
        {
            // Arrange
            var player = new Player(5, 5);

            // Act
            player.AddScore(-10);
        }

        [TestMethod]
        public void TryMove_ToWalkableCellWithoutEntity_MovesSuccessfully()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);
            var entities = new EntityRepository();
            var player = new Player(5, 5);
            entities.SetPlayer(player);

            // Act
            var result = player.TryMove(1, 0, map, entities);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(6, player.X);
            Assert.AreEqual(5, player.Y);
        }

        [TestMethod]
        public void TryMove_ToWall_DoesNotMove()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);
            map.SetCellType(6, 5, Cell.CellType.Wall);
            var entities = new EntityRepository();
            var player = new Player(5, 5);
            entities.SetPlayer(player);

            // Act
            var result = player.TryMove(1, 0, map, entities);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(5, player.X);
            Assert.AreEqual(5, player.Y);
        }

        [TestMethod]
        public void TryMove_ToCellWithNonPassableEntity_DoesNotMove()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);
            var entities = new EntityRepository();
            var player = new Player(5, 5);
            var enemy = new MovingEnemy(6, 5);

            entities.SetPlayer(player);
            entities.AddMovingEnemy(enemy);

            // Act
            var result = player.TryMove(1, 0, map, entities);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(5, player.X);
            Assert.AreEqual(5, player.Y);
        }

        [TestMethod]
        public void TryMove_ToCellWithPassableEntity_MovesSuccessfully()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);
            var entities = new EntityRepository();
            var player = new Player(5, 5);
            var treasure = new Treasure(6, 5);

            entities.SetPlayer(player);
            entities.AddTreasure(treasure);

            // Act
            var result = player.TryMove(1, 0, map, entities);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(6, player.X);
            Assert.AreEqual(5, player.Y);
        }
    }
}