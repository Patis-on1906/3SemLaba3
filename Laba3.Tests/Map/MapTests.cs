using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Laba3.Tests.Map
{
    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void Constructor_ValidDimensions_CreatesMapWithBorders()
        {
            // Arrange & Act
            var map = new Laba3.Map(10, 10);

            // Assert
            Assert.AreEqual(10, map.Width);
            Assert.AreEqual(10, map.Height);

            for (int x = 0; x < 10; x++)
            {
                Assert.IsFalse(map.IsWalkable(x, 0));
                Assert.IsFalse(map.IsWalkable(x, 9));
            }

            for (int y = 0; y < 10; y++)
            {
                Assert.IsFalse(map.IsWalkable(0, y));
                Assert.IsFalse(map.IsWalkable(9, y));
            }

            Assert.IsTrue(map.IsWalkable(5, 5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_DimensionsLessThan3_ThrowsArgumentException()
        {
            // Act
            var map = new Laba3.Map(2, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_DimensionsLessThan3_Height_ThrowsArgumentException()
        {
            // Act
            var map = new Laba3.Map(10, 2);
        }

        [TestMethod]
        public void SetCellType_ValidCoordinates_ChangesCellType()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);

            // Act
            map.SetCellType(5, 5, Cell.CellType.Wall);

            // Assert
            Assert.IsFalse(map.IsWalkable(5, 5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetCellType_InvalidCoordinates_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);

            // Act
            map.SetCellType(10, 5, Cell.CellType.Wall);
        }

        [TestMethod]
        public void GetCell_ValidCoordinates_ReturnsCell()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);

            // Act
            var cell = map.GetCell(5, 5);

            // Assert
            Assert.IsNotNull(cell);
            Assert.AreEqual(5, cell.X);
            Assert.AreEqual(5, cell.Y);
            Assert.AreEqual(Cell.CellType.Floor, cell.Type);
        }

        [TestMethod]
        public void GetCell_InvalidCoordinates_ReturnsNull()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);

            // Act
            var cell = map.GetCell(10, 5);

            // Assert
            Assert.IsNull(cell);
        }

        [TestMethod]
        public void IsWalkable_ValidFloorCell_ReturnsTrue()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);

            // Act & Assert
            Assert.IsTrue(map.IsWalkable(5, 5));
        }

        [TestMethod]
        public void IsWalkable_ValidWallCell_ReturnsFalse()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);

            // Act & Assert
            Assert.IsFalse(map.IsWalkable(0, 0));
        }

        [TestMethod]
        public void IsWalkable_InvalidCoordinates_ReturnsFalse()
        {
            // Arrange
            var map = new Laba3.Map(10, 10);

            // Act & Assert
            Assert.IsFalse(map.IsWalkable(-1, 5));
            Assert.IsFalse(map.IsWalkable(5, -1));
            Assert.IsFalse(map.IsWalkable(10, 5));
            Assert.IsFalse(map.IsWalkable(5, 10));
        }
    }
}