using System;

namespace sudoku_brain
{
    class Program
    {
        static void Main(string[] args)
        {
            var loader = new LoadPuzzle();

            var puzzle = loader.LoadFromInputTxt("input2.txt");

            puzzle.WriteToConsole();

            int previousNumberFilledIn = 0;
            while (puzzle.ApplyAllStrats())
            {
                int currentNumberFilledIn = puzzle.NumbersFilledIn;
                if (currentNumberFilledIn > previousNumberFilledIn)
                {
                    puzzle.WriteToConsole();
                }

                previousNumberFilledIn = currentNumberFilledIn;
            }
        }
    }
}
