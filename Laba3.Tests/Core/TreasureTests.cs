using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Laba3.Tests.Entities
{
    [TestClass]
    public class TreasureTests
    {
        [TestMethod]
        public void Constructor_DefaultValues_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var treasure = new Treasure(5, 5);

            // Assert
            Assert.AreEqual(5, treasure.X);
            Assert.AreEqual(5, treasure.Y);
            Assert.AreEqual(10, treasure.Value);
            Assert.IsFalse(treasure.Collected);
            Assert.AreEqual('T', treasure.Symbol);
            Assert.AreEqual(EntityType.Treasure, treasure.EntityType);
            Assert.IsTrue(treasure.IsPassable);
        }

        [TestMethod]
        public void Constructor_CustomValue_ValueSetCorrectly()
        {
            // Arrange & Act
            var treasure = new Treasure(5, 5, 50);

            // Assert
            Assert.AreEqual(50, treasure.Value);
        }

        [TestMethod]
        public void Collect_TreasureNotCollected_AddsScoreToPlayerAndMarksCollected()
        {
            // Arrange
            var treasure = new Treasure(5, 5, 25);
            var player = new Player(5, 5);

            // Act
            treasure.Collect(player);

            // Assert
            Assert.IsTrue(treasure.Collected);
            Assert.AreEqual(25, player.Score);
        }

        [TestMethod]
        public void Collect_TreasureAlreadyCollected_DoesNotAddScoreAgain()
        {
            // Arrange
            var treasure = new Treasure(5, 5, 25);
            var player = new Player(5, 5);

            treasure.Collect(player);

            // Act
            treasure.Collect(player);

            // Assert
            Assert.AreEqual(25, player.Score);
        }

        [TestMethod]
        public void Collect_NullPlayer_DoesNotThrow()
        {
            // Arrange
            var treasure = new Treasure(5, 5);

            // Act
            treasure.Collect(null);

            // Assert
            Assert.IsFalse(treasure.Collected);
        }
    }
}