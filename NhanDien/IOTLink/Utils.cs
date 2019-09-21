using System.Drawing;
using Accord.Math;

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
        /// <param name="matrix3x3"></param>
        /// <returns></returns>
        public static double[,] LamMo(double[,] a, double[,] matrix3x3)
        {
            var rs = new double[a.GetLength(0), a.GetLength(1)];
            var w = matrix3x3.GetLength(0) / 2;
            var h = matrix3x3.GetLength(1) / 2;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (i < w || i >= a.GetLength(0) - w || j < h || j >= a.GetLength(1) - h)
                    {
                        rs[i, j] = a[i, j];
                    }
                    else
                    {
                        var temp = 0.0;
                        for (int l = 0; l < matrix3x3.GetLength(0); l++)
                        {
                            for (int k = 0; k < matrix3x3.GetLength(1); k++)
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
        /// <param name="matrix4x4"></param>
        /// <returns></returns>
        public static double[,] LamMo4x4(double[,] a, double[,] matrix4x4)
        {
            var rs = new double[a.GetLength(0), a.GetLength(1)];
            var w = matrix4x4.GetLength(0) / 2;
            var h = matrix4x4.GetLength(1) / 2;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (i < w || i > a.GetLength(0) - w || j < h || j > a.GetLength(1) - h)
                    {
                        rs[i, j] = a[i, j];
                    }
                    else
                    {
                        var temp = 0.0;
                        for (int l = 0; l < matrix4x4.GetLength(0); l++)
                        {
                            for (int k = 0; k < matrix4x4.GetLength(1); k++)
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
    }
}
