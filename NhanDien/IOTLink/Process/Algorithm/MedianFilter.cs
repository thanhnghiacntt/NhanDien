using Accord.Math;

namespace NhanDien.IOTLink.Process.Algorithm
{
    public static class MedianFilter
    {
        public static void Filter(byte[,,]data)
        {
            byte[] temp = new byte[9];
            int w = data.GetLength(0);
            int h = data.GetLength(1);
            for (int i = 1; i < w - 1; i++)
            {
                for (int j = 1; j < h - 1; j++)
                {
                    temp[0] = data[i - 1, j - 1, 0];
                    temp[1] = data[i - 1, j, 0];
                    temp[2] = data[i - 1, j + 1, 0];
                    temp[3] = data[i, j - 1, 0];
                    temp[4] = data[i, j, 0];
                    temp[5] = data[i, j + 1, 0];
                    temp[6] = data[i + 1, j - 1, 0];
                    temp[7] = data[i + 1, j, 0];
                    temp[8] = data[i + 1, j + 1, 0];
                    temp.Sort();
                    data[i, j, 0] = temp[4];
                }
            }
        }
    }
}
