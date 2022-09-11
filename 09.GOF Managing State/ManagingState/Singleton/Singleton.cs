
public class Singleton
{
    private static Singleton _instance;

    public Guid Guid { get; }

    private Singleton()
    {
        Guid = Guid.NewGuid();
    }

    public static Singleton Instance => _instance ??= new Singleton();
}

