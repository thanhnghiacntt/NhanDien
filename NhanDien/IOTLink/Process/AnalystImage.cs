using System.Collections.Generic;
using System.Drawing;

namespace NhanDien.IOTLink.Process
{
    /// <summary>
    /// Phân tích hình ảnh
    /// </summary>
    public class AnalystImage
    {
        /// <summary>
        /// Danh sách màu của hình
        /// </summary>
        public Color[,] Colors { get; set; }

        /// <summary>
        /// Image 2 dim
        /// </summary>
        public bool[,] Image { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public byte[,,] Data { get; set; }

        /// <summary>
        /// Phân tích
        /// </summary>
        public bool[] Analyst { get; set; }

        /// <summary>
        /// Bound của tile
        /// </summary>
        public BoundsFromTile BoundsFromTile { get; set; }

        /// <summary>
        /// Phân tích hình
        /// </summary>
        /// <param name="colors"></param>
        public AnalystImage(Color[,] colors, int z, int x, int y)
        {
            Colors = colors;
            BoundsFromTile = new BoundsFromTile(x, y, z);
            var w = colors.GetLength(0);
            var h = colors.GetLength(1);
            var tempData = new byte[w + 2, h + 2, 1];
            Data = new byte[w, h, 1];
            Image = new bool[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (colors[i,j].A == 255 && colors[i, j].B == 255 && colors[i, j].G == 255 && colors[i, j].R == 255)
                    {
                        tempData[i + 1, j + 1, 0] = 255;
                    }
                    else
                    {
                        tempData[i + 1, j + 1, 0] = 0;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                ZhangSuen.ZhangSuenThinning(tempData);
            }
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Data[i, j, 0] = tempData[i, j, 0];
                    Image[i, j] = Data[i, j, 0] == 255 ? true : false;
                    if (Image[i,j])
                    {
                        Colors[i, j] = Color.Black;
                    }
                    else
                    {
                        Colors[i, j] = Color.White;
                    }
                }
            }
        }
    }
}
