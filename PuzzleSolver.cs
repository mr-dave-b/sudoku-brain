using System;
using System.Collections.Generic;
using SudokuBrain.Services;

public class PuzzleSolver
{
    private readonly List<IStrategy> _strategies;
    private readonly IMessageLogger _logger;

    public PuzzleSolver(IMessageLogger logger)
    {
        _logger = logger;
        
        _strategies = new List<IStrategy>
        {
            new SimpleElimination(),
            new HiddenSinglesStrategy(),
            new PointingCandidatesStrategy(_logger),
            new BoxLineReductionStrategy(_logger),
            new NakedPairsStrategy(_logger),
            new HiddenPairsStrategy(_logger),
            new NakedTripletsStrategy(_logger),
            new HiddenTripletsStrategy(_logger),
            new XWingStrategy(_logger),
            new SwordfishStrategy(_logger),
            new YWingStrategy(_logger)
        };
    }

    // Applying strategies until some progress is made, then we should go round again
    public bool ApplyAllStrats(Puzzle puzzle)
    {
        foreach (var strat in _strategies)
        {
            if (strat.Apply(puzzle))
            {
                if (!puzzle.UsedStrats.ContainsKey(strat.Name))
                {
                    puzzle.UsedStrats.Add(strat.Name, strat.SkillLevel);
                }
                if (!puzzle.Validate())
                {
                    throw new Exception("Quitting because validation failed");
                }
                return true;
            }
        }
        return false;
    }

    public void EndSolutionStats(Puzzle puzzle)
    {
        _logger.Log(null, "Strategies applied successfully:");
        int difficulty = 0;
        foreach (var strat in puzzle.UsedStrats)
        {
            difficulty += strat.Value;
            _logger.Log(null, $"{strat.Key}");

        }
        if (puzzle.NumbersFilledIn == 81)
        {
            _logger.Log(null, $"Difficulty: {difficulty}");
            _logger.Log(null, "All Done :)");
        }
        else
        {
            _logger.Log(null, $"Difficulty: {difficulty}+");
            _logger.Log(null, "Ran out of ideas :/", SudokuBrain.Models.LogItemLevel.Problem);
        }
    }
}