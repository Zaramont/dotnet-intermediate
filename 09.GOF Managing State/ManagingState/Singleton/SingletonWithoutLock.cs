
public class SingletonWithoutLock
{
    private static readonly Lazy<SingletonWithoutLock> instance = new(() => new SingletonWithoutLock());

    public Guid Guid { get; private set; }

    private SingletonWithoutLock()
    {
        Guid = Guid.NewGuid();
    }

    public static SingletonWithoutLock GetInstance()
    {
        return instance.Value;
    }
}
