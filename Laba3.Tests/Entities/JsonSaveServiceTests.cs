using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Numerics;

namespace Laba3.Tests.Core
{
    [TestClass]
    public class JsonSaveServiceTests
    {
        private const string TestFileName = "savegame.json";
        private JsonSaveService _saveService;

        [TestInitialize]
        public void Setup()
        {
            _saveService = new JsonSaveService();
            if (File.Exists(TestFileName))
                File.Delete(TestFileName);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFileName))
                File.Delete(TestFileName);
        }

        private GameState CreateTestState()
        {
            var map = new Laba3.Map(10, 10);
            var repo = new EntityRepository();
            var player = new Player(5, 5, 100);
            player.AddScore(50);
            repo.SetPlayer(player);

            repo.AddMovingEnemy(new MovingEnemy(3, 3, 10, 2));
            repo.AddStaticEnemy(new StaticEnemy(7, 7, 15, 2, 3));
            repo.AddTreasure(new Treasure(2, 2, 25));
            repo.AddTreasure(new Treasure(8, 8, 30));

            return new GameState(map, repo);
        }

        [TestMethod]
        public void Save_ValidState_FileCreated()
        {
            // Arrange
            var state = CreateTestState();

            // Act
            _saveService.Save(state);

            // Assert
            Assert.IsTrue(File.Exists(TestFileName));
        }

        [TestMethod]
        public void SaveAndLoad_RoundTrip_StatePreserved()
        {
            // Arrange
            var originalState = CreateTestState();

            // Act
            _saveService.Save(originalState);
            var loadedState = _saveService.Load();

            // Assert
            Assert.IsNotNull(loadedState);
            Assert.AreEqual(originalState.Map.Width, loadedState.Map.Width);
            Assert.AreEqual(originalState.Map.Height, loadedState.Map.Height);
            Assert.AreEqual(originalState.Player.X, loadedState.Player.X);
            Assert.AreEqual(originalState.Player.Y, loadedState.Player.Y);
            Assert.AreEqual(originalState.Player.Health, loadedState.Player.Health);
            Assert.AreEqual(originalState.Player.Score, loadedState.Player.Score);
            Assert.AreEqual(originalState.EntityRepository.MovingEnemies.Count,
                          loadedState.EntityRepository.MovingEnemies.Count);
            Assert.AreEqual(originalState.EntityRepository.StaticEnemies.Count,
                          loadedState.EntityRepository.StaticEnemies.Count);
            Assert.AreEqual(originalState.EntityRepository.Treasures.Count,
                          loadedState.EntityRepository.Treasures.Count);
        }

        [TestMethod]
        public void Load_NoSaveFile_ReturnsNull()
        {
            // Act
            var result = _saveService.Load();

            // Assert
            Assert.IsNull(result);
        }
    }
}