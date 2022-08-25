var foodElements = new Elements<string>(new[] { "apple", "banana", "watermelon" });
var adaptedElements = new AdaptedElements<string>(foodElements);
var printer = new Printer();
printer.Print(adaptedElements);

public class Printer
{
    public void Print<T>(IContainer<T> container)
    {

        foreach (var item in container.Items)
        {
            Console.WriteLine(item.ToString());
        }
    }
}


public interface IContainer<T>

{
    IEnumerable<T> Items { get; }

    int Count { get; }
}

public interface IElements<T>

{
    IEnumerable<T> GetElements();
}

public class Elements<T> : IElements<T>
{
    private readonly IEnumerable<T> _collection;

    public Elements(IEnumerable<T> collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    public IEnumerable<T> GetElements()
    {
        return this._collection;
    }
}

public class AdaptedElements<T> : IContainer<T>
{
    private readonly IElements<T> _elements;

    public AdaptedElements(IElements<T> elements)
    {
        _elements = elements ?? throw new ArgumentNullException(nameof(elements));
    }

    public IEnumerable<T> Items => this._elements.GetElements();

    public int Count => this.Items.Count();
}