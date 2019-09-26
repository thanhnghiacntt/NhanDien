using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NhanDien.IOTLink
{
    /// <summary>
    /// Utils
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// File image to color
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Color[,] GetColorsImage(string filePath)
        {
            Bitmap bmp = new Bitmap(filePath);
            return BitmapToColors(bmp);
        }

        /// <summary>
        /// Save bitmap
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="colors"></param>
        public static void SaveColorImage(string filePath, Color[,] colors)
        {
            var bit = ColorToBitmap(colors);
            bit.Save(filePath);
        }

        /// <summary>
        /// Save bitmap
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="colors"></param>
        public static void SaveColorText(string filePath, Color[,] colors)
        {
            var list = new List<string>();
            var w = colors.GetLength(0);
            var h = colors.GetLength(1);
            for (int i = 0; i < w; i++)
            {
                var str = "";
                for (int j = 0; j < h; j++)
                {
                    var c = colors[i, j];
                    if (c.A == 255 && c.B == 0 && c.G == 0 && c.R == 0)
                    {
                        str += "-";
                    }
                    else
                    {
                        str += "+";
                    }
                }
                list.Add(str);
            }
            File.WriteAllLines(filePath, list);
        }

        /// <summary>
        /// Save bitmap
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="images"></param>
        public static void SaveColorText(string filePath, bool[,] images)
        {
            var list = new List<string>();
            var w = images.GetLength(0);
            var h = images.GetLength(1);
            for (int i = 0; i < w; i++)
            {
                var str = "";
                for (int j = 0; j < h; j++)
                {
                    if (images[i, j])
                    {
                        str += "+";
                    }
                    else
                    {
                        str += "-";
                    }
                }
                list.Add(str);
            }
            File.WriteAllLines(filePath, list);
        }

        /// <summary>
        /// Bitmap to color
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Color[,] BitmapToColors(Bitmap bmp)
        {
            int height = bmp.Height;
            int width = bmp.Width;
            Color[,] matrix = new Color[width, height];
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    matrix[i,j] = bmp.GetPixel(i, j);
                }
            }
            return matrix;
        }

        /// <summary>
        /// Color to bitmap
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Bitmap ColorToBitmap(Color[,] colors)
        {
            int height = colors.GetLength(0);
            int width = colors.GetLength(1);
            Bitmap bit = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bit.SetPixel(i, j, colors[i, j]);
                }
            }
            return bit;
        }

        /// <summary>
        /// Create array two dim
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static T[][] CreateArrayTwoDim<T>(int width, int height)
        {
            var rs = new T[width][];
            for (int i = 0; i < width; i++)
            {
                rs[i] = new T[height];
            }
            return rs;
        }

        /// <summary>
        /// Làm mờ bằng matrix 3x3
        /// </summary>
        /// <param name="a"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[,] LamMo(double[,] a, double[,] matrix)
        {
            var width = a.GetLength(0);
            var height = a.GetLength(1);
            var rs = new double[width, height];
            var w = matrix.GetLength(0) / 2;
            var h = height / 2;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i < w || i >= width - w || j < h || j >= height - h)
                    {
                        rs[i, j] = a[i, j];
                    }
                    else
                    {
                        var temp = 0.0;
                        for (int l = 0; l < width; l++)
                        {
                            for (int k = 0; k < height; k++)
                            {
                                temp += a[i + l, j + k];
                            }
                        }
                        rs[i, j] = temp;
                    }
                }
            }
            return rs;
        }

        /// <summary>
        /// Làm mờ bằng matrix 3x3
        /// </summary>
        /// <param name="a"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Color[,] LamMo(Color[,] a, int[,] matrix)
        {
            var width = a.GetLength(0);
            var height = a.GetLength(1);
            var rs = new Color[width, height];
            var w = matrix.GetLength(0) / 2;
            var h = matrix.GetLength(1) / 2;
            var sum = 0.0;
            var wMatrix = matrix.GetLength(0);
            var hMatrix = matrix.GetLength(1);
            for (int l = 0; l < wMatrix; l++)
            {
                for (int k = 0; k < hMatrix; k++)
                {
                    sum += matrix[l,k];
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i < w || i >= width - w - 1 || j < h || j >= height - h - 1)
                    {
                        rs[i, j] = a[i, j];
                    }
                    else
                    {
                        var temp = new double[] { 0.0, 0.0, 0.0, 0.0 };
                        var avg = new int[] { 0, 0, 0, 0 };
                        for (int l = 0; l < wMatrix; l++)
                        {
                            for (int k = 0; k < hMatrix; k++)
                            {
                                temp[0] += a[i + l, j + k].A * matrix[l, k];
                                temp[1] += a[i + l, j + k].R * matrix[l, k];
                                temp[2] += a[i + l, j + k].G * matrix[l, k];
                                temp[3] += a[i + l, j + k].B * matrix[l, k];
                            }
                        }
                        if (sum != 0)
                        {
                            for (int l = 0; l < temp.Length; l++)
                            {
                                avg[l] = (int)(temp[l] / sum);
                            }
                        }
                        rs[i, j] = Color.FromArgb(avg[0], avg[1], avg[2], avg[3]);
                    }
                }
            }
            return rs;
        }

        /// <summary>
        /// Giao hai ma trận
        /// </summary>
        /// <param name="a"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool[,] Giao(bool[,] a, bool[,] matrix)
        {
            var width = a.GetLength(0);
            var height = a.GetLength(1);
            var rs = new bool[width, height];
            var w = matrix.GetLength(0) / 2;
            var h = matrix.GetLength(1) / 2;
            var wMatrix = matrix.GetLength(0);
            var hMatrix = matrix.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i < w || i >= width - w - 1 || j < h || j >= height - h - 1)
                    {
                        rs[i, j] = a[i, j];
                    }
                    else
                    {
                        for (int l = 0; l < wMatrix; l++)
                        {
                            for (int k = 0; k < hMatrix; k++)
                            {
                                rs[i, j] = a[i, j] & matrix[l,k];
                            }
                        }
                    }
                }
            }
            return rs;
        }
    }
}
