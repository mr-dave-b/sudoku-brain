using System;
using System.Collections.Generic;
using System.Linq;

public class HiddenTripletsStrategy
{
    // TODO: NOT WRITTEN YET
    public bool Apply(Puzzle puzzle)
    {
        bool progress = false;
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
        return progress;
    }

    private bool CheckGroup(Group group)
    {
        bool progress = false;
        // Get all the candidates in the group
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

        // Iterate all triples of possible candidates
        Console.WriteLine($"Hidden triplets: searching in {group.Description} with {candidates.Count*(candidates.Count-1)*(candidates.Count-2)} pair combos");
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
                        Console.WriteLine($"Triplet found: {candidate1}{candidate2}{candidate3} in {group.Description}");
                        // We found a hidden triplet! Expose it
                        foreach (var cellNum in inCells)
                        {
                            // TODO: Expose the triplet and then remove candidates from other cells in the group
                            //
                            //
                            
                                
                            //var cellData = group.GetCell(cellNum);
                            //if (cellData.SetOnlyCandidates(new char[] {candidate1, candidate2}))
                            //{
                            //    Console.WriteLine($"Hidden pairs: hidden pair {candidate1}{candidate2} exposed in {group.Description}");
                            //    // Re-check all candidates before moving on to the next group
                            //    //puzzle.CheckAllGroups();
                            //    progress = true;
                            //}
                        }
                    }
                }
            }
        }
        return progress;
    }
}