using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Save
{
    public class MarkdownObject
    {
        private readonly List<MarkdownElement> _elements = new();

        public MarkdownObject(string content)
        {
            ParseContent(content);
        }

        private void ParseContent(string content)
        {
            // Упрощенный парсер Markdown
            var lines = content.Split('\n');
            foreach (var line in lines)
            {
                if (line.StartsWith("# "))
                    _elements.Add(new HeaderElement(line[2..], 1));
                else if (line.StartsWith("## "))
                    _elements.Add(new HeaderElement(line[3..], 2));
                // ... аналогично для других элементов ...
            }
        }

        public string ToJson()
        {
            var elements = _elements.Select(e => e.ToJson());
            return $"[{string.Join(",", elements)}]";
        }

        public string ToXml()
        {
            var elements = _elements.Select(e => e.ToXml());
            return $"<Elements>{string.Join("", elements)}</Elements>";
        }
    }
    public abstract class MarkdownElement
    {
        public abstract string ToJson();
        public abstract string ToXml();
    }

    // Реализация для заголовков
    public class HeaderElement : MarkdownElement
    {
        private readonly string _text;
        private readonly int _level;

        public HeaderElement(string text, int level)
        {
            _text = text;
            _level = level;
        }

        public override string ToJson() =>
            $$"""{ "type": "header", "level": {{_level}}, "text": "{{_text}}" }""";

        public override string ToXml() =>
            $"<Header level=\"{_level}\">{_text}</Header>";
    }
}
