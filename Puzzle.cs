using System;

public class Puzzle
{
    private Group[] _allRows = new Group[9];

    public Puzzle(Group[] initialRows)
    {
        for (int i = 0; i < 9; i++)
        {
            _allRows[i] = initialRows[i];
        }
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

    // Applying strategies until some progress is made, then we should go round again
    public bool ApplyAllStrats()
    {
        bool progress = false;
        progress = CheckAllGroups();
        if (progress)
        {
            return progress;
        }

        // Look at each box and find candidates that are restricted to a row or column
        var strat = new BoxLineReductionStrategy();
        progress = strat.Apply(this) || progress;
        if (progress)
        {
            return progress;
        }

        var strat2 = new NakedPairsStrategy();
        progress = strat2.Apply(this) || progress;
        if (progress)
        {
            return progress;
        }

        var strat3 = new HiddenPairsStrategy();
        progress = strat3.Apply(this) || progress;
        if (progress)
        {
            return progress;
        }

        var strat4 = new HiddenTripletsStrategy();
        progress = strat4.Apply(this) || progress;
        if (progress)
        {
            return progress;
        }

        return progress;
    }

    // Returns true so long as some progress has been made
    public bool CheckAllGroups()
    {
        bool progress = false;
        // Iterate each row, column and box, eliminating candidates that are already filled in
        for (int num = 1; num < 10; num++)
        {
            var group = GetRow(num);
            progress = group.RemoveCandidates() || progress;
            group = GetColumn(num);
            progress = group.RemoveCandidates() || progress;
            group = GetBox(num);
            progress = group.RemoveCandidates() || progress;
        }
        return progress;
    }

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
        return new Group(colCells, $"column {column}");
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
        return new Group(boxCells, $"box {box}");
    }

    public void WriteToConsole()
    {
        for (int row = 1; row <= 9; row++)
        {
            if ((row-1) % 3 == 0)
            {
                Console.WriteLine("-------------");
            }
            var rowData = GetRow(row);
            for (int col = 1; col <= 9; col++)
            {
                if ((col-1) % 3 == 0)
                {
                    Console.Write("|");
                }
                Console.Write(rowData.GetCell(col).Value);
            }
            
            Console.WriteLine("|");
        }
        Console.WriteLine("-------------");
    }
}