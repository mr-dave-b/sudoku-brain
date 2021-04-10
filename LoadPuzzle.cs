using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SudokuBrain.Models;
using SudokuBrain.Services;

public class LoadPuzzle
{
    private readonly IMessageLogger _logger;

    public LoadPuzzle(IMessageLogger logger)
    {
        _logger = logger;
    }
    
    public Puzzle LoadFromString(string input)
    {
        Group[] rows = new Group[9];
        try
        {
            var enumerator = input.GetEnumerator();
            for (int row = 0; row < 9; row++)
            {
                var rowCells = new Cell[9];
                
                for (int col = 0; col < 9; col++)
                {
                    enumerator.MoveNext();
                    var current = enumerator.Current;
                    rowCells[col] = new Cell(current, row+1, col+1, _logger);
                }
                rows[row] = new Group(rowCells, $"row {row+1}", _logger);
            }
        }
        catch (IOException e)
        {
            _logger.Log("LoadPuzzle", $"Problem reading puzzle data: {e.Message}");
            throw;
        }
        return new Puzzle(rows, _logger);
    }

    public Puzzle LoadFromFile(string filename)
    {
        string title = "";
        string description = "";
        Group[] rows = new Group[9];
        try
        {
            // Open the text file using a stream reader.
            using (var sr = new StreamReader(filename))
            {
                string line = sr.ReadLine();
                if (line.StartsWith("title:", StringComparison.InvariantCultureIgnoreCase))
                {
                    title = line.Substring(6).Trim();
                    line = sr.ReadLine();
                }
                if (line.StartsWith("description:", StringComparison.InvariantCultureIgnoreCase))
                {
                    description = line.Substring(12).Trim();
                    line = sr.ReadLine();
                }
                for (int row = 0; row < 9; row++)
                {
                    if (row > 0)
                    {
                        line = sr.ReadLine();
                    }
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
        return new Puzzle(rows, _logger, Path.GetFileNameWithoutExtension(filename), title, description);
    }

    public List<PuzzleMetadata> ListPuzzleFiles(string folder)
    {
        // Process the list of files found in the directory.
        var results = new List<PuzzleMetadata>();
        string [] fileEntries = Directory.GetFiles(folder, "*.txt");
        foreach(string fileName in fileEntries)
        {
            string title = "";
            string description = "";
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(fileName))
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("title:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        title = line.Substring(6).Trim();
                        line = sr.ReadLine();
                    }
                    if (line.StartsWith("description:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        description = line.Substring(12).ToString();
                        line = sr.ReadLine();
                    }
                }
            }
            catch (IOException e)
            {
                _logger.Log("LoadPuzzle", $"The file could not be read: {e.Message}");
                throw;
            }
            results.Add(new PuzzleMetadata(Path.GetFileNameWithoutExtension(fileName), title, description));
        }
        return results;
    }
}