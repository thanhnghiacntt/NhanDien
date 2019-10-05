namespace NhanDien.IOTLink.Process.Model
{
    /// <summary>
    /// Point pixcel on image
    /// </summary>
    public class PointPixcel
    {
        /// <summary>
        /// X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Count Đếm xung quanh bao nhiêu cái
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public Location Location { get; set; }
    }
}
