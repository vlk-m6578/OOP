using DocMaster.Command;
using DocMaster.Data.Adapters;
using DocMaster.Data.StorageStrategies;
using DocMaster.Models;
using DocMaster.Roles;
using DocMaster.Roles.Observers;
using DocMaster.Services.FileService;
using DocMaster.Services;
namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TextInsertCommand_Execute_InsertsTextCorrectly()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.TXT);
            var command = new TextInsertCommand(doc, 0, "hello", 0, "user");

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual("hello", doc.Content);
            Assert.AreEqual(5, command.CursorPositionAfter);
        }
        [TestMethod]
        public void TextInsertCommand_Undo_RevertsInsertion()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.TXT);
            var command = new TextInsertCommand(doc, 0, "hello", 0, "user");
            command.Execute();

            // Act
            command.Undo();

            // Assert
            Assert.AreEqual(string.Empty, doc.Content);
        }

        [TestMethod]
        public void TextDeleteCommand_Execute_DeletesTextCorrectly()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.TXT) { Content = "test" };
            var command = new TextDeleteCommand(doc, 0, 2, 4, "user");

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual("st", doc.Content);
        }

        [TestMethod]
        public void TextReplaceCommand_Execute_ReplacesTextCorrectly()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.TXT) { Content = "old text" };
            var command = new TextReplaceCommand(doc, 0, 3, "new", 0, "user");

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual("new text", doc.Content);
            Assert.AreEqual(3, command.CursorPositionAfter);
        }
        [TestMethod]
        public void JsonAdapter_Convert_ProducesValidJson()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.JSON) { Content = "content" };
            var adapter = new JsonAdapter();

            // Act
            var result = adapter.Convert(doc);

            // Assert
            StringAssert.Contains(result, "\"name\": \"test\"");
            StringAssert.Contains(result, "\"content\": \"content\"");
        }
        [TestMethod]
        public void Undo_WithEmptyUndoStack_ReturnsFailure()
        {
            // Arrange
            var history = new CommandHistory();
            int currentPos = 5;

            // Act
            var result = history.Undo(currentPos);

            // Assert
            Assert.AreEqual(currentPos, result.newPosition);
            Assert.IsFalse(result.success);
        }

        [TestMethod]
        public void Redo_WithEmptyRedoStack_ReturnsFailure()
        {
            // Arrange
            var history = new CommandHistory();
            int currentPos = 3;

            // Act
            var result = history.Redo(currentPos);

            // Assert
            Assert.AreEqual(currentPos, result.newPosition);
            Assert.IsFalse(result.success);
        }

        [TestMethod]
        public void TxtAdapter_Convert_ReturnsOriginalContent()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.TXT) { Content = "Original content" };
            var adapter = new TxtAdapter();

            // Act
            var result = adapter.Convert(doc);

            // Assert
            Assert.AreEqual("Original content", result);
        }

        [TestMethod]
        public void XmlAdapter_Convert_WrapsContentInCDATA()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.XML) { Content = "<tag>content</tag>" };
            var adapter = new XmlAdapter();

            // Act
            var result = adapter.Convert(doc);

            // Assert
            StringAssert.Contains(result, "<![CDATA[<tag>content</tag>]]>");
        }

        [TestMethod]
        public void StorageStrategyFactory_CreateStrategy_ReturnsCorrectType()
        {
            // Act
            var localStrategy = StorageStrategyFactory.CreateStrategy(StorageType.Local, "test_path");
            var dbStrategy = StorageStrategyFactory.CreateStrategy(StorageType.Database);
            var cloudStrategy = StorageStrategyFactory.CreateStrategy(StorageType.Cloud);

            // Assert
            Assert.IsInstanceOfType(localStrategy, typeof(LocalStorageStrategy));
            Assert.IsInstanceOfType(dbStrategy, typeof(DatabaseStorageStrategy));
            Assert.IsInstanceOfType(cloudStrategy, typeof(CloudStorageStrategy));
        }

        [TestMethod]
        public void Document_ConvertTo_UsesCorrectAdapter()
        {
            // Arrange
            var doc = new Document("test", DocumentFormat.Markdown) { Content = "# Header" };

            // Act
            var jsonResult = doc.ConvertTo(DocumentFormat.JSON);
            var xmlResult = doc.ConvertTo(DocumentFormat.XML);

            // Assert
            StringAssert.Contains(jsonResult, "\"content\": \"# Header\"");
            StringAssert.Contains(xmlResult, "<Content><![CDATA[# Header]]></Content>");
        }

        [TestMethod]
        public void RoleContext_SetRole_CreatesCorrectStrategy()
        {
            // Arrange
            var context = new RoleContext();

            // Act
            context.SetRole(UserRole.Admin);
            var adminStrategy = context.CanManageUsers;

            context.SetRole(UserRole.Editor);
            var editorStrategy = context.CanManageUsers;

            // Assert
            Assert.IsTrue(adminStrategy);
            Assert.IsFalse(editorStrategy);
        }

        [TestMethod]
        public void BlockedDocumentManager_BlockDocument_AddsUserToBlockList()
        {
            // Arrange
            var manager = new BlockedDocumentManager();
            const string testFile = "test.txt";
            const string user = "user1";

            // Act
            manager.BlockDocument(testFile, user);
            var isBlocked = manager.IsDocumentBlocked(testFile, user);

            // Cleanup
            manager.UnblockDocument(testFile, user);

            // Assert
            Assert.IsTrue(isBlocked);
        }

    }
}