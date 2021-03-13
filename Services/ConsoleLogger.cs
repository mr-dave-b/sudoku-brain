using System;
using SudokuBrain.Models;

namespace SudokuBrain.Services
{
    public class ConsoleLogger : IMessageLogger
    {
        public void Log(string owner, string message, LogItemLevel level = LogItemLevel.Normal)
        {
            if (string.IsNullOrWhiteSpace(owner))
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine($"{owner}: {message}");
            }
        }
    }
}