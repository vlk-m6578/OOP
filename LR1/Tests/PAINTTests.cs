using PAINT.Models;
using PAINT.Utilities;
using System.Diagnostics;

namespace TestProject1
{

    [TestClass]
    public class PAINTTests
    {
        [TestMethod]
        public void AddShape_ShapeIsAdded_ReturnCorrectShape()
        {
            ////////////// Arrange ////////////////
            var canvas = new Canvas(60, 20);
            var shape = new Circle("TestsCircle", '*', '#', 10);

            ////////// Act ////////
            canvas.AddShape(shape);

            /////////////////// Assert /////////////
            Assert.AreEqual(1, canvas.ShapesCount());
            Assert.AreEqual("TestsCircle", canvas.GetShape(0).Name);

        }
        [TestMethod]
        public void RemoveShape_ShapeRemovedFromList_CorrectRemovedFromList()
        {
            ////////////// Arrange ////////////////
            var canvas = new Canvas(60, 20);
            canvas.AddShape(new Circle("TestsCircle", '*', '#', 10));

            ////////// Act ////////
            canvas.RemoveShape(0);

            /////////////////// Assert /////////////
            Assert.AreEqual(0, canvas.ShapesCount());
        }
        [TestMethod]
        public void SaveAndLoad_SaveAndLoadCanvas_CorrectSavingAndLoading()
        {
            ////////////// Arrange ////////////////
            var canvas = new Canvas(60, 20);
            canvas.AddShape(new Rectangle("TestsRectangle", '$', '@', 4, 7));
            string testFile = "test.json";

            ////////// Act ////////
            canvas.SaveToFile(testFile);
            canvas.ClearShapes();
            canvas.LoadFromFile(testFile);

            /////////////////// Assert /////////////
            Assert.AreEqual(1, canvas.ShapesCount());
            Assert.IsTrue(canvas.GetShape(0) is Rectangle);
        }
        [TestMethod]
        public void Move_CircleMove_UpdateCoordinates()
        {
            ////////////// Arrange ////////////////
            var shape = new Circle("TestsCircle", '$', '@', 4); // X = 30 Y = 10

            ////////// Act ////////
            shape.Move(5, -5);

            /////////////////// Assert /////////////
            Assert.AreEqual(35, shape.X);
            Assert.AreEqual(5, shape.Y);
        }

        [TestMethod]
        public void Clone_CircleClone_CreatesCopy()
        {
            ////////////// Arrange ////////////////
            var original = new Circle("TestsCircle", ' ', '@', 3); // X = 30 Y = 10

            ////////// Act ////////
            var copy = original.Clone();
            copy.Move(10, 0);

            /////////////////// Assert /////////////
            Assert.AreEqual(30, original.X);
            Assert.AreEqual(10, original.Y);
            Assert.AreEqual(40, copy.X);
            Assert.AreEqual(10, copy.Y);
        }
        [TestMethod]
        public void Undo_UndoCommand_ReturnPreviousState()
        {
            ////////////// Arrange ////////////////
            var canvas = new Canvas(60, 20);
            var manager = new CommandManager();
            manager.SaveState(canvas);

            ////////// Act ////////
            canvas.AddShape(new Triangle("TestsTriangle", '*', '#', 3, 4, 5));
            manager.SaveState(canvas);
            var result = manager.Undo(canvas);

            /////////////////// Assert /////////////
            Assert.IsTrue(result);
            Assert.AreEqual(0, canvas.ShapesCount());
        }
        [TestMethod]
        public void Redo_RedoCommand_ReturnNextState()
        {
            ////////////// Arrange ////////////////
            var canvas = new Canvas(60, 20);
            var manager = new CommandManager();
            manager.SaveState(canvas);
            canvas.AddShape(new Circle("Test", '*', '#', 10));
            manager.SaveState(canvas);
            manager.Undo(canvas);

            ////////// Act ////////
            var result = manager.Redo(canvas);

            /////////////////// Assert /////////////
            Assert.IsTrue(result);
            Assert.AreEqual(1, canvas.ShapesCount());
        }
        [TestMethod]
        public void GetIntInput_ValidatesCorrectInput()
        {
            ////////////// Arrange ////////////////
            var input = "5\n";
            var consoleInput = new System.IO.StringReader(input);
            Console.SetIn(consoleInput);

            ////////// Act ////////
            var result = InputValidator.GetIntInput(1, 10);

            /////////////////// Assert /////////////
            Assert.AreEqual(5, result);
        }
    }
}