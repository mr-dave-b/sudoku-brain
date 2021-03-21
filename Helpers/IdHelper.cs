using System.Text.RegularExpressions;

namespace SudokuBrain.Helpers
{
    public static class IdHelper
    {
        public static string FormatId(this string id)
        {
            Regex invalidCharRemover = new Regex("[^a-z0-9]");
            id = invalidCharRemover.Replace(id.ToLowerInvariant(), "-");

            Regex duplicateDashRemover = new Regex("[-]{2,}");
            id = duplicateDashRemover.Replace(id, "-").Trim('-');

            if (id.Length < 1)
            {
                id = "-";
            }
            return id;
        }
    }
}
