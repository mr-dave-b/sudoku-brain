using System;
using System.Collections.Generic;
using System.Linq;

public class SwordfishStrategy : IStrategy
{
    public string Name => "Swordfish Strategy";

    public int SkillLevel => 9;

    public bool Apply(Puzzle puzzle)
    {
        bool progress;
        // For each column, find the candidates which only appear in the same 3 rows
        progress = FindSwordfish(puzzle, true);
        // For each row, find the candidates which only appear in the same 3 columns
        progress = FindSwordfish(puzzle, false) || progress;
        return progress;
    }

    public bool FindSwordfish(Puzzle puzzle, bool columnsSearch)
    {
        bool progress = false;
        var lineCandidatePositions = new Dictionary<char,List<int>>[10];
        for (int lineNum = 1; lineNum <=9; lineNum++)
        {
            var candidatePositions = new Dictionary<char,List<int>>();
            Group line;
            if (columnsSearch)
            {
                line = puzzle.GetColumn(lineNum);
            }
            else
            {
                line = puzzle.GetRow(lineNum);
            }
            for (int cellNum=1; cellNum<=9; cellNum++)
            {
                var cell = line.GetCell(cellNum);
                if (!cell.Filled)
                {
                    foreach (var candidate in cell.Candidates)
                    {
                        if (candidatePositions.ContainsKey(candidate))
                        {
                            candidatePositions[candidate].Add(cellNum);
                        }
                        else
                        {
                            candidatePositions[candidate] = new List<int>{cellNum};
                        }
                    }
                }
            }
            lineCandidatePositions[lineNum] = new Dictionary<char,List<int>>();
            foreach (var candidate in candidatePositions.Keys)
            {
                if (candidatePositions[candidate].Count == 2 || candidatePositions[candidate].Count == 3)
                {
                    lineCandidatePositions[lineNum].Add(candidate, candidatePositions[candidate]);
                }
            }
        }

        // Phew OK we've now got for each line a list of candidates and which 2 or 3 cell numbers they appear in
        for (int firstLineNum=1; firstLineNum<=7; firstLineNum++)
        {
            for (int secondLineNum=firstLineNum+1; secondLineNum<=8; secondLineNum++)
            {
                for (int thirdLineNum=secondLineNum+1; thirdLineNum<=9; thirdLineNum++)
                {
                    var firstLineCandidates = lineCandidatePositions[firstLineNum];
                    foreach(var candidate in firstLineCandidates.Keys)
                    {
                        var secondLineCandidates = lineCandidatePositions[secondLineNum];
                        var thirdLineCandidates = lineCandidatePositions[thirdLineNum];
                        if (secondLineCandidates.ContainsKey(candidate) && thirdLineCandidates.ContainsKey(candidate))
                        {
                            // Check if the three lines have the candidate within the same 3 positions
                            var allCandidateCells = firstLineCandidates[candidate]
                            .Union(secondLineCandidates[candidate])
                            .Union(thirdLineCandidates[candidate])
                            .ToList();

                            if (allCandidateCells.Count == 3)
                            {
                                // We've found a Swordfish
                                if (columnsSearch)
                                {
                                    Console.WriteLine($"Swordfish? {candidate} in cols {firstLineNum}, {secondLineNum}, {thirdLineNum} rows {allCandidateCells[0]}, {allCandidateCells[1]}, {allCandidateCells[2]}");
                                }
                                else
                                {
                                    Console.WriteLine($"Swordfish? {candidate} in rows {firstLineNum}, {secondLineNum}, {thirdLineNum} cols {allCandidateCells[0]}, {allCandidateCells[1]}, {allCandidateCells[2]}");
                                }
                                
                                // Eliminate any other instances of candidate from the 3 perpendicular lines
                                IEnumerable<Group> lines;
                                if (columnsSearch)
                                {
                                    lines = allCandidateCells.Select(c => puzzle.GetRow(c));
                                }
                                else
                                {
                                    lines = allCandidateCells.Select(c => puzzle.GetColumn(c));
                                }
                                var lineProgress = new HashSet<Group>();
                                for (int cell=1; cell<=9; cell++)
                                {
                                    if (cell != firstLineNum && cell != secondLineNum && cell != thirdLineNum)
                                    {
                                        foreach (var line in lines)
                                        {
                                            if (line.GetCell(cell).EliminateCandidate(candidate))
                                            {
                                                lineProgress.Add(line);
                                                //Console.WriteLine($"Swordfish works!!!");
                                            }
                                        }
                                    }
                                }
                                foreach (var line in lineProgress)
                                {
                                    Console.WriteLine($"Swordfish removed {candidate}s in {line.Description}");
                                    progress = true;
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