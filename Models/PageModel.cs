using System.Collections.Generic;
using SudokuBrain.Services;

namespace SudokuBrain.Models
{
    public class PageModel
    {
        public PageModel(MessageLog log = null)
        {
            Log = log ?? new MessageLog();
        }
        public string PageTitle { get; set; }
        public MessageLog Log { get; set; }

        public Puzzle InitialState { get; set; }
        public Puzzle Puzzle { get; set; }

        public List<PuzzleMetadata> Menu { get; set; }
    }
}