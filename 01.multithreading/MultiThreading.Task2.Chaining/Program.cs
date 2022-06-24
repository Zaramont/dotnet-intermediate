/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private static Random rand = new Random();
        const byte numberOfItems = 10;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var arrayOfRandomNumbers = Task.Run(() => CreateArray(numberOfItems));
            var arrayMultipliedByRandomInt = arrayOfRandomNumbers.ContinueWith(antecedent => MultiplyArrayByNumber(antecedent));
            var sortedByAscendingArray = arrayMultipliedByRandomInt.ContinueWith(antecedent => SortArray(antecedent));
            var averageValue = sortedByAscendingArray.ContinueWith(antecedent => CountAverageValue(antecedent));

            averageValue.Wait();
            Console.ReadLine();
        }

        private static int CountAverageValue(Task<int[]> antecedent)
        {
            var numbers = antecedent.Result;
            int sumOfNumbers = 0;

            foreach (int number in numbers)
            {
                sumOfNumbers += number;
            }

            int result = sumOfNumbers / numbers.Length;
            Console.WriteLine($"Average value: {result}");
            return result;

        }

        private static int[] SortArray(Task<int[]> antecedent)
        {
            var numbers = antecedent.Result;
            Array.Sort(numbers);

            printArrayToConsole(numbers, "Sorted array:");
            return numbers;
        }

        private static int[] MultiplyArrayByNumber(Task<int[]> antecedent)
        {
            var numbers = antecedent.Result;
            int multiplier = rand.Next();

            for (int index = 0; index < numberOfItems; index++)
            {
                numbers[index] = numbers[index] * multiplier;
            }

            printArrayToConsole(numbers, "Multiplied by random integer array:");
            return numbers;
        }

        private static int[] CreateArray(int size)
        {

            int[] numbers = new int[size];

            for (int index = 0; index < size; index++)
            {
                numbers[index] = rand.Next();
            }

            printArrayToConsole(numbers, "Initial array:");
            return numbers;

        }

        static void printArrayToConsole(int[] array, string label)
        {
            var sb = new StringBuilder();
            sb.AppendLine(label);
            sb.Append('[');

            foreach (var item in array)
            {
                sb.Append($"{item}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(']');

            Console.WriteLine(sb.ToString());
        }
    }
}
