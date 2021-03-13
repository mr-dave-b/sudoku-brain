using System;
using System.IO;
using SudokuBrain.Models;
using SudokuBrain.Services;

public class LoadPuzzle
{
    private readonly IMessageLogger _logger;

    public LoadPuzzle(IMessageLogger logger)
    {
        _logger = logger;
    }

    public Puzzle LoadFromInputTxt(string filename)
    {
        Group[] rows = new Group[9];
        try
        {
            // Open the text file using a stream reader.
            using (var sr = new StreamReader(filename))
            {
                for (int row = 0; row < 9; row++)
                {
                    string line = sr.ReadLine();
                    var rowCells = new Cell[9];
                    
                    for (int col = 0; col < 9; col++)
                    {
                        rowCells[col] = new Cell(line[col], row+1, col+1, _logger);
                    }
                    rows[row] = new Group(rowCells, $"row {row+1}", _logger);
                }
            }
        }
        catch (IOException e)
        {
            _logger.Log("LoadPuzzle", $"The file could not be read: {e.Message}");
            throw;
        }
        return new Puzzle(rows, _logger);
    }
}