/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            ExampleOfTaskA("TaskA");

            ExampleOfTaskB("TaskB");

            ExampleOfTaskC("TaskC");

            ExampleOfTaskD("TaskD");

            Console.ReadLine();
        }



        private static void ExampleOfTaskA(string taskName)
        {
            try
            {
                Console.WriteLine();
                var task1 = Task.Run(() => FuncWithError($"{taskName}"));
                task1.ContinueWith(antecedent => Console.WriteLine($"{taskName} continuation works in any case."));
                var task2 = Task.Run(() => SuccessfulFunc($"{taskName}"));
                task2.ContinueWith(antecedent => Console.WriteLine($"{taskName} continuation works in any case."));
                Task.WaitAll(task1, task2);
            }
            catch { }
        }

        private static void ExampleOfTaskB(string taskName)
        {
            try
            {
                Console.WriteLine();
                var task1 = Task.Run(() => FuncWithError($"{taskName}"));
                task1.ContinueWith(antecedent => Console.WriteLine($"{taskName} continuation works only with task without success."), TaskContinuationOptions.OnlyOnFaulted);
                var task2 = Task.Run(() => SuccessfulFunc($"{taskName}"));
                task2.ContinueWith(antecedent => Console.WriteLine($"{taskName} continuation shouldn't work."), TaskContinuationOptions.OnlyOnFaulted);

                Task.WaitAll(task1, task2);
            }
            catch { }
        }

        private static void ExampleOfTaskC(string taskName)
        {
            try
            {
                Console.WriteLine();
                var task1 = Task.Run(() =>
                {
                    Console.WriteLine($"{taskName} started in {Thread.CurrentThread.ManagedThreadId.ToString()} thread.");
                    FuncWithError($"{taskName}");
                });
                task1.ContinueWith(antecedent =>
                {
                    Console.WriteLine($"{taskName} continuation in {Thread.CurrentThread.ManagedThreadId} thread.");
                    Console.WriteLine($"{taskName} continuation works only with task without success.");

                }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

                var task2 = Task.Run(() => SuccessfulFunc($"{taskName}"));
                task2.ContinueWith(antecedent => Console.WriteLine($"{taskName} continuation shouldn't work."), TaskContinuationOptions.OnlyOnFaulted);

                Task.WaitAll(task1, task2);
            }
            catch { }
        }

        private static void ExampleOfTaskD(string taskName)
        {
            try
            {
                Console.WriteLine();
                var cts = new CancellationTokenSource();
                cts.Cancel();

                var task1 = Task.Run(() => SuccessfulFunc($"{taskName}"), cts.Token);
                task1.ContinueWith(antecedent =>
                {
                    if (!Thread.CurrentThread.IsThreadPoolThread)
                    {
                        Console.WriteLine($"{taskName} prevent use of a pooled thread for continuation.");
                    }
                }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

                task1.Wait();
            }
            catch { }
        }

        private static void SuccessfulFunc(string task) => Console.WriteLine($"{task} is successful.");
        private static void FuncWithError(string task)
        {
            Console.WriteLine($"{task} is ended with error.");
            throw new InvalidOperationException();
        }
    }
}
