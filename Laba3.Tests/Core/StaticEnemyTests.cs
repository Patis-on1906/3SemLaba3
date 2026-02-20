using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Laba3.Tests.Entities
{
    [TestClass]
    public class StaticEnemyTests
    {
        [TestMethod]
        public void Constructor_DefaultValues_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var enemy = new StaticEnemy(5, 5);

            // Assert
            Assert.AreEqual(5, enemy.X);
            Assert.AreEqual(5, enemy.Y);
            Assert.AreEqual(15, enemy.Damage);
            Assert.AreEqual(1, enemy.AttackRange);
            Assert.AreEqual(3, enemy.AttackCooldown);
            Assert.AreEqual('S', enemy.Symbol);
            Assert.AreEqual(EntityType.StaticEnemy, enemy.EntityType);
            Assert.IsFalse(enemy.IsPassable);
        }

        [TestMethod]
        public void Constructor_CustomValues_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var enemy = new StaticEnemy(5, 5, 20, 3, 2);

            // Assert
            Assert.AreEqual(5, enemy.X);
            Assert.AreEqual(5, enemy.Y);
            Assert.AreEqual(20, enemy.Damage);
            Assert.AreEqual(3, enemy.AttackRange);
            Assert.AreEqual(2, enemy.AttackCooldown);
        }

        [TestMethod]
        public void Update_PlayerInRangeButCooldownNotReady_IncrementsCounter()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);
            var enemy = new StaticEnemy(5, 5, 15, 2, 3);
            enemy.AttackCounter = 1;
            var player = new Player(6, 6, 100);

            var entities = new EntityRepository();
            entities.SetPlayer(player);

            var gameStateMock = new Mock<IGameState>();
            gameStateMock.Setup(g => g.Player).Returns(player);
            gameStateMock.Setup(g => g.PlayerX).Returns(6);
            gameStateMock.Setup(g => g.PlayerY).Returns(6);
            gameStateMock.Setup(g => g.EntityRepository).Returns(entities);

            // Act
            enemy.Update(map, gameStateMock.Object);

            // Assert
            Assert.AreEqual(100, player.Health);
            Assert.AreEqual(2, enemy.AttackCounter);
        }

        [TestMethod]
        public void Update_PlayerOutOfRange_DoesNotAttack()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);
            var enemy = new StaticEnemy(5, 5, 15, 1, 1);
            var player = new Player(8, 8, 100);

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
            Assert.AreEqual(100, player.Health);
        }
    }
}