using System;
using System.Collections.Generic;
using SudokuBrain.Services;

public class Puzzle
{
    private readonly List<IStrategy> _strategies;
    private readonly Group[] _allRows = new Group[9];
    private readonly IMessageLogger _logger;

    public Puzzle(Group[] initialRows, IMessageLogger logger)
    {
        for (int i = 0; i < 9; i++)
        {
            _allRows[i] = initialRows[i];
        }
        _logger = logger;
        
        _strategies = new List<IStrategy>
        {
            new SimpleElimination(),
            new HiddenSinglesStrategy(),
            new PointingCandidatesStrategy(_logger),
            new BoxLineReductionStrategy(_logger),
            new NakedPairsStrategy(_logger),
            new HiddenPairsStrategy(_logger),
            new NakedTripletsStrategy(_logger),
            new HiddenTripletsStrategy(_logger),
            new XWingStrategy(_logger),
            new SwordfishStrategy(_logger)
        };
    }

    public Cell GetCell(int column, int row)
    {
        return null;
    }

    public int NumbersFilledIn
    {
        get
        {
            int filledIn = 0;
            foreach(var row in _allRows)
            {
                for (var col = 1; col <=9; col++)
                {
                    if (row.GetCell(col).Filled)
                    {
                        filledIn++;
                    }
                }
            }
            return filledIn;
        }
    }

    public void EndSolutionStats(IMessageLogger logger)
    {
        int difficulty = 0;
        foreach(var strat in UsedStrats)
        {
            difficulty += strat.SkillLevel;
            logger.Log(null, $"{strat.Name} ({strat.SkillLevel})");

        }
        if (NumbersFilledIn == 81)
        {
            logger.Log(null, $"Difficulty: {difficulty}");
            logger.Log(null, "All Done :)");
        }
        else
        {
            logger.Log(null, $"Difficulty: {difficulty}+");
            logger.Log(null, "Ran out of ideas :/", SudokuBrain.Models.LogItemLevel.Problem);
        }
    }

    // Applying strategies until some progress is made, then we should go round again
    public bool ApplyAllStrats()
    {
        foreach (var strat in _strategies)
        {
            if (strat.Apply(this))
            {
                if (!UsedStrats.Contains(strat))
                {
                    UsedStrats.Add(strat);
                }
                if (!Validate())
                {
                    throw new Exception("Quitting because validation failed");
                }
                return true;
            }
        }
        return false;
    }

    public bool Validate()
    {
        bool valid = true;
        for (int num = 1; num < 10; num++)
        {
            var group = GetRow(num);
            valid = group.IsValid() && valid;
            group = GetColumn(num);
            valid = group.IsValid() && valid;
            group = GetBox(num);
            valid = group.IsValid() && valid;
        }
        return valid;
    }

    public List<IStrategy> UsedStrats {get;} = new List<IStrategy>();

    public Group GetRow(int row)
    {
        return _allRows[row-1];
    }

    public Group GetColumn(int column)
    {
        var colCells = new Cell[9];
        for (int row = 1; row < 10; row++)
        {
            colCells[row-1] = GetRow(row).GetCell(column);
        }
        return new Group(colCells, $"column {column}", _logger);
    }  

    public Group GetBox(int box)
    {
        var boxCells = new Cell[9];
        int rowOffset = Helpers.GetRowOffset(box);
        int colOffset = Helpers.GetColOffset(box);
        for (int row = 1; row <= 3; row++)
        {
            var rowData = GetRow(row + rowOffset);
            for (int col = 1; col < 4; col++)
            {
                boxCells[(3*(row-1)) + col - 1] = rowData.GetCell(col + colOffset);
            }
        }
        return new Group(boxCells, $"box {box}", _logger);
    }

    public void WriteGridAsText(IMessageLogger logger)
    {
        for (int row = 1; row <= 9; row++)
        {
            if ((row-1) % 3 == 0)
            {
                logger.Log(null, "-------------");
            }
            var rowData = GetRow(row);
            var msg = "";
            for (int col = 1; col <= 9; col++)
            {
                if ((col-1) % 3 == 0)
                {
                    msg += "|";
                }
                msg += rowData.GetCell(col).Value;
            }
            logger.Log(null, msg + "|");
        }
        logger.Log(null, "-------------");
    }
}