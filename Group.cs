using System;
using System.Collections.Generic;
using System.Linq;
using SudokuBrain.Services;

public class Group
{
        private Cell[] _cells = new Cell[9];
        private string _description;
        private IMessageLogger _log;

        public Group(Cell[] initialCells, string description, IMessageLogger log)
        {
                _log = log;
                for (int i = 0; i < 9; i++)
                {
                        _cells[i] = initialCells[i];
                }
                _description = description;
        }

        public string Description => _description;

    public IEnumerable<char> Candidates => _cells.Where(c => !c.Filled).SelectMany(c => c.Candidates).Distinct();

    public IEnumerable<char> FilledIn => _cells.Where(c => c.Filled).Select(c => c.Value);

    public Cell GetCell(int cellNum)
        {
                return _cells[cellNum-1];
        }

        public override string ToString()
        {
                string result = "";
                foreach (var cell in _cells)
                {
                        if (cell.Filled)
                        {
                                result += cell.Value;
                        }
                        else
                        {
                                result += ' ';
                        }
                }
                return result;
        }

    internal bool IsValid()
    {
            var valid = true;
            var candidates = new HashSet<char>();
            var placed = new HashSet<char>();
            foreach (var cell in _cells)
            {
                    if (cell.Filled)
                    {
                            if (placed.Contains(cell.Value))
                            {
                                    _log.Log(null, $"{cell.Value} filled in more than once in {this.Description}!", SudokuBrain.Models.LogItemLevel.Problem);
                                    valid = false;
                            }
                        placed.Add(cell.Value);
                    }
                    else
                    {
                        candidates.UnionWith(cell.Candidates);
                    }

            }
            placed.UnionWith(candidates);
            var allNumbers = new HashSet<char>(Constants.AllValues);
            foreach (var ch in placed)
            {
                allNumbers.Remove(ch);
            }
            if (allNumbers.Count > 0)
            {
                _log.Log(null, $"{allNumbers.First()} is missing from {this.Description}!", SudokuBrain.Models.LogItemLevel.Problem);
                valid = false;
            }

        return valid;
    }
}