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
                else
                    CopyToSystemClipboard(); // Копируем при изменении выделения
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
            bool isShiftPressed = (key.Modifiers & ConsoleModifiers.Shift) != 0;

            // Вызываем перед обновлением позиции
            if (isShiftPressed) CopyToSystemClipboard();

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

            }
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
            // Добавляем проверку на валидное выделение
            if (_selectionStart == -1 || _selectionStart == _cursorPosition)
                return;

            // Явно указываем типы
            int start = Math.Min(_selectionStart, _cursorPosition);
            int end = Math.Max(_selectionStart, _cursorPosition);

            // Проверка границ
            if (start < 0) start = 0;
            if (end > _document.Content.Length) end = _document.Content.Length;

            // Копируем текст
            string selectedText = _document.Content.Substring(start, end - start);
            Clipboard.SetText(selectedText);

            // Визуальная обратная связь
            Console.Beep(); // Звуковой сигнал
            RenderText(); // Обновляем экран
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

        private void RenderText()
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

            // Подсветка
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(_cursorPosition < _document.Content.Length ? _document.Content[_cursorPosition] : ' ');
            Console.ResetColor();
            Console.SetCursorPosition(consoleColumn, consoleLine);
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
}
