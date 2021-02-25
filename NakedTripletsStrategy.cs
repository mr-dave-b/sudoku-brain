using System;
using System.Collections.Generic;
using System.Linq;

public class NakedTripletsStrategy : IStrategy
{
    public string Name => "Naked Triplets Strategy";

    public int SkillLevel => 6;
    
    public bool Apply(Puzzle puzzle)
    {
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

        if (candidatesList.Count >= 3)
        {
            // Iterate all possible triplets
            int combos = Helpers.Factorial(candidates.Count) / (Helpers.Factorial(3) * Helpers.Factorial(candidates.Count-3));
            Console.WriteLine($"Naked triplets: searching in {group.Description} with {combos} triplet combos");
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
                            //Console.WriteLine($"Naked triplet found: {candidate1}{candidate2}{candidate3} in {group.Description}");
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
                                Console.WriteLine($"Naked triplet helps us: {candidate1}{candidate2}{candidate3} in {group.Description}");
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