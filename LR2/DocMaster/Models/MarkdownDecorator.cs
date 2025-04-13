
namespace DocMaster.Models
{
    public class MarkdownDecorator : TextDecorator
    {
        private readonly string _formatType;

        // Конструктор принимает контент и тип форматирования
        public MarkdownDecorator(string content, string formatType)
            : base(content)
        {
            _formatType = formatType.ToLower();
        }

        // Реализация форматирования
        public override string GetFormattedContent()
        {
            return _formatType switch
            {
                "bold" => $"**{_content}**",
                "italic" => $"*{_content}*",
                "underline" => $"__{_content}__",
                _ => _content // Возвращаем оригинал, если формат не распознан
            };
        }
    }
}
