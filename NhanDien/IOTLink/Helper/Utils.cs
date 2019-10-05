using Newtonsoft.Json;
using NhanDien.IOTLink.Process.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NhanDien.IOTLink.Helper
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
            for (int i = 0; i < h; i++)
            {
                var str = "";
                for (int j = 0; j < w; j++)
                {
                    var c = colors[j, i];
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
            for (int i = 0; i < h; i++)
            {
                var str = "";
                for (int j = 0; j < w; j++)
                {
                    if (images[j,i])
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
        /// Save bitmap
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void SaveColorText(string filePath, byte[,,] data)
        {
            var list = new List<string>();
            var w = data.GetLength(0);
            var h = data.GetLength(1);
            for (int j = 0; j < h; j++)
            {
                var str = "";
                for (int i = 0; i < w; i++)
                {
                    if (data[i, j, 0] == 255)
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
            int width = colors.GetLength(0);
            int height = colors.GetLength(1);
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
        /// <param name="matrix3x3"></param>
        /// <returns></returns>
        public static bool[,] Giao(bool[,] a, bool[,] matrix3x3)
        {
            var width = a.GetLength(0);
            var height = a.GetLength(1);
            var w = matrix3x3.GetLength(0);
            var h = matrix3x3.GetLength(1);
            var rs = new bool[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var hit = true;
                    for (int l = 0; l < w && hit; l++)
                    {
                        for (int k = 0; k < h && hit; k++)
                        {
                            var m = i - (w / 2) + l;
                            var n = j - (h / 2) + k;
                            if (matrix3x3[l, k] && m >= 0 && n >=0 && m <width && n < height)
                            {
                                hit = a[m, n];
                            }
                        }
                    }
                    rs[i, j] = hit;
                }
            }
            return rs;
        }

        /// <summary>
        /// Kiêm tra có phải anh tối hết không
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool IsEmpty(bool[,] a)
        {
            var width = a.GetLength(0);
            var height = a.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (a[i,j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Pixcel to location
        /// </summary>
        /// <param name="i">Pixcel at i</param>
        /// <param name="j">Pixcel at j</param>
        /// <param name="w">Width of image</param>
        /// <param name="h">Height of image</param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static Location PixcelToLocation(int i, int j, int w, int h, Bounds bounds)
        {
            double px = i / (w*1.0);
            double py = j / (h * 1.0);
            var lng = (px * (bounds.MaxLng - bounds.MinLng)) + bounds.MinLng;
            var lat = (py * (bounds.MinLat - bounds.MaxLat)) + bounds.MaxLat;
            int digits = 5;
            var location = new Location
            {
                Lng = Math.Round(lng, digits),
                Lat = Math.Round(lat, digits),
            };
            return location;
        }

        /// <summary>
        /// Source to string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToString(object source)
        {
            string json = null;
            if (source is string temp)
            {
                json = temp;
            }
            else
            {
                json = JsonConvert.SerializeObject(source);
            }
            return json;
        }

        /// <summary>
        /// Đếm tại vị trí này xung quanh có bao nhiêu phần tử
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static int Count(int i, int j, int w, int h, bool[,] image)
        {
            var count = 0;
            for (int l = i - 1; l <= i + 1; l++)
            {
                for (int m = j - 1; m <= j + 1; m++)
                {
                    if (IsTruePoint(i, j, w, h, image))
                    {
                        /// Tránh vị trí trung tâm
                        if (m != j || l != i)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }


        /// <summary>
        /// Tại vị trí này phải phần tử true hay không
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static bool IsTruePoint(int i, int j, int w, int h, bool[,] image)
        {
            if (i >= 0 && j >= 0 && i < w && j < h)
            {
                return image[i, j];
            }
            return false;
        }

        /// <summary>
        /// Find direction
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static HashSet<Direction> FindDirection(int i, int j, int w, int h, bool[,] image)
        {
            HashSet<Direction> hash = new HashSet<Direction>();
            if (IsTruePoint(i, j - 1, w, h, image))
            {
                hash.Add(Direction.Top);
            }
            if (IsTruePoint(i - 1, j, w, h, image))
            {
                hash.Add(Direction.Left);
            }
            if (IsTruePoint(i, j + 1, w, h, image))
            {
                hash.Add(Direction.Bottom);
            }
            if (IsTruePoint(i + 1, j, w, h, image))
            {
                hash.Add(Direction.Right);
            }
            if (IsTruePoint(i - 1, j - 1, w, h, image))
            {
                hash.Add(Direction.TopLeft);
            }
            if (IsTruePoint(i + 1, j - 1, w, h, image))
            {
                hash.Add(Direction.TopRight);
            }
            if (IsTruePoint(i - 1, j + 1, w, h, image))
            {
                hash.Add(Direction.BottomLeft);
            }
            if (IsTruePoint(i + 1, j + 1, w, h, image))
            {
                hash.Add(Direction.BottomRight);
            }
            return hash;
        }

        /// <summary>
        /// Compare 2 double
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool Compare(double a, double b, double e = 0.000001)
        {
            if (Math.Abs(a-b) < e)
            {
                return true;
            }
            return false;
        }
    }
}
