using NhanDien.IOTLink.Helper;
using NhanDien.IOTLink.Service;

namespace NhanDien.IOTLink
{
    public static class Main
    {
        public static void Process()
        {
            string startupPath = @"D:\MyProject\C#\NhanDien\NhanDien\";
            //var path = startupPath + @"test\839572,476863,20.png";
            var path = startupPath + @"test\image5.png";
            var a = Utils.GetColorsImage(path);
            Utils.SaveColorImage(@"D:\test10.png", a);
            Utils.SaveColorText(@"D:\test0.txt", a);
            var temp = new AnalystImage(a, "16.06035,108.21150,16.06832,108.22510");
            Utils.SaveColorImage(@"D:\test14.png", temp.Colors);
            Utils.SaveColorText(@"D:\test3.txt", temp.Data);
            var b = System.IO.File.CreateText(@"D:\\test4.txt");
            b.WriteLine(Utils.ToString(temp.GeoJson));
            b.Close();
        }
    }
}
