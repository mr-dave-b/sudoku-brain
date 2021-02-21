using System;
using System.Collections.Generic;
using System.Linq;

public class Cell
{
    private char[] allValues = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    public Cell(char initialValue)
    {
        if (int.TryParse(initialValue.ToString(), out _))
        {
            Value = initialValue;
            Given = true;
            Filled = true;
        }
        else
        {
            Candidates = new HashSet<char>(allValues);
            Value = '.';
        }
    }

    public bool Given { get; set; }

    public bool Filled { get; set; }

    public char Value { get; set; }

    public HashSet<char> Candidates { get; set; }

    public bool EliminateCandidates(IEnumerable<char> values)
    {
        bool somethingRemoved = false;
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
        Console.WriteLine("Filled in a " + candidate + "!");
    }
}