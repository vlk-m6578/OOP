using PAINT.Models;

namespace PAINT.Utilities
{
    public class CommandManager
    {
        private Stack<List<Shape>> _undoStack = new Stack<List<Shape>>();
        private Stack<List<Shape>> _redoStack = new Stack<List<Shape>>();

        public void SaveState(Canvas canvas)
        {
            var state = canvas.GetState();
            _undoStack.Push(state);
            _redoStack.Clear();
        }
        public bool Undo(Canvas canvas)
        {
            if (_undoStack.Count < 2)
                return false;

            var currentState = _undoStack.Pop();
            _redoStack.Push(currentState);
            var previousState = _undoStack.Peek();
            canvas.RestoreState(previousState);
            return true;
        }

        public bool Redo(Canvas canvas)
        {
            if (_redoStack.Count == 0)
                return false;

            var nextState = _redoStack.Pop();
            _undoStack.Push(nextState);
            canvas.RestoreState(nextState);
            return true;
        }
    }
}
