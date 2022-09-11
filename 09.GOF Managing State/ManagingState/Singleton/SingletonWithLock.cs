public class SingletonWithLock
{
    private static SingletonWithLock _instance;
    private static object _lockObj = new object();
    public Guid Guid { get; }
    private SingletonWithLock()
    {
        Guid = Guid.NewGuid();
    }

    public static SingletonWithLock Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new SingletonWithLock();
                    }
                }
            }
            return _instance;
        }
    }
}

