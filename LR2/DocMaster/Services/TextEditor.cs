using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models;
using DocMaster.Command;

namespace DocMaster.Services
{
    public class TextEditor
    {
        private Document _document;
        private int _cursorPosition;
        private int _selectionStart = -1;
        private readonly CommandHistory _history = new();

        public TextEditor(Document doc)
        {
            _document = doc;
            _cursorPosition = doc.Content.Length;
        }

        public void StartEditing()
        {
            Console.TreatControlCAsInput = true;
            Console.CancelKeyPress += (s, e) => e.Cancel = true;
            Console.CursorVisible = false;
            ConsoleKeyInfo key;
            do
            {
                RenderText();
                key = Console.ReadKey(true);
                HandleKeyPress(key);
            } while (key.Key != ConsoleKey.Escape);

            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress -= (s, e) => e.Cancel = true;
            Console.CursorVisible = true;
        }
        private void UpdateSelection(bool isSelecting)
        {
            if (isSelecting)
            {
                if (_selectionStart == -1)
                    _selectionStart = _cursorPosition;
            }
            else
            {
                _selectionStart = -1;
            }
        }
        private void HandleKeyPress(ConsoleKeyInfo key)
        {
            bool isShiftPressed = (key.Modifiers & ConsoleModifiers.Shift) != 0;

            // Включаем режим выделения при нажатии Shift
            UpdateSelection(isShiftPressed);

            if (key.Modifiers == ConsoleModifiers.Control)
            {
                HandleControlCombination(key.Key);
                return;
            }

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    InsertText(Environment.NewLine);
                    break;
                case ConsoleKey.Tab:
                    InsertText("    "); // 4 пробела вместо табуляции
                    break;
                case ConsoleKey.LeftArrow:
                    _cursorPosition = Math.Max(0, _cursorPosition - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _cursorPosition = Math.Min(_document.Content.Length, _cursorPosition + 1);
                    break;
                case ConsoleKey.Backspace when _cursorPosition > 0:
                    DeleteChar(_cursorPosition - 1);
                    break;
                case ConsoleKey.Delete when _cursorPosition < _document.Content.Length:
                    DeleteChar(_cursorPosition);
                    break;
                case ConsoleKey.Home:
                    _cursorPosition = 0;
                    break;
                case ConsoleKey.End:
                    _cursorPosition = _document.Content.Length;
                    break;
                default:
                    if (!char.IsControl(key.KeyChar))
                        InsertChar(key.KeyChar);
                    break;
            }

            if (!isShiftPressed) UpdateSelection(false);
        }

        private void HandleControlCombination(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Z:
                    _history.Undo();
                    break;
                case ConsoleKey.Y:
                    _history.Redo();
                    break;
                case ConsoleKey.C:
                    CopySelection();
                    break;
                case ConsoleKey.V:
                    PasteText();
                    break;
            }
        }

        private void InsertChar(char c)
        {
            var cmd = new TextInsertCommand(_document, _cursorPosition, c.ToString());
            cmd.Execute();
            _history.Push(cmd);
            _cursorPosition++;
        }

        private void DeleteChar(int pos)
        {
            var cmd = new TextDeleteCommand(_document, pos, 1);
            cmd.Execute();
            _history.Push(cmd);
            if (pos < _cursorPosition) _cursorPosition--;
        }
        private void InsertText(string text)
        {
            var cmd = new TextInsertCommand(_document, _cursorPosition, text);
            cmd.Execute();
            _history.Push(cmd);
            _cursorPosition += text.Length;
        }
        private void CopySelection()
        {
            if (_selectionStart == -1) return;
            var start = Math.Min(_selectionStart, _cursorPosition);
            var end = Math.Max(_selectionStart, _cursorPosition);
            var text = _document.Content.Substring(start, end - start);
            Clipboard.SetText(text);
        }

        private void PasteText()
        {
            var text = Clipboard.GetText();
            if (string.IsNullOrEmpty(text)) return;

            var cmd = new TextInsertCommand(_document, _cursorPosition, text);
            cmd.Execute();
            _history.Push(cmd);
            _cursorPosition += text.Length;
        }

        private void RenderText()
        {
            Console.Clear();
            Console.WriteLine("=== Editing Mode (ESC to exit) ===");
            if (_selectionStart != -1 && _selectionStart != _cursorPosition)
            {
                var start = Math.Min(_selectionStart, _cursorPosition);
                var end = Math.Max(_selectionStart, _cursorPosition);
                var before = _document.Content.Substring(0, start);
                var selected = _document.Content.Substring(start, end - start);
                var after = _document.Content.Substring(end);

                Console.Write(before);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(selected);
                Console.ResetColor();
                Console.Write(after);
            }
            else
            {
                Console.Write(_document.Content);
            }

            Console.SetCursorPosition(_cursorPosition % Console.WindowWidth,
                _cursorPosition / Console.WindowWidth + 2);

            int cursorX = _cursorPosition % Console.WindowWidth;
            int cursorY = _cursorPosition / Console.WindowWidth + 2;

            if (cursorY < Console.WindowHeight)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                char cursorChar = _cursorPosition < _document.Content.Length
                    ? _document.Content[_cursorPosition]
                    : ' ';
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write(cursorChar);
                Console.ResetColor();
            }
        }
        public void ApplyFormatting(string formatType)
        {
            var decorator = _document.Format switch
            {
                DocumentFormat.Markdown => new MarkdownDecorator(_document),
                _ => null
            };

            decorator?.ApplyFormat(formatType);
        }
    }

    // Простой буфер обмена
    public static class Clipboard
    {
        public static string Content { get; private set; } = string.Empty;
        public static void SetText(string text)
        {
            try
            {
                Content = text;
                // Для реального буфера обмена Windows можно использовать:
                // System.Windows.Forms.Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clipboard error: {ex.Message}");
            }
        }

        public static string GetText()
        {
            try
            {
                return Content;
                // Для реального буфера обмена:
                // return System.Windows.Forms.Clipboard.GetText();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clipboard error: {ex.Message}");
                return string.Empty;
            }

        }
    }
}
