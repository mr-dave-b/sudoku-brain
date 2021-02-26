using System;
using System.Collections.Generic;

public class Constants
{
    public static readonly IList<char> AllValues = Array.AsReadOnly(new [] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
    public static readonly IList<int> BoxTopRow = Array.AsReadOnly(new [] {1, 2, 3});
    public static readonly IList<int> BoxMiddleRow = Array.AsReadOnly(new [] {4, 5, 6});
    public static readonly IList<int> BoxBottomRow = Array.AsReadOnly(new [] {7, 8, 9});
    public static readonly IList<int> BoxLeftCol = Array.AsReadOnly(new [] {1, 4, 7});
    public static readonly IList<int> BoxMiddleCol = Array.AsReadOnly(new [] {2, 5, 8});
    public static readonly IList<int> BoxRightCol = Array.AsReadOnly(new [] {3, 6, 9});
}