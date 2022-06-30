/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static Random rand = new Random();
        const byte numberOfItems = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var sharedList = new List<int>();
            var waitForAdding = new AutoResetEvent(true);
            var waitForDisplaying = new AutoResetEvent(false);

            var addElementsToCollectionTask = Task.Run(()=> {

                for (int i = 0; i < numberOfItems; i++)
                {
                    waitForAdding.WaitOne();
                    sharedList.Add(rand.Next());
                    waitForDisplaying.Set();
                }
            });

            var displayElementsInCollectionTask = Task.Run(() => {

                for (int i = 0; i < numberOfItems; i++)
                {
                    waitForDisplaying.WaitOne();
                    printArrayToConsole(sharedList);
                    waitForAdding.Set();
                }
            });

            Task.WaitAll(addElementsToCollectionTask, displayElementsInCollectionTask);
            Console.ReadLine();
        }
        static void printArrayToConsole(IEnumerable<int> collection)
        {
            var sb = new StringBuilder();
            sb.Append('[');

            foreach (var item in collection)
            {
                sb.Append($"{item}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(']');

            Console.WriteLine(sb.ToString());
        }
    }
}
