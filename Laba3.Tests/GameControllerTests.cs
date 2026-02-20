using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Laba3.Tests
{
    [TestClass]
    public class GameControllerTests
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
        public void Constructor_ValidParameters_CreatesController()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            // Act
            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Assert
            Assert.IsNotNull(controller);
            Assert.AreEqual(state, controller.GameState);
            Assert.IsTrue(controller.IsRunning);
        }

        [TestMethod]
        public void HandleCommand_Quit_RaisesRequestClose()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            bool closeRaised = false;
            controller.RequestClose += () => closeRaised = true;

            // Act
            controller.HandleCommand(InputCommand.Quit);

            // Assert
            Assert.IsTrue(closeRaised);
        }

        [TestMethod]
        public void HandleCommand_Save_CallsSaveService()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.HandleCommand(InputCommand.Save);

            // Assert
            saveServiceMock.Verify(s => s.Save(state), Times.Once);
            rendererMock.Verify(r => r.ShowMessage(It.IsAny<string>(), ConsoleColor.Green), Times.Once);
        }

        [TestMethod]
        public void HandleCommand_Save_WhenExceptionOccurs_ShowsErrorMessage()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            saveServiceMock.Setup(s => s.Save(It.IsAny<GameState>()))
                .Throws(new SaveLoadException("Test error", new Exception()));

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.HandleCommand(InputCommand.Save);

            // Assert
            rendererMock.Verify(r => r.ShowMessage(It.Is<string>(msg => msg.Contains("Ошибка")), ConsoleColor.Red), Times.Once);
        }

        [TestMethod]
        public void HandleCommand_Load_CallsLoadService()
        {
            // Arrange
            var state = CreateTestState();
            var loadedState = CreateTestState();

            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            saveServiceMock.Setup(s => s.Load()).Returns(loadedState);

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.HandleCommand(InputCommand.Load);

            // Assert
            saveServiceMock.Verify(s => s.Load(), Times.Once);
            rendererMock.Verify(r => r.ShowMessage(It.IsAny<string>(), ConsoleColor.Green), Times.Once);
            Assert.AreEqual(loadedState, controller.GameState);
        }

        [TestMethod]
        public void HandleCommand_Load_WhenNoSaveFile_ShowsMessage()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            saveServiceMock.Setup(s => s.Load()).Returns((GameState)null);

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.HandleCommand(InputCommand.Load);

            // Assert
            rendererMock.Verify(r => r.ShowMessage(It.Is<string>(msg => msg.Contains("не найдено")), ConsoleColor.Yellow), Times.Once);
        }

        [TestMethod]
        public void HandleCommand_MoveUp_CallsProcessPlayerMovement()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.HandleCommand(InputCommand.MoveUp);

            // Assert
            gameLogicMock.Verify(g => g.ProcessPlayerMovement(state, 0, -1), Times.Once);
            gameLogicMock.Verify(g => g.UpdateWorld(state), Times.Once);
        }

        [TestMethod]
        public void Update_CallsUpdateWorldAndCheckGameState()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.Update();

            // Assert
            gameLogicMock.Verify(g => g.UpdateWorld(state), Times.Once);
            gameLogicMock.Verify(g => g.CheckGameOver(state), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(GameOverException))]
        public void Update_WhenGameOverExceptionThrown_PropagatesException()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            gameLogicMock.Setup(g => g.CheckGameOver(state))
                .Throws(new GameOverException("Game Over"));

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.Update();
        }

        [TestMethod]
        [ExpectedException(typeof(VictoryException))]
        public void Update_WhenVictoryConditionMet_ThrowsVictoryException()
        {
            // Arrange
            var state = CreateTestState();
            var rendererMock = new Mock<IRenderer>();
            var saveServiceMock = new Mock<ISaveService>();
            var gameLogicMock = new Mock<IGameLogicService>();

            gameLogicMock.Setup(g => g.CheckVictory(state)).Returns(true);

            var controller = new GameController(state, rendererMock.Object, saveServiceMock.Object, gameLogicMock.Object);

            // Act
            controller.Update();
        }
    }
}