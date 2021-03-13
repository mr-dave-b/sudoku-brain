using SudokuBrain.Models;

namespace SudokuBrain.Services
{
    public interface IMessageLogger
    {
        void Log(string owner, string message, LogItemLevel level = LogItemLevel.Normal);
    }
}
