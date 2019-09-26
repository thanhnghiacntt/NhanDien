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
        /// Hình trắng và đen
        /// </summary>
        public bool[,] Images { get; set; }

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
            BoundsFromTile = new BoundsFromTile(x, y, z);
            var w = colors.GetLength(0);
            var h = colors.GetLength(1);
            Images = new bool[w, h];
            var width = new bool[w, h];
            var height = new bool[w, h];
            Analyst = new bool[h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (colors[i,j].A == 255 && colors[i, j].B == 255 && colors[i, j].G == 255 && colors[i, j].R == 255)
                    {
                        width[i, j] = true;
                        height[i, j] = true;
                    }
                    else
                    {
                        width[i, j] = false;
                        height[i, j] = false;
                    }
                    Images[i, j] = false;
                }
            }
            for (int i = 0; i < w; i++)
            {
                IList<int> counts = new List<int>();
                int count = 0;
                for (int j = 0; j < h; j++)
                {
                    if (height[i, j])
                    {
                        count++;
                        if (j == h-1)
                        {
                            counts.Add(count);
                        }
                    }
                    else
                    {
                        if (count > 0)
                        {
                            counts.Add(count);
                        }
                        count = 0;
                    }
                }
                var index = 0;
                for (int j = 0; j < h; j++)
                {
                    if (height[i, j])
                    {
                        j = j + counts[index];
                        if (counts[index] > 20)
                        {
                            Images[i, j - (counts[index] / 2)] = true;
                        }
                        index++;
                    }
                }
            }

            for (int j = 0; j < h; j++)
            {
                IList<int> counts = new List<int>();
                int count = 0;
                for (int i = 0; i < w; i++)
                {
                    if (width[i, j])
                    {
                        count++;
                        if (i == w - 1)
                        {
                            counts.Add(count);
                        }
                    }
                    else
                    {
                        if (count > 0)
                        {
                            counts.Add(count);
                        }
                        count = 0;
                    }
                }
                var index = 0;
                for (int i = 0; i < w; i++)
                {
                    if (width[i, j])
                    {
                        i = i + counts[index];
                        if (counts[index] > 20)
                        {
                            Images[i - (counts[index] / 2), j] = true;
                        }
                        index++;
                    }
                }
            }
        }
    }
}
