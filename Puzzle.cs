using System;
using System.Collections.Generic;

public class Puzzle
{
    private List<IStrategy> _strategies = new List<IStrategy>
    {
        new SimpleElimination(),
        new HiddenSinglesStrategy(),
        new PointingCandidatesStrategy(),
        new BoxLineReductionStrategy(),
        new NakedPairsStrategy(),
        new HiddenPairsStrategy(),
        new NakedTripletsStrategy(),
        new HiddenTripletsStrategy(),
        new XWingStrategy(),
        new SwordfishStrategy()
    };

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

    public void EndSolutionStats()
    {
        int difficulty = 0;
        Console.WriteLine("Successful strategies applied:");
        foreach(var strat in UsedStrats)
        {
            difficulty += strat.SkillLevel;
            Console.WriteLine($"{strat.Name} ({strat.SkillLevel})");
        }
        Console.Write($"Difficulty: {difficulty}");
        if (NumbersFilledIn == 81)
        {
            Console.WriteLine();
            Console.WriteLine("All Done :)");
        }
        else
        {
            Console.WriteLine("+");
            Console.WriteLine("Ran out of ideas :/");
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