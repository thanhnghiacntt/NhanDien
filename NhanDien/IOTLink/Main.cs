using Accord.MachineLearning.VectorMachines;
using System;
using System.Drawing;

namespace NhanDien.IOTLink
{
    public static class Main
    {
        public static void Process()
        {
            var myBitmap = GetBitmapColorMatrix(@"D:\MyProject\C#\NhanDien\NhanDien\NhanDien\test\839513,476850,20.png");
            int count = 0;
            for (int i = 0; i < myBitmap.Length; i++)
            {
                for (int j = 0; j < myBitmap[i].Length; j++)
                {
                    var color = myBitmap[i][j];
                    if (color.A == 255 && color.B == 255 && color.G == 255)
                    {
                        count++;
                    }
                }
            }
            Console.WriteLine(count);
            var temp = new SupportVectorMachine(2);
        }

        public static Color[][] GetBitmapColorMatrix(string filePath)
        {
            Bitmap bmp = new Bitmap(filePath);
            Color[][] matrix;
            int height = bmp.Height;
            int width = bmp.Width;
            matrix = new Color[bmp.Width][];
            for (int i = 0; i <= bmp.Width - 1; i++)
            {
                matrix[i] = new Color[bmp.Height];
                for (int j = 0; j < bmp.Height - 1; j++)
                {
                    matrix[i][j] = bmp.GetPixel(i, j);
                }
            }
            return matrix;
        }
    }
}
