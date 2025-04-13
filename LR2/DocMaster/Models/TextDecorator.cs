
public abstract class TextDecorator
{
    protected string _content;

    public TextDecorator(string content) => _content = content;
    public abstract string GetFormattedContent();
}

