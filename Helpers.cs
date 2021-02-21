public static class Helpers
{
    public static int GetRowOffset(int boxNumber)
    {
        if (boxNumber > 6)
        {
            return 6;
        }
        else if (boxNumber > 3)
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }

    public static int GetColOffset(int boxNumber)
    {
        return 3*((boxNumber-1) % 3);
    }
}