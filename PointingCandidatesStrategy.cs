using System;
using System.Linq;
using System.Collections.Generic;

public class PointingCandidatesStrategy : IStrategy
{
    public string Name => "Pointing Pairs/Triples strategy";
    public int SkillLevel => 3;

    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        for (int boxNum = 1; boxNum < 10; boxNum++)
        {
            var box = puzzle.GetBox(boxNum);
            int rowOffset = Helpers.GetRowOffset(boxNum);
            int colOffset = Helpers.GetColOffset(boxNum);
            bool boxProgress = false;

            // See if any of the candidates appear only in a row or a column
            foreach (var candidate in box.Candidates)
            {
                var cellNumbers = new HashSet<int>();
                for (int cellNum = 1; cellNum <= 9; cellNum++)
                {
                    var cell = box.GetCell(cellNum);
                    if (!cell.Filled && cell.Candidates.Contains(candidate))
                    {
                        cellNumbers.Add(cellNum);
                    }
                }
                if (cellNumbers.Count == 2 || cellNumbers.Count == 3)
                {
                    if (cellNumbers.IsSubsetOf(Constants.BoxTopRow))
                    {
                        if (RemoveCandidatesFromRow(puzzle, 1, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(Constants.BoxMiddleRow))
                    {
                        if (RemoveCandidatesFromRow(puzzle, 2, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(Constants.BoxBottomRow))
                    {
                        if (RemoveCandidatesFromRow(puzzle, 3, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(Constants.BoxLeftCol))
                    {
                        if (RemoveCandidatesFromColumn(puzzle, 1, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(Constants.BoxMiddleCol))
                    {
                        if (RemoveCandidatesFromColumn(puzzle, 2, rowOffset, colOffset, candidate, boxNum))
                        {
                            boxProgress = true;
                        }                        
                        continue;
                    }
                    if (cellNumbers.IsSubsetOf(Constants.BoxRightCol))
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
            if (group.GetCell(colNum).EliminateCandidate(candidate))
            {
                progress = true;
            }
        }
        if (progress)
        {
            Console.WriteLine($"Pointing candidates: {candidate}s all in a row in box {boxNum}");
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
            if (group.GetCell(rowNum).EliminateCandidate(candidate))
            {
                progress = true;
            }
        }
        if (progress)
        {
            Console.WriteLine($"Pointing candidates: {candidate}s all in a col in box {boxNum}");
        }
        return progress;
    }
}