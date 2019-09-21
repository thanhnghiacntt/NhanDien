using Accord.MachineLearning.VectorMachines;
using System;
using System.Drawing;

namespace NhanDien.IOTLink
{
    public static class Main
    {
        public static void Process()
        {
            var myBitmap = Utils.GetColorsImage(@"D:\MyProject\C#\NhanDien\NhanDien\NhanDien\test\839513,476850,20.png");
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

    }
}
