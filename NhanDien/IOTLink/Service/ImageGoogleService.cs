using NhanDien.IOTLink.Helper;
using System.Drawing;
using System.IO;

namespace NhanDien.IOTLink.Service
{
    public class ImageGoogleService
    {
        private string url = @"https://maps.googleapis.com/maps/vt?pb=!1m5!1m4!1i{z}!2i{x}!3i{y}!4i256!2m3!1e0!2sm!3i485193772!3m17!2svi-VN!3sUS!5e18!12m4!1e68!2m2!1sset!2sRoadmap!12m3!1e37!2m1!1ssmartmaps!12m4!1e26!2m2!1sstyles!2zcy50OjJ8cC52Om9mZixzLmU6bHxwLnY6b2ZmLHMudDo4MXxwLnY6b2Zm!4e0&key=AIzaSyA2Zb2vY8-t_9BUYqFFjc9LQiNWUZPLft4&token=53040";

        public Bounds Bounds { get; private set; }

        public Color[,] Color { get; set; }

        public ImageGoogleService(Bounds bounds)
        {
            Bounds = bounds;
            SetColor();
        }

        /// <summary>
        /// Get stream
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Stream GetStream(int x, int y, int z)
        {
            var temp = url.Replace("{x}", x + "");
            temp = temp.Replace("{y}", y + "");
            temp = temp.Replace("{z}", z + "");
            return HttpClient.ReadStream(temp);
        }

        /// <summary>
        /// Set color
        /// </summary>
        public void SetColor()
        {
            int x = Bounds.TileMax.X - Bounds.TileMin.X + 1;
            int y = Bounds.TileMax.Y - Bounds.TileMin.Y + 1;
            Color = new Color[(x * 256) + 1, (y * 256) + 1];
            int w = 0;
            int width = 0;
            int height = 0;
            for (int i = Bounds.TileMin.X; i <= Bounds.TileMax.X; i++)
            {
                int h = 0;
                for (int j = Bounds.TileMin.Y; j <= Bounds.TileMax.Y; j++)
                {
                    var stream = GetStream(i, j, Bounds.Zoom);
                    var image = Utils.GetColorsImage(stream);
                    width = image.GetLength(0);
                    height = image.GetLength(1);
                    for (int k = 0; k < width; k++)
                    {
                        for (int l = 0; l < height; l++)
                        {
                            Color[w + k, h + l] = image[k, l];
                        }
                    }
                    h += height;
                }
                w += width;
            }
        }
    }
}
