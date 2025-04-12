using DocMaster.Models;
using DocMaster.Roles.Observers;
using DocMaster.Services;

namespace DocMaster.Command
{
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undoStack = new();
        private readonly Stack<ICommand> _redoStack = new();

        public void Push(ICommand command) => _undoStack.Push(command);

        public (int newPosition, bool success) Undo(int currentPosition)
        {
            if (_undoStack.Count == 0) return (currentPosition, false);

            var cmd = _undoStack.Pop();
            cmd.Undo();
            _redoStack.Push(cmd);

            return (cmd.CursorPositionBefore, true);
        }

        public (int newPosition, bool success) Redo(int currentPosition)
        {
            if (_redoStack.Count == 0) return (currentPosition, false);

            var cmd = _redoStack.Pop();
            cmd.Execute();
            _undoStack.Push(cmd);

            return (cmd.CursorPositionAfter, true);
        }
    }
    public class TextInsertCommand : ICommand
    {
        private readonly Document _document;
        private readonly int _position;
        private readonly string _text;
        private readonly string _editedBy;

        public int CursorPositionBefore { get; }
        public int CursorPositionAfter => _position + _text.Length;

        //////////////////////////// Adding text to a doc ////////////////////////////
        public TextInsertCommand(Document doc, int pos, string text, int cursorBefore, string editedBy)
        {
            _document = doc;
            _position = pos;
            _text = text;
            CursorPositionBefore = cursorBefore;
            _editedBy = editedBy;
        }

        public void Execute()
        {
            _document.Content = _document.Content.Insert(_position, _text);
            _document.NotifyChange(_editedBy);
            DocumentHistoryService.AddRecord(_document, _editedBy, "changed the text");
        }

        public void Undo()
        {
            _document.Content = _document.Content.Remove(_position, _text.Length);
        }
    }

    //////////////////////////// Deleting text from a doc ////////////////////////////
    public class TextDeleteCommand : ICommand
    {
        private readonly Document _document;
        private readonly int _position;
        private readonly string _deletedText;
        private readonly string _editedBy;

        public int CursorPositionBefore { get; }
        public int CursorPositionAfter { get; }

        public TextDeleteCommand(Document doc, int pos, int length, int cursorBefore, string editedBy)
        {
            pos = Math.Clamp(pos, 0, doc.Content.Length);
            length = Math.Clamp(length, 0, doc.Content.Length - pos);

            _document = doc;
            _position = pos;
            _deletedText = doc.Content.Substring(pos, length);
            CursorPositionBefore = cursorBefore;
            CursorPositionAfter = pos;
            _editedBy = editedBy;
        }

        public void Execute()
        {
            if (_position + _deletedText.Length <= _document.Content.Length)
            {
                _document.Content = _document.Content.Remove(_position, _deletedText.Length);
            }
            _document.NotifyChange(_editedBy);
            DocumentHistoryService.AddRecord(_document, _editedBy, "deleted the text");
        }

        public void Undo()
        {
            _document.Content = _document.Content.Insert(_position, _deletedText);
        }
    }
}
