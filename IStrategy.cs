using System;
using System.Linq;

public interface IStrategy
{
    // Iterate each row, column and box, eliminating candidates that are already filled in
    bool Apply(Puzzle puzzle);

    string Name { get; }

    int SkillLevel { get; }
}