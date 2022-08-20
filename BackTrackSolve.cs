using System;
using System.Collections.Generic;
using System.Linq;
using SudokuBrain.Services;

public class BackTrackSolve : IStrategy
{
    private readonly IMessageLogger _logger;
    private readonly SimpleElimination _eliminator = new SimpleElimination();

    public int SkillLevel => 9;
    private int _searchCombos = 0;

    public BackTrackSolve(IMessageLogger logger)
    {
        _logger = logger;
    }

    public string Name => "Backtracking Solver";

    public bool Apply(Puzzle puzzle)
    {
        // Start the recursive guessing process
        var success = Guess(puzzle, 1, 1);

        _logger.Log(Name, $"{_searchCombos} incorrect solutions rejected.");

        if (success)
        {
            return true;
        }

        throw new Exception("This shouldn't fail! Invalid puzzle?");
    }

    private bool Guess(Puzzle puzzle, int startRow, int startCol)
    {
        // Make a copy of the current puzzle so we can backtrack
        // when we made a wrong guess
        var workingMemory = puzzle.Copy();

        // Update puzzle candidates before we make our guess
        _eliminator.Apply(workingMemory);

        // Make sure the puzzle is valid
        if (!workingMemory.Validate())
        {
            _searchCombos++;
            return false;
        }

        // Find the next cell that needs guessing
        for (int row = startRow; row < 10; row++)
        {
            for (int col = startCol; col < 10; col++)
            {
                var workingCell = workingMemory.GetRow(row).GetCell(col);
                if (workingCell.Filled)
                {
                    continue;
                }

                var success = false;
                var candidates = new HashSet<char>(workingCell.Candidates);

                while (!success)
                {
                    var guess = candidates.FirstOrDefault();
                    if (guess == default(char))
                    {
                        // There are no possible guesses, we are on the wrong path
                        _searchCombos++;
                        return false;
                    }

                    candidates.Remove(guess);

                    // Fill in a guess
                    workingCell.FillIn(guess, Name, backtrackSolver: true);
                    
                    // Recurse to make a guess for the next cell
                    success = Guess(workingMemory, row + 1, col);
                    
                    if (success)
                    {
                        // The puzzle has been solved - pass the solution up out of the recursion
                        puzzle.CopyFrom(workingMemory);
                        return true;
                    }
                }

                throw new Exception("I think we should never get here? 1");
            }
        }

        // There are no unfilled cells left
        puzzle.CopyFrom(workingMemory);
        return true;
    }
}