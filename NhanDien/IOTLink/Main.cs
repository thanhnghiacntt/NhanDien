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
            var path = startupPath + @"test\image12.png";
            var a = Utils.GetColorsImage(path);
            Utils.SaveColorImage(@"D:\test10.png", a);
            Utils.SaveColorText(@"D:\test0.txt", a);
            //var bounds = new Bounds(12354,54212, 19);
            //var bounds = new Bounds("16.0583510542009,108.211898803711,16.0623101324165,108.218078613281", 19);
            var bounds = new Bounds("16.0583510542009,108.211898803711,16.0623101324165,108.218078613281", 19, false); // bounds image12.png
            //var bounds = new Bounds("16.05885,108.21255,16.06184,108.21782", 19);
            //var service = new ImageGoogleService(bounds);
            //Utils.SaveColorImage(@"D:\\image10.png", service.Color);
            var temp = new AnalystImageService(a, bounds);
            Utils.SaveColorImage(@"D:\test14.png", temp.Colors);
            Utils.SaveColorText(@"D:\test31.txt", temp.Colors);
            Utils.SaveColorText(@"D:\test30.txt", temp.Data);
            var b = System.IO.File.CreateText(@"D:\\test4.txt");
            b.WriteLine(Utils.ToString(temp.GeoJson));
            b.Close();
        }
    }
}
