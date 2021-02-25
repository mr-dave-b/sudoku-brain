using System;

namespace sudoku_brain
{
    class Program
    {
        static void Main(string[] args)
        {
            var loader = new LoadPuzzle();

            var puzzle = loader.LoadFromInputTxt(args[0]);

            puzzle.WriteToConsole();

            int previousNumberFilledIn = puzzle.NumbersFilledIn;
            while (puzzle.ApplyAllStrats())
            {
                int currentNumberFilledIn = puzzle.NumbersFilledIn;
                if (currentNumberFilledIn > previousNumberFilledIn)
                {
                    puzzle.WriteToConsole();
                }
                if (currentNumberFilledIn == 81)
                {
                    puzzle.EndSolutionStats();
                    return;
                }
                previousNumberFilledIn = currentNumberFilledIn;
            }
            puzzle.EndSolutionStats();
        }
    }
}
