using System;
using System.Collections.Generic;
using System.Linq;

public class SimpleElimination : IStrategy
{
    public string Name => "Simple Elimination";

    public int SkillLevel => 1;

    // Iterate each row, column and box, eliminating candidates that are already filled in
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        for (int num = 1; num < 10; num++)
        {
            var group = puzzle.GetRow(num);
            progress = RemoveCandidates(group) || progress;
            group = puzzle.GetColumn(num);
            progress = RemoveCandidates(group) || progress;
            group = puzzle.GetBox(num);
            progress = RemoveCandidates(group) || progress;
        }
        return progress;
    }

    private bool RemoveCandidates(Group group)
    {
        bool somethingRemoved = false;
        var filledValues = new HashSet<char>();
        for (int i=1; i<=9; i++)
        {
            var cell = group.GetCell(i);
            if (cell.Filled)
            {
                filledValues.Add(cell.Value);
            }
        }

        // Remove filled in digits as candidates
        for (int i=1; i<=9; i++)
        {
            var cell = group.GetCell(i);
            if (!cell.Filled)
            {
                if (cell.EliminateCandidates(filledValues, Name))
                {
                    somethingRemoved = true;
                }
            }
        }
        return somethingRemoved;
    }
}