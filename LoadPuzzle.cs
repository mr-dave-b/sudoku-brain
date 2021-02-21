using System;
using System.IO;

public class LoadPuzzle
{
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
                        rowCells[col] = new Cell(line[col], row+1, col+1);
                    }
                    rows[row] = new Group(rowCells);
                }
                
                // Read the stream as a string, and write the string to the console.
                Console.WriteLine(sr.ReadToEnd());
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
            throw e;
        }
        return new Puzzle(rows);
    }
}