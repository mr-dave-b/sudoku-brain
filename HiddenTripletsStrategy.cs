using System;
using System.Collections.Generic;
using System.Linq;

public class HiddenTripletsStrategy : IStrategy
{
    public string Name => "Hidden Triplets Strategy";

    public int SkillLevel => 7;

    private int _searchCombos = 0;
    
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
        _searchCombos = 0;
        // Iterate each row, column and box, looking for 3 candidates that are only in 3 cells
        for (int num = 1; num < 10; num++)
        {
            var group = puzzle.GetRow(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetColumn(num);
            progress = CheckGroup(group) || progress;
            group = puzzle.GetBox(num);
            progress = CheckGroup(group) || progress;
        }
        Console.WriteLine($"Hidden triplets: Searched {_searchCombos} group/triplet combos");
        return progress;
    }

    private bool CheckGroup(Group group)
    {
        bool progress = false;

        var candidatesList = new List<char>(group.Candidates);
        if (candidatesList.Count >= 3)
        {
            // Iterate all triples of possible candidates
            int combos = Helpers.Factorial(candidatesList.Count) / (Helpers.Factorial(3) * Helpers.Factorial(candidatesList.Count-3));
            _searchCombos += combos;
            //Console.WriteLine($"Hidden triplets: searching in {group.Description} with {combos} triplet combos");
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
                            //Console.WriteLine($"Triplet found: {candidate1}{candidate2}{candidate3} in {group.Description}");
                            // We found a triplet! If it is hidden, expose it
                            bool exposeProgress = false;
                            var tripleValues = new char[] {candidate1, candidate2, candidate3};
                            foreach (var cellNum in inCells)
                            {
                                var cellData = group.GetCell(cellNum);
                                if (cellData.SetOnlyCandidates(tripleValues))
                                {
                                    exposeProgress = true;
                                }
                            }
                            if (exposeProgress)
                            {
                                Console.WriteLine($"Hidden triplet: {candidate1}{candidate2}{candidate3} in {group.Description}");
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