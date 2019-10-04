namespace NhanDien.IOTLink.Helper
{
    /// <summary>
    /// Direction matrix
    /// _____________
    /// | 1 | 2 | 4 |
    /// | 8 | 16| 32|
    /// | 64|128|256|
    /// </summary>
    public enum Direction
    {
        Center = 16,
        Top = 2,
        Bottom = 128,
        Left = 8,
        Right = 32,
        TopLeft = 1,
        TopRight = 4,
        BottomLeft = 64,
        BottomRight = 256
    }
}
