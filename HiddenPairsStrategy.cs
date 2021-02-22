using System;
using System.Collections.Generic;
using System.Linq;

public class HiddenPairsStrategy
{
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        // Iterate each row, column and box, looking for 2 candidates that are only in 2 cells
        for (int num = 1; num < 10; num++)
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
        // Get all the candidates in the group
        var candidates = new HashSet<char>(Constants.AllValues);
        for (int cell = 1; cell <= 9; cell++)
        {
            var cellData = group.GetCell(cell);
            if (cellData.Given || cellData.Filled)
            {
                candidates.Remove(cellData.Value);
            }
        }

        // Iterate all pairs of possible candidates
        Console.WriteLine($"Hidden pairs: searching in {group.Description} with {candidates.Count*(candidates.Count-1)} pair combos");
        foreach (var candidate1 in candidates)
        {
            foreach (var candidate2 in candidates.Where(x => x != candidate1))
            {
                // Find all cells containing both of the candidates
                var rejected = false;
                var inCells = new List<int>();
                for (int cell = 1; cell <= 9; cell++)
                {
                    var cellData = group.GetCell(cell);
                    if (cellData.Given || cellData.Filled)
                    {
                        continue;
                    }
                    if (cellData.Candidates.Contains(candidate1))
                    {
                        if (cellData.Candidates.Contains(candidate2))
                        {
                            if (inCells.Count > 1)
                            {
                                // This pair is in too many cells
                                rejected = true;
                                break;
                            }
                            inCells.Add(cell);
                        }
                        else
                        {
                            rejected = true;
                            break;
                        }
                    }
                    else if (cellData.Candidates.Contains(candidate2))
                    {
                        rejected = true;
                        break;
                    }
                }
                if (rejected)
                {
                    continue;
                }
                if (inCells.Count == 2)
                {
                    // We found a hidden pair! Expose it
                    foreach (var cellNum in inCells)
                    {
                        var cellData = group.GetCell(cellNum);
                        if (cellData.SetOnlyCandidates(new char[] {candidate1, candidate2}))
                        {
                            Console.WriteLine($"Hidden pairs: hidden pair {candidate1}{candidate2} exposed in {group.Description}");
                            // Re-check all candidates before moving on to the next group
                            //puzzle.CheckAllGroups();
                            progress = true;
                        }
                    }
                }
            }
        }
        return progress;
    }
}