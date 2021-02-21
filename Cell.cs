using System;
using System.Collections.Generic;
using System.Linq;

public class Cell
{
    public Cell(char initialValue, int row, int col)
    {
        Col = col;
        Row = row;
        if (int.TryParse(initialValue.ToString(), out _))
        {
            Value = initialValue;
            Given = true;
            Filled = true;
        }
        else
        {
            Candidates = new HashSet<char>(Constants.AllValues);
            Value = '.';
        }
    }

    public bool Given { get; set; }

    public bool Filled { get; set; }

    public char Value { get; set; }

    public int Row { get; private set;}

    public int Col { get; private set;}

    public HashSet<char> Candidates { get; set; }

    public bool EliminateCandidates(IEnumerable<char> values)
    {
        bool somethingRemoved = false;
        if (Given || Filled)
        {
            return somethingRemoved;
        }
        foreach (var value in values)
        {
            if (Candidates.Contains(value))
            {
                Candidates.Remove(value);
                somethingRemoved = true;
            }
        }
        if (Candidates.Count == 1)
        {
            FillIn(Candidates.First());
        }
        return somethingRemoved;
    }

    internal void FillIn(char candidate)
    {
        Filled = true;
        Value = candidate;
        Console.WriteLine($"Filled in a {candidate} at {Col},{Row}!");
    }
}