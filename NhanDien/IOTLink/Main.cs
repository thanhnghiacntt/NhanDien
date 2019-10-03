using NhanDien.IOTLink.Process;


namespace NhanDien.IOTLink
{
    public static class Main
    {
        public static void Process()
        {

            string startupPath = @"D:\MyProject\C#\NhanDien\NhanDien\";
            //var path = startupPath + @"test\839572,476863,20.png";
            var path = startupPath + @"test\image1.png";
            var a = Utils.GetColorsImage(path);
            Utils.SaveColorImage(@"D:\test10.png", a);
            Utils.SaveColorText(@"D:\test0.txt", a);
            var temp = new AnalystImage(a, 20, 839453, 476850);
            Utils.SaveColorImage(@"D:\test12.png", temp.Colors);
            Utils.SaveColorText(@"D:\test2.txt", temp.Data);
        }
    }
}
