using SudokuBrain.Services;

namespace SudokuBrain.Models
{
    public class PageModel
    {
        public PageModel()
        {
            Log = new MessageLog();
        }
        public string PageTitle { get; set; }
        public MessageLog Log { get; set; }
    }
}