using System;
using System.Collections.Generic;

public class Group
{
        private char[] allValues = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private Cell[] _cells = new Cell[9];

        public Group(Cell[] initialCells)
        {
                for (int i = 0; i < 9; i++)
                {
                        _cells[i] = initialCells[i];
                }
        }

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

    internal bool RemoveCandidates()
    {
            bool somethingRemoved = false;
            var filledValues = new HashSet<char>();
            foreach (var cell in _cells)
            {
                    if (cell.Filled)
                    {
                        filledValues.Add(cell.Value);
                    }
            }

        // Remove filled in digits as candidates
        foreach (var cell in _cells)
            {
                    if (!cell.Filled)
                    {
                            if (cell.EliminateCandidates(filledValues))
                            {
                                somethingRemoved = true;
                            }
                    }
            }

            // Check for numbers that are only candidates in 1 place
        foreach (var candidate in allValues)
            {
                    int count = 0;
                    foreach (var cell in _cells)
                        {
                                if (cell.Filled)
                                {
                                        if (cell.Value == candidate)
                                        {
                                                count = -1;
                                                break;
                                        }
                                }
                                else
                                {
                                        if (cell.Candidates.Contains(candidate))
                                        {
                                                count++;
                                        }
                                }
                        }
                        if (count == 1)
                        {
                                // We have a cell to fill in!
                                foreach (var cell in _cells)
                                {
                                        if (!cell.Filled && cell.Candidates.Contains(candidate))
                                        {
                                                cell.FillIn(candidate);
                                                break;
                                        }
                                }
                        }
            }

            return somethingRemoved;
    }
}