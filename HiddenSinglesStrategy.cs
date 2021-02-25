using System;
using System.Linq;

public class HiddenSinglesStrategy : IStrategy
{
    public string Name => "Hidden Singles Strategy";

    public int SkillLevel => 2;
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        // Iterate each row, column and box, looking for a candidate in only 1 cell
        for (int num = 1; num <= 9; num++)
        {
            var group = puzzle.GetRow(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetColumn(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetBox(num);
            progress = CheckGroup(group) || progress;
        }
        return progress;
    }

    private bool CheckGroup(Group group)
    {
        bool progress = false;
        // Check for numbers that are only candidates in 1 place
        foreach (var candidate in Constants.AllValues)
        {
            Cell foundInCell = null;
            for (int i=1; i<=9; i++)
            {
                var cell = group.GetCell(i);

                if (cell.Filled)
                {
                    if (cell.Value == candidate)
                    {
                        foundInCell = null;
                        break;
                    }
                }
                else
                {
                    if (cell.Candidates.Contains(candidate))
                    {
                        if (foundInCell != null)
                        {
                            // Found in more than 1 cell, reject
                            foundInCell = null;
                            break;
                        }
                        foundInCell = cell;
                    }
                }
            }

            if (foundInCell != null)
            {
                // We have a cell to fill in!
                foundInCell.FillIn(candidate);
                progress = true;
            }
        }
        
        return progress;
    }
}