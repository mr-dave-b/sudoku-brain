using System;
using System.Linq;
using System.Collections.Generic;

public class BoxLineReductionStrategy : IStrategy
{
    public string Name => "Box line reduction strategy";
    public int SkillLevel => 4;

    // For each row and column, check if all instances of a candidate are in the same box
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        _puzzle = puzzle;
        for (int num = 1; num <= 9; num++)
        {
            var line = puzzle.GetColumn(num);
            progress = ApplyToLine(line, num, true) || progress;
            line = puzzle.GetRow(num);
            progress = ApplyToLine(line, num, false) || progress;
        }
        return progress;
    }

    private Puzzle _puzzle;

    public bool ApplyToLine(Group line, int lineNum, bool isColumn)
    {
        bool progress = false;

        foreach (var candidate in line.Candidates)
        {
            // Check if this digit is a candidate in only 2 or 3 locations
            var cellNumbers = new HashSet<int>();
            for (int cellNum = 1; cellNum <= 9; cellNum++)
            {
                var cell = line.GetCell(cellNum);
                if (!cell.Filled && cell.Candidates.Contains(candidate))
                {
                    cellNumbers.Add(cellNum);
                }
            }
            if (cellNumbers.Count == 2 || cellNumbers.Count == 3)
            {
                // Check if our candidates are all in the same box number
                int boxNum = 0;
                bool valid = true;
                foreach (var cellNum in cellNumbers)
                {
                    int currentBoxNum;
                    if (isColumn)
                    {
                        currentBoxNum = Helpers.BoxNumber(lineNum, cellNum);
                    }
                    else
                    {
                        currentBoxNum = Helpers.BoxNumber(cellNum, lineNum);
                    }   
                    if (boxNum == 0)
                    {
                        // First cell - record the box number
                        boxNum = currentBoxNum;
                    }
                    else if (boxNum != currentBoxNum)
                    {
                        // Subsequent cell was not in the same box number
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    // Remove this candidate from all the other cells in the box
                    IList<int> skipCells;
                    if (isColumn)
                    {
                        if (lineNum % 3 == 1)
                        {
                            skipCells = Constants.BoxLeftCol;
                        }
                        else if (lineNum % 3 == 2)
                        {
                            skipCells = Constants.BoxMiddleCol;
                        }
                        else
                        {
                            skipCells = Constants.BoxRightCol;
                        }
                    }
                    else
                    {
                        if (lineNum % 3 == 1)
                        {
                            skipCells = Constants.BoxTopRow;
                        }
                        else if (lineNum % 3 == 2)
                        {
                            skipCells = Constants.BoxMiddleRow;
                        }
                        else
                        {
                            skipCells = Constants.BoxBottomRow;
                        }
                    }

                    var box = _puzzle.GetBox(boxNum);
                    bool boxProgress = false;
                    for (int b = 1; b <= 9; b++)
                    {
                        if (!skipCells.Contains(b))
                        {
                            // This cell isnt in our column/row - remove the candidate
                            var boxCell = box.GetCell(b);
                            if (boxCell.EliminateCandidate(candidate))
                            {
                                boxProgress = true;
                            }
                        }
                    }
                    if (boxProgress)
                    {
                        progress = true;
                        if (isColumn)
                        {
                            Console.WriteLine($"Box line reduction win: {candidate}s all in a column in box {boxNum}");
                        }
                        else
                        {
                            Console.WriteLine($"Box line reduction win: {candidate}s all in a row in box {boxNum}");
                        }
                    }
                }
            }
        }
        return progress;
    }
}
