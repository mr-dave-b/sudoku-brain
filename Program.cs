using System;

namespace sudoku_brain
{
    class Program
    {
        static void Main(string[] args)
        {
            var loader = new LoadPuzzle();

            var puzzle = loader.LoadFromInputTxt("input.txt");

            puzzle.WriteToConsole();

            int previousNumberFilledIn = 0;
            while (puzzle.ApplyAllStrats())
            {
                int currentNumberFilledIn = puzzle.NumbersFilledIn;
                if (currentNumberFilledIn > previousNumberFilledIn)
                {
                    puzzle.WriteToConsole();
                }
                if (currentNumberFilledIn == 81)
                {
                    Console.WriteLine("All Done :)");
                    return;
                }
                previousNumberFilledIn = currentNumberFilledIn;
            }
        }
    }
}
