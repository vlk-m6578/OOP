using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models.Documents;

namespace DocMaster.Services
{
    public class DocumentManager
    {
        private Document _currentDocument;
        private int _cursorPosition;
        private int _windowWidth = Console.WindowWidth;

        public void CreateDocument(DocumentType type)
        {
            _currentDocument = type switch
            {
                DocumentType.Text => new TextDocument(),
                //DocumentType.Text => new MarkdownDocument(),
                DocumentType.RichText => new RichTextDocument(),
                _ => throw new ArgumentException("Invalid document type")
            };
            _cursorPosition = 0;
        }
        public void OpenDocument(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            _currentDocument = extension switch
            {
                ".txt" => new TextDocument(),
                ".md" => new MarkdownDocument(),
                ".rtf" => new RichTextDocument(),
                _ => throw new NotSupportedException("Unsupported file format")
            };
            _currentDocument.Load(path);
            _cursorPosition = 0;
        }
        public void StartEditing()
        {
            if (_currentDocument == null) return;

            ConsoleKeyInfo key;
            do
            {
                Console.Clear();
                //render
                Console.SetCursorPosition(_cursorPosition % _windowWidth, _cursorPosition / _windowWidth);
                key = Console.ReadKey(true);
                HandleKeyPress(key);

            } while (key.Key != ConsoleKey.Escape);
        }
        private void HandleKeyPress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow when _cursorPosition > 0:
                    _cursorPosition--;
                    break;
                case ConsoleKey.RightArrow when _cursorPosition < _currentDocument.Content.Length:
                    _cursorPosition++;
                    break;
                case ConsoleKey.Backspace when _cursorPosition > 0:
                    _currentDocument.Delete(_cursorPosition - 1, 1);
                    _cursorPosition--;
                    break;
                case ConsoleKey.Delete when _cursorPosition < _currentDocument.Content.Length:
                    _currentDocument.Delete(_cursorPosition, 1);
                    break;
                case ConsoleKey.Enter:
                    InsertText("\n");
                    break;
                default:
                    if (!char.IsControl(key.KeyChar))
                        InsertText(key.KeyChar.ToString());
                    break;
            }
        }
        private void InsertText(string text) 
        {
            _currentDocument.Insert(text, _cursorPosition);
            _cursorPosition += text.Length;
        }
    }
}
