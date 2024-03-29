using System;
using System.Collections.Generic;
using System.Text;
using SudokuBrain.Services;

public class Puzzle
{
    private readonly Group[] _allRows = new Group[9];
    private readonly IMessageLogger _logger;
    private PuzzleMetadata _info;

    public Puzzle(Group[] initialRows, IMessageLogger logger, string filename = "", string title = "", string description = "")
    {        
        for (int i = 0; i < 9; i++)
        {
            _allRows[i] = initialRows[i];
        }
        _logger = logger;
        _info = new PuzzleMetadata(filename, title, description);
    }

    public Dictionary<string, int> UsedStrats {get;} = new Dictionary<string, int>();

    public Puzzle Copy()
    {
        var rows = new Group[9];
        for (int r = 0; r < 9; r++)
        {
            rows[r] = _allRows[r].Copy();
        }
        var copy = new Puzzle(rows, _logger, FileName, Title, Description);
        foreach (var strat in UsedStrats)
        {
            copy.UsedStrats.Add(strat.Key, strat.Value);
        }
        return copy;
    }

    public string FileName => _info.FileName;

    public string Title => _info.Title;

    public string Description => _info.Description;    

    public Cell GetCell(int column, int row)
    {
        return null;
    }

    public int CountFilledInCells
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

    public Group GetRow(int row)
    {
        return _allRows[row-1];
    }

    // Updates the grid with that from sourcePuzzle
    internal void CopyFrom(Puzzle sourcePuzzle)
    {
        for (int i = 0; i < 9; i++)
        {
            _allRows[i] = sourcePuzzle.GetRow(i+1);
        }
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

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int row = 1; row <= 9; row++)
        {
            var rowData = GetRow(row);
            for (int col = 1; col <= 9; col++)
            {
                sb.Append(rowData.GetCell(col).Value);
            }
        }
        return sb.ToString();
    }
}