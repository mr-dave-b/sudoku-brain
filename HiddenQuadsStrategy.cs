using System;
using System.Collections.Generic;
using System.Linq;
using SudokuBrain.Services;

public class HiddenQuadsStrategy : IStrategy
{
    // TODO: This one isn't finished yet!

    private readonly IMessageLogger _logger;

    public HiddenQuadsStrategy(IMessageLogger logger)
    {
        _logger = logger;
    }
    public string Name => "Hidden Quads Strategy";

    public int SkillLevel => 6;
    
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        // Iterate each row, column and box, looking for 4 candidates that are only in 4 cells
        for (int num = 1; num < 10; num++)
        {
            var group = puzzle.GetRow(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetColumn(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetBox(num);
            progress = CheckGroup(group) || progress;
        }
        return progress;
    }

    private bool CheckGroup(Group group)
    {
        bool progress = false;
        // Get all remaining candidates in the group
        var candidates = new HashSet<char>(Constants.AllValues);
        for (int cell = 1; cell <= 9; cell++)
        {
            var cellData = group.GetCell(cell);
            if (cellData.Given || cellData.Filled)
            {
                candidates.Remove(cellData.Value);
            }
        }

        var candidatesList = new List<char>(candidates);

        if (candidatesList.Count >= 4)
        {
            // Iterate all sets of 4 possible candidates
            int combos = Helpers.Factorial(candidates.Count) / (Helpers.Factorial(4) * Helpers.Factorial(candidates.Count-4));
            _logger.Log(Name, $"Hidden quads: searching in {group.Description} with {combos} quad combos");
            for (var i=0; i<candidatesList.Count-2; i++)
            {
                var candidate1 = candidatesList[i];
                for (var j=i+1; j<candidatesList.Count-1; j++)
                {
                    var candidate2 = candidatesList[j];
                    for (var k=j+1; k<candidatesList.Count; k++)
                    {
                        var candidate3 = candidatesList[k];
                        // Find all cells containing 2 or 3 of the candidates
                        var permutation = new char[] { candidate1, candidate2, candidate3 };
                        var rejected = false;
                        var inCells = new List<int>();
                        for (int cell = 1; cell <= 9; cell++)
                        {
                            var cellData = group.GetCell(cell);
                            if (cellData.Given || cellData.Filled)
                            {
                                continue;
                            }
                            var foundCandidates = new HashSet<char>(cellData.Candidates);
                            foundCandidates.IntersectWith(permutation);
                            if (foundCandidates.Count == 1)
                            {
                                rejected = true;
                                break;
                            }
                            if (foundCandidates.Count > 3)
                            {
                                throw new Exception("makes no sense");
                            }
                            if (foundCandidates.Count > 1)
                            {
                                if (inCells.Count >= 3)
                                {
                                    // This triplet is in too many cells
                                    rejected = true;
                                    break;
                                }
                                inCells.Add(cell);
                            }
                        }
                        if (rejected)
                        {
                            continue;
                        }
                        if (inCells.Count == 3)
                        {
                            _logger.Log(Name, $"{candidate1}{candidate2}{candidate3} in {group.Description}");
                            // We found a hidden triplet! Expose it
                            bool exposeProgress = false;
                            var tripleValues = new char[] {candidate1, candidate2, candidate3};
                            foreach (var cellNum in inCells)
                            {
                                var cellData = group.GetCell(cellNum);
                                var currentCandidateCount = cellData.Candidates.Count;
                                cellData.Candidates.IntersectWith(tripleValues);
                                if (cellData.Candidates.Count < currentCandidateCount)
                                {
                                    exposeProgress = true;
                                }
                            }
                            if (exposeProgress)
                            {
                                _logger.Log(Name, $"Exposed {candidate1}{candidate2}{candidate3} in {group.Description}", SudokuBrain.Models.LogItemLevel.Debug);
                                progress = true;
                            }

                            bool tripletProgress = false;
                            // This is a naked triplet - remove candidates from all other cells!
                            for (int cellNum = 1; cellNum <= 9; cellNum++)
                            {
                                if (inCells.Contains(cellNum))
                                {
                                    continue;
                                }

                                var cell = group.GetCell(cellNum);
                                if (!cell.Given && !cell.Filled)
                                {
                                    if (cell.EliminateCandidates(tripleValues))
                                    {
                                        tripletProgress = true;
                                    }
                                }
                            }
                            if (tripletProgress)
                            {
                                _logger.Log(Name, $"Progress {candidate1}{candidate2}{candidate3} in {group.Description}");
                                progress = true;
                            }
                        }
                    }
                }
            }
        }
        return progress;
    }
}