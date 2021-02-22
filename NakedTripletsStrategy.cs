using System;
using System.Linq;

public class NakedTripletsStrategy
{
    // TODO: NOT WRITTEN YET
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        // Iterate each row, column and box, looking for 3 cells where 3 candidates must be in them
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
        // Easy version only...
        // find first cell with 2 or 3 candidates
        // find another cell with 3 candidates containing first 2 or 2 different candidates
        // find 3rd cell with same candidates

        // TODO: This is too hard!

        bool progress = false;
        for (int firstCell = 1; firstCell <= 7; firstCell++)
        {
            var firstCellData = group.GetCell(firstCell);
            if (firstCellData.Given || firstCellData.Filled)
            {
                continue;
            }
            if (firstCellData.Candidates.Count == 2 || firstCellData.Candidates.Count == 3)
            {
                // Find another cell that might work with this one
                for (int secondCell = firstCell+1; secondCell <= 8; secondCell++)
                {
                    var secondCellData = group.GetCell(secondCell);
                    if (secondCellData.Given || secondCellData.Filled)
                    {
                        continue;
                    }
                    if (firstCellData.Candidates.Count == 3)
                    {
                        if (secondCellData.Candidates.SetEquals(firstCellData.Candidates))
                        {
                            for (int thirdCell = secondCell+1; thirdCell <= 9; thirdCell++)
                            {
                                var thirdCellData = group.GetCell(thirdCell);
                                if (thirdCellData.Given || thirdCellData.Filled)
                                {
                                    continue;
                                }
                                if (thirdCellData.Candidates.IsSubsetOf(firstCellData.Candidates))
                                {
                                    bool tripleProgress = false;
                                    // This is a naked triple - remove all other candidates!

                                    //TODO: See the pairs one

                                }
                            }
                        }
                    }
                }
            }
        }
        return progress;
    }
}
