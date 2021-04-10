public class PuzzleMetadata
{
    private string _title;
    private string _description;

    private string _filename;

    public PuzzleMetadata(string filename = "", string title = "", string description = "")
    {
        _filename = filename;
        _title = title;
        _description = description;
    }

    public string FileName => _filename;
    public string Title => string.IsNullOrWhiteSpace(_title) ? _filename : _title;
    public string Description => _description;
}