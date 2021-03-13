using System;
using System.Collections.Generic;
using System.Linq;
using SudokuBrain.Services;

public class NakedTripletsStrategy : IStrategy
{
    private readonly IMessageLogger _logger;

    public NakedTripletsStrategy(IMessageLogger logger)
    {
        _logger = logger;
    }

    public string Name => "Naked Triplets Strategy";

    public int SkillLevel => 6;

    private int _searchCombos = 0;
    
    public bool Apply(Puzzle puzzle)
    {
        _searchCombos = 0;
        bool progress = false;
        // Iterate each row, column and box, looking for 3 cells that contain only 3 candidates
        for (int num = 1; num < 10; num++)
        {
            var group = puzzle.GetRow(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetColumn(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetBox(num);
            progress = CheckGroup(group) || progress;
        }
        _logger.Log(Name, $"Searched {_searchCombos} group/triplet combos");
        return progress;
    }

    private bool CheckGroup(Group group)
    {
        bool progress = false;   

        var candidatesList = new List<char>(group.Candidates);
        if (candidatesList.Count >= 3)
        {
            // Iterate all possible triplets
            int combos = Helpers.Factorial(candidatesList.Count) / (Helpers.Factorial(3) * Helpers.Factorial(candidatesList.Count-3));
            // _logger.Log(Name, $"Searching in {group.Description} with {combos} triplet combos");
            _searchCombos += combos;
            for (var i=0; i<candidatesList.Count-2; i++)
            {
                var candidate1 = candidatesList[i];
                for (var j=i+1; j<candidatesList.Count-1; j++)
                {
                    var candidate2 = candidatesList[j];
                    for (var k=j+1; k<candidatesList.Count; k++)
                    {
                        var candidate3 = candidatesList[k];
                        // Find all cells containing only 2 or 3 of the candidates
                        var permutation = new char[] { candidate1, candidate2, candidate3 };
                        var rejected = false;
                        var inCells = new List<int>();
                        for (int cell = 1; cell <= 9; cell++)
                        {
                            var cellData = group.GetCell(cell);
                            if (cellData.Filled)
                            {
                                continue;
                            }
                            var foundCandidates = new HashSet<char>(cellData.Candidates);
                            foundCandidates.IntersectWith(permutation);
                            if (foundCandidates.Count < cellData.Candidates.Count)
                            {
                                // Cell contains other candidates
                                continue;
                            }
                            if (foundCandidates.Count < 2)
                            {
                                continue;
                            }
                            if (inCells.Count >= 3)
                            {
                                // This triplet spans too many cells
                                rejected = true;
                                break;
                            }
                            inCells.Add(cell);
                        }
                        if (rejected)
                        {
                            continue;
                        }
                        if (inCells.Count == 3)
                        {
                            _logger.Log(Name, $"Found {candidate1}{candidate2}{candidate3} in {group.Description}", SudokuBrain.Models.LogItemLevel.Debug);
                            var tripleValues = new char[] {candidate1, candidate2, candidate3};

                            bool tripletProgress = false;
                            // This is a naked triplet - remove candidates from all other cells!
                            for (int cellNum = 1; cellNum <= 9; cellNum++)
                            {
                                if (inCells.Contains(cellNum))
                                {
                                    continue;
                                }
                                var cell = group.GetCell(cellNum);
                                if (!cell.Filled)
                                {
                                    if (cell.EliminateCandidates(tripleValues))
                                    {
                                        tripletProgress = true;
                                    }
                                }
                            }
                            if (tripletProgress)
                            {
                                _logger.Log(Name, $"{candidate1}{candidate2}{candidate3} in {group.Description}");
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