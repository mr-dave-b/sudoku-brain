using System;
using System.Collections.Generic;
using System.Linq;
using SudokuBrain.Services;

public class Cell
{
    private IMessageLogger _log;

    public Cell(char initialValue, int row, int col, IMessageLogger log) : this(row, col, log)
    {
        if (int.TryParse(initialValue.ToString(), out _))
        {
            Value = initialValue;
            Given = true;
            Filled = true;
            Candidates = null;
        }
        else
        {
            Candidates = new HashSet<char>(Constants.AllValues);
            Value = '.';
        }
    }

    private Cell(int row, int col, IMessageLogger log)
    {
        _log = log;
        Col = col;
        Row = row;
    }

    public Cell Copy()
    {
        var copy = new Cell(this.Row, this.Col, this._log)
        {
            Given = this.Given,
            Filled = this.Filled,
            Value = this.Value,
            Candidates = this.Candidates == null ? null : new HashSet<char>(this.Candidates)
        };

        return copy;
    }

    public bool Given { get; private set; }

    public bool Filled { get; private set; }

    public char Value { get; private set; }

    public int Row { get; private set; }

    public int Col { get; private set; }

    public HashSet<char> Candidates { get; private set; }

    public bool SetOnlyCandidates(IEnumerable<char> values, string actor)
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
            FillIn(Candidates.First(), actor);
        }
        return true;
    }

    public bool EliminateCandidates(IEnumerable<char> values, string actor)
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
            FillIn(Candidates.First(), actor);
        }
        return somethingRemoved;
    }

    public bool EliminateCandidate(char value, string actor)
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
                    FillIn(Candidates.First(), actor);
                }
            }
        }
        return somethingRemoved;
    }

    public void FillIn(char candidate, string actor)
    {
        Filled = true;
        Value = candidate;
        Candidates = null;
        _log.Log(actor, $"Filled in a {candidate} at {Col},{Row}!");
    }
}