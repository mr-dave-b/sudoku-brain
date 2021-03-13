namespace SudokuBrain.Models
{
    public class LogItem
    {
        public LogItem(string owner, string message, LogItemLevel level)
        {
            Owner = owner;
            Message = message;
            Level = level;
        }

        public string Owner { get; set; }

        public string Message { get; set; }

        public LogItemLevel Level { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Owner))
            {
                return $"{Message}";
            }
            return $"{Owner}: {Message}";
        }
    }

    public enum LogItemLevel
    {
        Debug,
        Normal,
        Problem
    }
}
