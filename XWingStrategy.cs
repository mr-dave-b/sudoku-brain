using System;
using System.Collections.Generic;
using System.Linq;

public class XWingStrategy : IStrategy
{
    public string Name => "X-Wing Strategy";

    public int SkillLevel => 8;

    public bool Apply(Puzzle puzzle)
    {
        bool progress;
        // For each column, find the candidates which only appear in the same 2 rows
        progress = FindXWings(puzzle, true);
        // For each row, find the candidates which only appear in the same 2 columns
        progress = FindXWings(puzzle, false) || progress;
        return progress;
    }

    public bool FindXWings(Puzzle puzzle, bool columnsSearch)
    {
        bool progress = false;
        var lineCandidatePositions = new Dictionary<char, (int cell1, int cell2)>[10];
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
            lineCandidatePositions[lineNum] = new Dictionary<char, (int,int)>();
            foreach (var candidate in candidatePositions.Keys)
            {
                if (candidatePositions[candidate].Count == 2)
                {
                    lineCandidatePositions[lineNum].Add(candidate, (candidatePositions[candidate][0], candidatePositions[candidate][1]));
                }
            }
        }

        // Phew OK we've now got for each line a list of candidates and which 2 cell numbers they appear in
        for (int firstLineNum=1; firstLineNum<9; firstLineNum++)
        {
            for (int secondLineNum=firstLineNum+1; secondLineNum<=9; secondLineNum++)
            {
                var firstLineCandidates = lineCandidatePositions[firstLineNum];
                foreach(var candidate in firstLineCandidates.Keys)
                {
                    var secondLineCandidates = lineCandidatePositions[secondLineNum];
                    if (secondLineCandidates.ContainsKey(candidate))
                    {
                        if (secondLineCandidates[candidate] == firstLineCandidates[candidate])
                        {
                            // We've found an X Wing
                            int cellPos1 = firstLineCandidates[candidate].cell1;
                            int cellPos2 = firstLineCandidates[candidate].cell2;
                            if (columnsSearch)
                            {
                                Console.WriteLine($"XWing? {candidate} in cols {firstLineNum}, {secondLineNum} rows {cellPos1}, {cellPos2}");
                            }
                            else
                            {
                                Console.WriteLine($"XWing? {candidate} in rows {firstLineNum}, {secondLineNum} cols {cellPos1}, {cellPos2}");
                            }                            

                            // Eliminate any other instances of candidate from the 2 perpendicular lines
                            Group line1;
                            Group line2;
                            if (columnsSearch)
                            {
                                line1 = puzzle.GetRow(cellPos1);
                                line2 = puzzle.GetRow(cellPos2);
                            }
                            else
                            {
                                line1 = puzzle.GetColumn(cellPos1);
                                line2 = puzzle.GetColumn(cellPos2);
                            }
                            bool line1Progress = false, line2Progress = false;
                            for(int cell=1; cell<=9; cell++)
                            {
                                if (cell != firstLineNum && cell != secondLineNum)
                                {
                                    if (line1.GetCell(cell).EliminateCandidate(candidate))
                                    {
                                        line1Progress = true;
                                    }
                                    if (line2.GetCell(cell).EliminateCandidate(candidate))
                                    {
                                        line2Progress = true;
                                    }
                                }
                            }
                            if (columnsSearch)
                            {
                                if (line1Progress)
                                {
                                    Console.WriteLine($"XWing removed {candidate}s in row {cellPos1}");
                                    progress = true;
                                }
                                if (line2Progress)
                                {
                                    Console.WriteLine($"XWing removed {candidate}s in row {cellPos2}");
                                    progress = true;
                                }  
                            }
                            else
                            {
                                if (line1Progress)
                                {
                                    Console.WriteLine($"XWing removed {candidate}s in col {cellPos1}");
                                    progress = true;
                                }
                                if (line2Progress)
                                {
                                    Console.WriteLine($"XWing removed {candidate}s in col {cellPos2}");
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