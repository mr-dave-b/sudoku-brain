using System;
using System.Linq;
using System.Collections.Generic;

public class BoxLineReductionStrategy
{
    private static int[] topRow = {1, 2, 3};
    private static int[] middleRow = {4, 5, 6};
    private static int[] bottomRow = {7, 8, 9};
    private static int[] leftCol = {1, 4, 7};
    private static int[] middleCol = {2, 5, 8};
    private static int[] rightCol = {3, 6, 9};

    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        for (int boxNum = 1; boxNum < 10; boxNum++)
        {
            var box = puzzle.GetBox(boxNum);
            int rowOffset = Helpers.GetRowOffset(boxNum);
            int colOffset = Helpers.GetColOffset(boxNum);
            bool boxProgress = false;

            // See if any of the digits are candidates only in a row or a column
            foreach (var candidate in Constants.AllValues)
            {
                var cellNumbers = new HashSet<int>();
                for (int cellNum = 1; cellNum <= 9; cellNum++)
                {
                    var cell = box.GetCell(cellNum);
                    if (!cell.Given && !cell.Filled && cell.Candidates.Contains(candidate))
                    {
                        cellNumbers.Add(cellNum);
                    }
                }
                if (cellNumbers.Count > 0)
                {
                    if (cellNumbers.Count == 1)
                    {
                        Console.WriteLine($"BoxLineReductionStrategy problem: {candidate} only in cell {cellNumbers.First()} in box {boxNum} :(");
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(topRow))
                    {
                        if (RemoveCandidatesFromRow(puzzle, 1, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(middleRow))
                    {
                        if (RemoveCandidatesFromRow(puzzle, 2, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(bottomRow))
                    {
                        if (RemoveCandidatesFromRow(puzzle, 3, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(leftCol))
                    {
                        if (RemoveCandidatesFromColumn(puzzle, 1, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(middleCol))
                    {
                        if (RemoveCandidatesFromColumn(puzzle, 2, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(rightCol))
                    {
                        if (RemoveCandidatesFromColumn(puzzle, 3, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }
                        continue;
                    }
                }
            }
            if (boxProgress)
            {
                // Re-check all candidates before moving on to the next box
                puzzle.CheckAllGroups();
                progress = true;
            }
        }
        return progress;
    }

    private bool RemoveCandidatesFromRow(Puzzle puzzle, int rowNum, int rowOffset, int colOffset, char candidate, int boxNum)
    {
        bool progress = false;
        var group = puzzle.GetRow(rowNum + rowOffset);

        for (int colNum = 1; colNum <= 9; colNum++)
        {
            if (colNum > colOffset && colNum <= colOffset + 3)
            {
                // Skip the 3 cells in the current box
                continue;
            }
            if (group.GetCell(colNum).EliminateCandidates(new[] { candidate }))
            {
                progress = true;
            }
        }
        if (progress)
        {
            Console.WriteLine($"Box line reduction: {candidate}s all in a row in box {boxNum}");
        }
        return progress;
    }

    private bool RemoveCandidatesFromColumn(Puzzle puzzle, int colNum, int rowOffset, int colOffset, char candidate, int boxNum)
    {
        bool progress = false;
        var group = puzzle.GetColumn(colNum + colOffset);

        for (int rowNum = 1; rowNum <= 9; rowNum++)
        {
            if (rowNum > rowOffset && rowNum <= rowOffset + 3)
            {
                // Skip the 3 cells in the current box
                continue;
            }
            if (group.GetCell(rowNum).EliminateCandidates(new[] { candidate }))
            {
                progress = true;
            }
        }
        if (progress)
        {
            Console.WriteLine($"Box line reduction: {candidate}s all in a col in box {boxNum}");
        }
        return progress;
    }
}