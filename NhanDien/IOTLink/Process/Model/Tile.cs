using System;

namespace NhanDien.IOTLink.Process.Model
{
    /// <summary>
    /// Tile
    /// </summary>
    public class Tile
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
        /// Zoom
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// Tile
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="zoom"></param>
        public Tile(double lat, double lng, int zoom)
        {
            X = (int)((lng + 180) / 360 * (1 << zoom));
            Y = (int)((1 - Math.Log(Math.Tan(lat * Math.PI / 180) + (1 / Math.Cos(lat * Math.PI / 180))) / Math.PI) / 2 * (1 << zoom));
            Z = zoom;
        }

        /// <summary>
        /// Tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Tile(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
