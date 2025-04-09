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

        private bool _isPreviewMode = false;
        private DocumentFormat _format;

        public TextEditor(Document doc)
        {
            _document = doc;
            _format = doc.Format;
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
        private void CopyToSystemClipboard()
        {
            if (_selectionStart == -1 || _selectionStart == _cursorPosition) return;

            int start = Math.Min(_selectionStart, _cursorPosition);
            int end = Math.Max(_selectionStart, _cursorPosition);
            string selectedText = _document.Content.Substring(start, end - start);

            try
            {
                TextCopy.ClipboardService.SetText(selectedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка буфера: {ex.Message}");
            }
        }
        private void HandleKeyPress(ConsoleKeyInfo key)
        {
            if (_format == DocumentFormat.Markdown && !_isPreviewMode)
            {
                HandleMarkdownShortcuts(key);
            }
            if (key.Key == ConsoleKey.F12)
            {
                TogglePreviewMode();
                return;
            }

            if (_isPreviewMode) return;
            bool isShiftPressed = (key.Modifiers & ConsoleModifiers.Shift) != 0;

            // 1. Сначала обработать Control+Комбинации
            if ((key.Modifiers & ConsoleModifiers.Control) != 0)
            {
                HandleControlCombination(key.Key);
                UpdateSelection(isShiftPressed); // Обновить выделение после обработки
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

            UpdateSelection(isShiftPressed);
        }

        private void HandleControlCombination(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Z:
                    var (undoPos, undoSuccess) = _history.Undo(_cursorPosition);
                    if (undoSuccess)
                    {
                        _cursorPosition = undoPos;
                        _cursorPosition = Math.Clamp(_cursorPosition, 0, _document.Content.Length);
                    }
                    break;

                case ConsoleKey.Y:
                    var (redoPos, redoSuccess) = _history.Redo(_cursorPosition);
                    if (redoSuccess)
                    {
                        _cursorPosition = redoPos;
                        _cursorPosition = Math.Clamp(_cursorPosition, 0, _document.Content.Length);
                    }
                    break;
                case ConsoleKey.C:
                    CopySelection();
                    break;

                case ConsoleKey.V:
                    PasteText();
                    break;

                case ConsoleKey.X:
                    CutSelection();
                    break;
            }
        }
        private void CutSelection()
        {
            if (_selectionStart == -1 || _selectionStart == _cursorPosition)
                return;

            int start = Math.Min(_selectionStart, _cursorPosition);
            int end = Math.Max(_selectionStart, _cursorPosition);
            int length = end - start;

            // Копируем в буфер
            CopySelection();

            // Создаем команду удаления диапазона
            var cmd = new TextDeleteCommand(
                doc: _document,
                pos: start,
                length: length,
                cursorBefore: _cursorPosition
            );

            cmd.Execute();
            _history.Push(cmd);

            // Перемещаем курсор и сбрасываем выделение
            _cursorPosition = start;
            _selectionStart = -1;

            // Явно обновляем экран
            RenderText();
        }
        private void InsertChar(char c)
        {
            var before = _cursorPosition;
            var cmd = new TextInsertCommand(_document, _cursorPosition, c.ToString(), before);
            cmd.Execute();
            _history.Push(cmd);
            _cursorPosition = cmd.CursorPositionAfter;
        }

        private void DeleteChar(int pos)
        {
            var before = _cursorPosition;
            var cmd = new TextDeleteCommand(_document, pos, 1, before);
            cmd.Execute();
            _history.Push(cmd);
            _cursorPosition = cmd.CursorPositionAfter;
        }
        private void InsertText(string text)
        {
            // Заменяем Environment.NewLine на \n
            text = text.Replace(Environment.NewLine, "\n");

            // Сохраняем исходную позицию
            int originalPosition = _cursorPosition;

            // Создаем и выполняем команду
            var cmd = new TextInsertCommand(_document, _cursorPosition, text, originalPosition);
            cmd.Execute();
            _history.Push(cmd);

            // Обновляем позицию курсора
            _cursorPosition += text.Length;

            // Специальная обработка для перевода строки
            if (text.Contains('\n'))
            {
                // Находим позицию следующего перевода строки
                int newLineIndex = _document.Content.IndexOf('\n', originalPosition);

                if (newLineIndex != -1)
                {
                    // Устанавливаем курсор в начало новой строки
                    _cursorPosition = newLineIndex + 1;
                }
                else
                {
                    // Если это последняя строка, ставим курсор в конец
                    _cursorPosition = _document.Content.Length;
                }
            }

            // Гарантируем корректные границы
            _cursorPosition = Math.Clamp(_cursorPosition, 0, _document.Content.Length);
        }
        private void CopySelection()
        {
            if (_selectionStart == -1 || _selectionStart == _cursorPosition)
                return;

            int start = Math.Min(_selectionStart, _cursorPosition);
            int end = Math.Max(_selectionStart, _cursorPosition);
            string selectedText = _document.Content.Substring(start, end - start);

            try
            {
                TextCopy.ClipboardService.SetText(selectedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка буфера: {ex.Message}");
            }
            RenderText();
        }

        private void PasteText()
        {
            var text = Clipboard.GetText();
            if (string.IsNullOrEmpty(text)) return;

            // Создаем команду с текущей позицией курсора
            var cmd = new TextInsertCommand(
                doc: _document,
                pos: _cursorPosition,
                text: text,
                cursorBefore: _cursorPosition
            );

            cmd.Execute();
            _history.Push(cmd);

            // Обновляем позицию курсора через свойство команды
            _cursorPosition = cmd.CursorPositionAfter;

            // Обеспечиваем корректные границы
            _cursorPosition = Math.Clamp(_cursorPosition, 0, _document.Content.Length);
        }
        private void HandleMarkdownShortcuts(ConsoleKeyInfo key)
        {
            if ((key.Modifiers & ConsoleModifiers.Control) != 0)
            {
                int start = Math.Min(_selectionStart, _cursorPosition);
                int end = Math.Max(_selectionStart, _cursorPosition);

                if (key.Key == ConsoleKey.B && start != end)
                {
                    ApplyMarkdownFormatting(start, end, "**");
                }
                else if (key.Key == ConsoleKey.I && start != end)
                {
                    ApplyMarkdownFormatting(start, end, "*");
                }
            }
            else if (key.Key == ConsoleKey.F12)
            {
                TogglePreviewMode();
            }
        }

        private void ApplyMarkdownFormatting(int start, int end, string wrapper)
        {
            string selectedText = _document.Content.Substring(start+1, end - start-1);
            string newText = $"{wrapper}{selectedText}{wrapper}";

            var cmd = new TextReplaceCommand(
                doc: _document,
                start: start,
                end: end,
                newText: newText,
                cursorBefore: _cursorPosition
            );

            cmd.Execute();
            _history.Push(cmd);
            _cursorPosition = end + wrapper.Length * 2;
            _selectionStart = -1;
        }

        private void TogglePreviewMode()
        {
            _isPreviewMode = !_isPreviewMode;
            RenderText();
        }
        private void RenderText()
        {
            if (_isPreviewMode && _format == DocumentFormat.Markdown)
            {
                RenderMarkdownPreview();
            }
            else
            {
                RenderNormalText();
            }
        }
        private void RenderMarkdownPreview()
        {
            Console.Clear();
            Console.WriteLine("=== Preview Mode (F12 to exit) ===");

            string content = _document.Content;
            int pos = 0;

            while (pos < content.Length)
            {
                if (pos < content.Length - 1 && content[pos] == '*' && content[pos + 1] == '*')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    pos = ProcessBold(content, pos);
                }
                else if (content[pos] == '*')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    pos = ProcessItalic(content, pos);
                }
                else
                {
                    Console.ResetColor();
                    Console.Write(content[pos]);
                    pos++;
                }
            }
        }

        private int ProcessBold(string content, int pos)
        {
            pos += 2;
            while (pos < content.Length+1 && !(content[pos] == '*' && content[pos + 1] == '*'))
            {
                Console.Write(content[pos]);
                pos++;
            }
            pos += 2;
            Console.ResetColor();
            return pos;
        }

        private int ProcessItalic(string content, int pos)
        {
            pos++;
            while (pos < content.Length && content[pos] != '*')
            {
                Console.Write(content[pos]);
                pos++;
            }
            pos++;
            Console.ResetColor();
            return pos;
        }

        private int ProcessToEndOfLine(string content, int pos)
        {
            // Пропускаем пробелы после #
            while (pos < content.Length && char.IsWhiteSpace(content[pos]))
                pos++;

            // Запоминаем начало строки
            int lineStart = pos;

            // Ищем конец строки
            while (pos < content.Length && content[pos] != '\n')
                pos++;

            // Выводим всю строку
            if (lineStart < pos)
            {
                Console.Write(content.Substring(lineStart, pos - lineStart));
            }

            // Добавляем перенос строки
            Console.WriteLine();

            // Пропускаем символ \n если он есть
            if (pos < content.Length && content[pos] == '\n')
                pos++;

            return pos;
        }
        private void RenderNormalText()
        {
            Console.Clear();
            Console.WriteLine("=== Editing Mode (ESC to exit) ===");
            Console.Write(_document.Content);

            // Рассчитываем позицию курсора в консоли
            int consoleLine = 2; // Строка после заголовка
            int consoleColumn = 0;
            int contentPos = 0;

            foreach (char c in _document.Content)
            {
                if (contentPos >= _cursorPosition) break;

                if (c == '\n')
                {
                    consoleLine++;
                    consoleColumn = 0;
                }
                else
                {
                    consoleColumn++;
                    if (consoleColumn >= Console.WindowWidth)
                    {
                        consoleLine++;
                        consoleColumn = 0;
                    }
                }
                contentPos++;
            }

            // Устанавливаем курсор
            Console.SetCursorPosition(consoleColumn, consoleLine);

            // Отображение курсора
            Console.Write(_cursorPosition < _document.Content.Length
                ? _document.Content[_cursorPosition]
                : ' ');

            Console.SetCursorPosition(consoleColumn, consoleLine);
        }
    }

}
public class TextReplaceCommand : ICommand
{
    private readonly Document _document;
    private readonly int _start;
    private readonly int _end;
    private readonly string _newText;
    private readonly string _oldText;

    public int CursorPositionBefore { get; }
    public int CursorPositionAfter { get; }

    public TextReplaceCommand(Document doc, int start, int end, string newText, int cursorBefore)
    {
        _document = doc;
        _start = start;
        _end = end;
        _newText = newText;
        _oldText = doc.Content.Substring(start, end - start);
        CursorPositionBefore = cursorBefore;
        CursorPositionAfter = start + newText.Length;
    }

    public void Execute()
    {
        _document.Content = _document.Content
            .Remove(_start, _end - _start)
            .Insert(_start, _newText);
    }

    public void Undo()
    {
        _document.Content = _document.Content
            .Remove(_start, _newText.Length)
            .Insert(_start, _oldText);
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
            // Используем системный буфер через TextCopy
            TextCopy.ClipboardService.SetText(text);
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
            return TextCopy.ClipboardService.GetText();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Clipboard error: {ex.Message}");
            return string.Empty;
        }
    }
}