/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore semaphore = new Semaphore(0, 2);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");
            Console.WriteLine();

            var t = new Thread(() => CreateTreads(10));
            t.Start();
            t.Join();

            ThreadPool.QueueUserWorkItem(CreateTreadsByThreadPool, 10);
            semaphore.WaitOne();

            Console.ReadLine();
        }

        private static void CreateTreads(int remainedAmountOfThreads)
        {
            if (remainedAmountOfThreads <= 0)
            {
                return;
            }

            remainedAmountOfThreads--;
            Console.WriteLine($"Remained amount of threads: {remainedAmountOfThreads}.");
            var t = new Thread(() => CreateTreads(remainedAmountOfThreads));
            t.Start();
            t.Join();
        }

        private static void CreateTreadsByThreadPool(object state)
        {
            int remainedAmountOfThreads = (int)state;
            if (remainedAmountOfThreads <= 0)
            {
                semaphore.Release();
                return;
            }

            remainedAmountOfThreads--;
            Console.WriteLine($"Remained amount of threads: {remainedAmountOfThreads}.");
            ThreadPool.QueueUserWorkItem(CreateTreadsByThreadPool, remainedAmountOfThreads);
            semaphore.WaitOne();
        }
    }
}
