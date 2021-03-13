using System;
using System.Linq;
using SudokuBrain.Services;

public class NakedPairsStrategy : IStrategy
{
    private readonly IMessageLogger _logger;

    public NakedPairsStrategy(IMessageLogger logger)
    {
        _logger = logger;
    }

    public string Name => "Naked Pairs Strategy";

    public int SkillLevel => 4;
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        // Iterate each row, column and box, looking for pairs with only 2 options
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
        for (int firstCell = 1; firstCell <= 8; firstCell++)
        {
            var firstCellData = group.GetCell(firstCell);
            if (firstCellData.Given || firstCellData.Filled)
            {
                continue;
            }
            if (firstCellData.Candidates.Count == 2)
            {
                // See if any of the other cells are a pair with this one
                for (int secondCell = firstCell+1; secondCell <= 9; secondCell++)
                {
                    var secondCellData = group.GetCell(secondCell);
                    if (secondCellData.Given || secondCellData.Filled)
                    {
                        continue;
                    }
                    if (secondCellData.Candidates.SetEquals(firstCellData.Candidates))
                    {
                        bool pairProgress = false;
                        // This is a naked pair - remove candidates from all other cells!
                        for (int cellNum = 1; cellNum <= 9; cellNum++)
                        {
                            if (cellNum == firstCell || cellNum == secondCell)
                            {
                                continue;
                            }

                            var cell = group.GetCell(cellNum);
                            if (!cell.Given && !cell.Filled)
                            {
                                if (cell.EliminateCandidates(firstCellData.Candidates))
                                {
                                    pairProgress = true;
                                }
                            }
                        }
                        if (pairProgress)
                        {
                            _logger.Log(Name, $"{firstCellData.Candidates.First()}{firstCellData.Candidates.Skip(1).First()} in {group.Description} helps us");
                            progress = true;
                        }
                    }
                }
            }
        }
        return progress;
    }
}