

public class LabelText: IXmlElement
{
    private readonly string value;
    public LabelText(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.value = value;
    }

    public string ConvertToString()
    {
        return $"< label value = '{this.value}' />";
    }

}
