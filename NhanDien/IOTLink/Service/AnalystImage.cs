using NhanDien.IOTLink.Helper;
using NhanDien.IOTLink.Process.Algorithm;
using NhanDien.IOTLink.Process.Model;
using System.Collections.Generic;
using System.Drawing;

namespace NhanDien.IOTLink.Service
{
    /// <summary>
    /// Phân tích hình ảnh
    /// </summary>
    public class AnalystImage
    {
        /// <summary>
        /// Danh sách màu của hình
        /// </summary>
        public Color[,] Colors { get; private set; }

        /// <summary>
        /// Image 2 dim
        /// </summary>
        public bool[,] Image { get; private set; }

        /// <summary>
        /// Data
        /// </summary>
        public byte[,,] Data { get; private set; }

        /// <summary>
        /// Phân tích
        /// </summary>
        public bool[] Analyst { get; private set; }

        /// <summary>
        /// Bounds
        /// </summary>
        public Bounds Bounds { get; private set; }

        /// <summary>
        /// GeoJson
        /// </summary>
        public GeoJson GeoJson { get; private set; }

        /// <summary>
        /// Phân tích hình
        /// </summary>
        /// <param name="colors"></param>
        public AnalystImage(Color[,] colors, int z, int x, int y)
        {
            Colors = colors;
            Bounds = new Bounds(x, y, z);
            SetValue();
            UpdateGeoJson();
        }

        /// <summary>
        /// Phân tích hình
        /// </summary>
        /// <param name="colors"></param>
        /// <param name="box"></param>
        public AnalystImage(Color[,] colors, string box)
        {
            Colors = colors;
            Bounds = new Bounds(box);
            SetValue();
            UpdateGeoJson();
        }

        /// <summary>
        /// Set value
        /// </summary>
        private void SetValue()
        {
            var w = Colors.GetLength(0);
            var h = Colors.GetLength(1);
            var tempData = new byte[w + 2, h + 2, 1];
            Data = new byte[w, h, 1];
            Image = new bool[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    var argb = Colors[i, j].ToArgb();
                    if (argb == Constant.ArgbWhite || argb == Constant.ArgbTunnel || argb == Constant.ArgbYellow)
                    {
                        tempData[i + 1, j + 1, 0] = 255;
                    }
                    else
                    {
                        tempData[i + 1, j + 1, 0] = 0;
                    }
                }
            }
            MedianFilter.Filter(tempData);
            for (int i = 0; i < 5; i++)
            {
                ZhangSuen.ZhangSuenThinning(tempData);
            }
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Data[i, j, 0] = tempData[i + 1, j + 1, 0];
                    Image[i, j] = Data[i, j, 0] == 255 ? true : false;
                    if (Image[i, j])
                    {
                        Colors[i, j] = Color.Black;
                    }
                    else
                    {
                        Colors[i, j] = Color.White;
                    }
                }
            }
        }

        /// <summary>
        /// Update geojson
        /// </summary>
        private void UpdateGeoJson()
        {
            var w = Colors.GetLength(0);
            var h = Colors.GetLength(1);
            var geometry = new Geometry
            {
                Type = "MultiPoint",
                Coordinates = new List<object>()
            };
            var feateure = new Feature
            {
                Type = "Feature",
                Properties = new Dictionary<string, object>(),
                Geometry = geometry
            };
            GeoJson = new GeoJson()
            {
                Type = "FeatureCollection",
                Features = new List<Feature>()
                {
                    feateure
                }
            };
            var hash = new HashSet<string>();
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (Image[i,j])
                    {
                        var location = Utils.PixcelToLocation(i, j, w, h, Bounds);
                        var str = location.Lat + "," + location.Lng;
                        if (!hash.Contains(str))
                        {
                            hash.Add(str);
                            var temp = new List<double>
                            {
                                location.Lng,
                                location.Lat
                            };
                            geometry.Coordinates.Add(temp);
                        }
                        else
                        {
                            Image[i, j] = false;
                            Colors[i, j] = Color.White;
                            Data[i, j, 0] = 0;
                            System.Console.WriteLine("exist " + str);
                        }
                    }
                }
            }
        }
    }
}
