int TaskAmount = 10;

var tasks = new List<Task>(TaskAmount);
for (int i = 0; i < TaskAmount; i++)
{
    int index = i;
    tasks.Add(Task.Factory.StartNew(() => Console.WriteLine(Singleton.Instance.Guid)));
}
Task.WaitAll(tasks.ToArray());
Console.WriteLine();

tasks = new List<Task>(TaskAmount);
for (int i = 0; i < TaskAmount; i++)
{
    int index = i;
    tasks.Add(Task.Factory.StartNew(() => Console.WriteLine(SingletonWithLock.Instance.Guid)));
}
Task.WaitAll(tasks.ToArray());
Console.WriteLine();

tasks = new List<Task>(TaskAmount);
for (int i = 0; i < TaskAmount; i++)
{
    int index = i;
    tasks.Add(Task.Factory.StartNew(() => Console.WriteLine(SingletonWithoutLock.GetInstance().Guid)));
}
Task.WaitAll(tasks.ToArray());
Console.ReadLine();