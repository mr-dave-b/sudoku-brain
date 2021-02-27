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

    public HashSet<char> Candidates { get; private set; }

    public bool SetOnlyCandidates(IEnumerable<char> values)
    {
        if (Filled)
        {
            return false;
        }
        var oldCount = Candidates.Count;
        Candidates.IntersectWith(values);
        if (oldCount == Candidates.Count)
        {
            return false;
        }
        if (Candidates.Count == 1)
        {
            FillIn(Candidates.First());
        }
        return true;
    }

    public bool EliminateCandidates(IEnumerable<char> values)
    {
        bool somethingRemoved = false;
        if (!Filled)
        {
            foreach (var value in values)
            {
                if (Candidates.Contains(value))
                {
                    Candidates.Remove(value);
                    somethingRemoved = true;
                }
            }
        }
        if (Candidates.Count == 1)
        {
            FillIn(Candidates.First());
        }
        return somethingRemoved;
    }

    public bool EliminateCandidate(char value)
    {
        bool somethingRemoved = false;
        if (!Filled)
        {
            if (Candidates.Contains(value))
            {
                Candidates.Remove(value);
                somethingRemoved = true;
                if (Candidates.Count == 1)
                {
                    FillIn(Candidates.First());
                }
            }
        }
        return somethingRemoved;
    }

    internal void FillIn(char candidate)
    {
        Filled = true;
        Value = candidate;
        Candidates = null;
        Console.WriteLine($"Filled in a {candidate} at {Col},{Row}!");
    }
}