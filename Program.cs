using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SimpleCalculatorGenerator
{
    class Program
    {
        private static StreamWriter programmCode;
        private static readonly string dirName = "Results";
        private static readonly char[] operators = new char[] { '+', '-', '*', '/' };
        private static readonly Func<int, int, int>[] actions = new Func<int, int, int>[]
        {
            (a,b) => (a + b),
            (a,b) => (a - b),
            (a,b) => (a * b),
            (a,b) => (a / b)
        };
        static void Main(string[] args)
        {
            if (Directory.Exists(dirName))
                Directory.Delete(dirName);                
            Directory.CreateDirectory(dirName);

            programmCode = new StreamWriter("Results/Program.cs");

            Console.WriteLine("Enter maximum number for calculator:");
            var sNum = Console.ReadLine();

            var maxValue = 0;
            int.TryParse(sNum, out maxValue);

            if(maxValue <= 0)
            {
                Console.WriteLine("Try again, 'mathematician'.");
            }

            LoadFilePart("header.txt");
            Console.WriteLine("Generated header.");

            var currentAction = 0;
            var totalActions = maxValue * maxValue * 4;

            for(var i = 1; i <= maxValue; ++i)
            {
                for(var j = 1; j <= maxValue; ++j)
                {
                    for(var k = 0; k < 4; ++k)
                    {
                        currentAction++;

                        programmCode.WriteLine($"\t\t\t\tif (firstNumber == {i} && sign == '{operators[k]}' && secondNumber == {j})");
                        programmCode.WriteLine( "\t\t\t\t{");
                        programmCode.WriteLine($"\t\t\t\t\tConsole.WriteLine(\"Answer is: {actions[k](i, j)}\");");
                        programmCode.WriteLine($"\t\t\t\t\tConsole.WriteLine(\"Goodbye!\");");
                        programmCode.WriteLine($"\t\t\t\t\tConsole.ReadKey();");
                        programmCode.WriteLine($"\t\t\t\t\treturn;");
                        programmCode.WriteLine(" \t\t\t\t}");

                        if (currentAction % 1000 == 0)
                        {
                            Console.WriteLine($"Passed: {currentAction.ToString().PadLeft(9)}/{totalActions} operations.");
                        }
                    }
                }
            }

            Console.WriteLine($"Passed {totalActions.ToString().PadLeft(9)}/{totalActions} operations.");

            LoadFilePart("footer.txt"); 
            Console.WriteLine("Generated footer.");

            programmCode.Close();
            programmCode.Dispose();

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void LoadFilePart(string filename)
        {
            var part = File.ReadAllLines(filename).ToList();

            part.ForEach(x => programmCode.WriteLine(x));
        }   
    }
}
