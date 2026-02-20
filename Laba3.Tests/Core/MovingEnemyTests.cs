using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Numerics;

namespace Laba3.Tests.Entities
{
    [TestClass]
    public class MovingEnemyTests
    {
        [TestMethod]
        public void Constructor_DefaultValues_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var enemy = new MovingEnemy(5, 5);

            // Assert
            Assert.AreEqual(5, enemy.X);
            Assert.AreEqual(5, enemy.Y);
            Assert.AreEqual(10, enemy.Damage);
            Assert.AreEqual(2, enemy.MoveSpeed);
            Assert.AreEqual('M', enemy.Symbol);
            Assert.AreEqual(EntityType.MovingEnemy, enemy.EntityType);
            Assert.IsFalse(enemy.IsPassable);
        }

        [TestMethod]
        public void Constructor_CustomValues_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var enemy = new MovingEnemy(5, 5, 15, 3);

            // Assert
            Assert.AreEqual(5, enemy.X);
            Assert.AreEqual(5, enemy.Y);
            Assert.AreEqual(15, enemy.Damage);
            Assert.AreEqual(3, enemy.MoveSpeed);
        }

        [TestMethod]
        public void Update_MoveCounterLessThanSpeed_DoesNotMove()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);
            var enemy = new MovingEnemy(5, 5, 10, 3);
            enemy.MoveCounter = 1;
            var player = new Player(8, 8);

            var entities = new EntityRepository();
            entities.SetPlayer(player);

            var gameStateMock = new Mock<IGameState>();
            gameStateMock.Setup(g => g.Player).Returns(player);
            gameStateMock.Setup(g => g.PlayerX).Returns(8);
            gameStateMock.Setup(g => g.PlayerY).Returns(8);
            gameStateMock.Setup(g => g.EntityRepository).Returns(entities);

            // Act
            enemy.Update(map, gameStateMock.Object);

            // Assert
            Assert.AreEqual(2, enemy.MoveCounter);
            Assert.AreEqual(5, enemy.X);
            Assert.AreEqual(5, enemy.Y);
        }
    }
}