using DocMaster.Models;

public abstract class TextDecorator : Document
{
    protected Document _document;

    protected TextDecorator(Document document)
        : base(document.Name, document.Format)
    {
        _document = document;
        Content = document.Content;
    }

    public abstract void ApplyFormat(string formatType);
}

public class MarkdownDecorator : TextDecorator
{
    public MarkdownDecorator(Document document) : base(document) { }

    public override void ApplyFormat(string formatType)
    {
        switch (formatType.ToLower())
        {
            case "bold":
                Content = $"**{Content}**";
                break;
            case "italic":
                Content = $"*{Content}*";
                break;
            case "underline":
                Content = $"__{Content}__";
                break;
        }
    }
}

