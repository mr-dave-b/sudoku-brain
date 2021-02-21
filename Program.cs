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

            while (puzzle.CheckAllGroups())
            {
                puzzle.WriteToConsole();
            }
        }
    }
}
