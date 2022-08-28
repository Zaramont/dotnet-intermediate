public class InputText : IXmlElement
{
    private readonly string name;
    private readonly string value;

    public InputText(string name, string value = "")
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.value = value;
    }

    public string ConvertToString()
    {
        return $"< inputText name = '{this.name}' value = '{this.value}' />";
    }
}