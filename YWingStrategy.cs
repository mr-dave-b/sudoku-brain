using System;
using System.Collections.Generic;
using System.Linq;
using SudokuBrain.Services;

public class YWingStrategy : IStrategy
{
    private readonly IMessageLogger _logger;

    public YWingStrategy(IMessageLogger logger)
    {
        _logger = logger;
    }

    public string Name => "Y-Wing";

    public int SkillLevel => 9;

    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        // For every 2 candidate cell, look for other cells it can see that also have 2 candidates
        for (int r = 1; r <= 9; r++)
        {
            var row = puzzle.GetRow(r);
            for (int c = 1; c <= 9; c++)
            {
                var hingeCell = row.GetCell(c);
                if (!hingeCell.Filled && hingeCell.Candidates.Count == 2)
                {
                    // Search the row, col and box for other cells with 2 candidates where 1 candidate is the same
                    var col = puzzle.GetColumn(c);
                    var box = puzzle.GetBox(Helpers.BoxNumber(c, r));
                    var potentialCells = new HashSet<Cell>();
                    for (int i = 1; i <= 9; i++)
                    {
                        if (i != c)
                        {
                            var rowCell = row.GetCell(i);
                            if (rowCell.TwoCandidatesWith1Intersection(hingeCell))
                            {
                                potentialCells.Add(rowCell);
                            }
                        }
                        if (i != r)
                        {
                            var colCell = col.GetCell(i);
                            if (colCell.TwoCandidatesWith1Intersection(hingeCell))
                            {
                                potentialCells.Add(colCell);
                            }
                        }
                        var boxCell = box.GetCell(i);
                        if (boxCell.Col != c && boxCell.Row != r)
                        {
                            if (boxCell.TwoCandidatesWith1Intersection(hingeCell))
                            {
                                potentialCells.Add(boxCell);
                            }
                        }

                        // Find any pairs of cells that make sense????
                        var potentials = potentialCells.ToArray();
                        for (int p1 = 0; p1 < potentialCells.Count - 1; p1++)
                        {
                            for (int p2 = p1 + 1; p2 < potentialCells.Count; p2++)
                            {
                                var potential1 = potentials[p1];
                                var potential2 = potentials[p2];

                                var intersectionCandidates = potential1.Candidates.Intersect(potential2.Candidates);
                                if (intersectionCandidates.Count() != 1)
                                {
                                    // Y Wing cells need 1 candidate in common
                                    break;
                                }
                                var intersectionCandidate = intersectionCandidates.First();
                                if (hingeCell.Candidates.Contains(intersectionCandidate))
                                {
                                    // Candidate in common needs to be something not in the hinge cell
                                    break;
                                }

                                // We have found a Y Wing - find any candidates to delete
                                // Get all the cells seen by p1 and p2 and remove candidate from the intersecting cells
                                HashSet<Cell> seen1 = new HashSet<Cell>(puzzle.GetRow(potential1.Row).Cells);
                                seen1.UnionWith(puzzle.GetColumn(potential1.Col).Cells);
                                seen1.UnionWith(puzzle.GetBox(Helpers.BoxNumber(potential1.Col, potential1.Row)).Cells);

                                HashSet<Cell> seen2 = new HashSet<Cell>(puzzle.GetRow(potential2.Row).Cells);
                                seen2.UnionWith(puzzle.GetColumn(potential2.Col).Cells);
                                seen2.UnionWith(puzzle.GetBox(Helpers.BoxNumber(potential2.Col, potential2.Row)).Cells);

                                seen1.IntersectWith(seen2);
                                seen1.Remove(potential1);
                                seen1.Remove(potential2);

                                var latestProgress = false;

                                foreach (var elimCell in seen1)
                                {
                                    if (elimCell.EliminateCandidate(intersectionCandidate, Name))
                                    {
                                        latestProgress = true;
                                    }
                                }

                                if (latestProgress)
                                {
                                    progress = true;
                                    _logger.Log(Name, $"({hingeCell.Col},{hingeCell.Row}), ({potential1.Col},{potential1.Row}), ({potential2.Col},{potential2.Row}) eliminated {intersectionCandidate}s successfully");
                                }
                            }
                        }
                    }
                }
            }
        }
        return progress;
    }
}