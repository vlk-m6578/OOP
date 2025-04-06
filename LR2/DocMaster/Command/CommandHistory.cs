using DocMaster.Models;

namespace DocMaster.Command
{
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undoStack = new();
        private readonly Stack<ICommand> _redoStack = new();

        public void Push(ICommand command) => _undoStack.Push(command);

        public void Undo()
        {
            if (_undoStack.Count == 0) return;
            var cmd = _undoStack.Pop();
            cmd.Undo();
            _redoStack.Push(cmd);
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) return;
            var cmd = _redoStack.Pop();
            cmd.Execute();
            _undoStack.Push(cmd);
        }
    }

    // Конкретные команды
    public class TextInsertCommand : ICommand
    {
        private readonly Document _document;
        private readonly int _position;
        private readonly string _text;

        public TextInsertCommand(Document doc, int pos, string text)
        {
            _document = doc;
            _position = pos;
            _text = text;
        }

        public void Execute() => _document.Content = _document.Content.Insert(_position, _text);
        public void Undo() => _document.Content = _document.Content.Remove(_position, _text.Length);
    }

    public class TextDeleteCommand : ICommand
    {
        private readonly Document _document;
        private readonly int _position;
        private readonly string _deletedText;

        public TextDeleteCommand(Document doc, int pos, int length)
        {
            _document = doc;
            _position = pos;
            _deletedText = doc.Content.Substring(pos, length);
        }

        public void Execute() => _document.Content = _document.Content.Remove(_position, _deletedText.Length);
        public void Undo() => _document.Content = _document.Content.Insert(_position, _deletedText);
    }
}
