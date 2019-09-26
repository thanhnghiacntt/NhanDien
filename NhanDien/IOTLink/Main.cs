using Accord.MachineLearning.VectorMachines;
using NhanDien.IOTLink.Process;
using System.Drawing;


namespace NhanDien.IOTLink
{
    public static class Main
    {
        public static void Process()
        {
            string startupPath = @"D:\MyProject\C#\NhanDien\NhanDien\";
            //var path = startupPath + @"test\839572,476863,20.png";
            var path = startupPath + @"test\20,839453,476850.png";
            var a = Utils.GetColorsImage(path);
            
            int count = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    var color = a[i,j];
                    if (color.A == 255 && color.B == 255 && color.G == 255 && color.R == 255)
                    {
                        count++;
                    }
                    else
                    {
                        a[i, j] = Color.FromArgb(255,0, 0, 0 );
                    }
                }
            }
            /*
            var c = new int[3, 3] {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 0, 1, 0 }
            };
            a = Utils.LamMo(a, c);
            */
            Utils.SaveColorImage(@"D:\test10.png", a);
            Utils.SaveColorText(@"D:\test0.txt", a);
            var temp = new AnalystImage(a, 20, 839453, 476850);
            var m = new bool[3, 3] { { false, true, false }, { true, true, true }, { false, true, false } };
            var z = Utils.Giao(temp.Images, m);
            Utils.SaveColorText(@"D:\test1.txt", temp.Images);
            Utils.SaveColorText(@"D:\test2.txt", z);
        }
    }
}
