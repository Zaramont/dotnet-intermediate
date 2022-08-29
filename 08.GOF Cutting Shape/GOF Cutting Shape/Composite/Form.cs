public class Form: IXmlElement
{
    private readonly string name;
    private readonly List<IXmlElement> children = new List<IXmlElement>();

    public Form(string name)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Form AddComponent(IXmlElement element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        this.children.Add(element);
        return this;
    }

    public string ConvertToString()
    {
        string newLine = Environment.NewLine;
        string childrenValue = String.Join( newLine, this.children.Select(element => '\t' + element.ConvertToString()));
        return $"<form name='myForm'>{newLine}{childrenValue}{newLine}</ form >";
    }
}