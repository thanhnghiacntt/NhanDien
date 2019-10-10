using NhanDien.IOTLink.Process.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NhanDien.IOTLink.Helper
{
    /// <summary>
    /// Find way in image
    /// </summary>
    public class FindWay
    {
        /// <summary>
        /// Image
        /// </summary>
        private int[,] image;

        /// <summary>
        /// Hash set content node
        /// </summary>
        private readonly HashSet<string> hash;

        /// <summary>
        /// Bounds
        /// </summary>
        private readonly Bounds bounds;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="image"></param>
        /// <param name="bounds"></param>
        public FindWay(bool[,] image, Bounds bounds)
        {
            this.image = ImageToByte(image);
            this.bounds = bounds;
            hash = new HashSet<string>();
            Utils.SaveColorText(@"D:/test100.txt", image);

            Utils.SaveColorCSV(@"D:/test100.csv", image);
        }

        /// <summary>
        /// Update geojson
        /// </summary>
        public GeoJson FindGeoJson()
        {
            var geoJson = new GeoJson
            {
                Type = "FeatureCollection",
                Features = FindFeatures()
            };
            return geoJson;
        }

        /// <summary>
        /// List feature
        /// </summary>
        /// <returns></returns>
        private IList<Feature> FindFeatures()
        {
            var features = new List<Feature>();
            var w = image.GetLength(0);
            var h = image.GetLength(1);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (image[i, j] == 1)
                    {
                        var feature = FindFeature(i, j);
                        if (!feature.Geometry.Type.Equals("Point"))
                        {
                            features.Add(feature);
                        }
                    }
                }
            }
            return features;
        }

        /// <summary>
        /// List feature
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private Feature FindFeature(int i, int j)
        {
            var temp = FindListLocation(i, j);
            temp = ProjectWay(temp);
            var coordinates = new List<object>();
            foreach (var item in temp)
            {
                var point = new List<double>
                {
                    item.Lng,
                    item.Lat
                };
                coordinates.Add(point);
            }
            var type = "LineString";
            if (coordinates.Count == 1)
            {
                type = "Point";
                coordinates = new List<object>
                {
                    temp[0].Lng,
                    temp[0].Lat
                };
            }
            var color = RandomColor();
            Feature feature = new Feature()
            {
                Type = "Feature",
                Geometry = new Geometry
                {
                    Type = type,
                    Coordinates = coordinates
                },
                Properties = new Dictionary<string, object>()
                {
                    {"stroke", HexConverter(color)}
                }
            };
            return feature;
        }

        private string HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        /// <summary>
        /// Random color
        /// </summary>
        /// <returns></returns>
        private Color RandomColor()
        {
            Random r = new Random();
            int random = r.Next(0, 10);
            switch (random)
            {
                case 0:
                    return Color.Red;
                case 1:
                    return Color.Black;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.AliceBlue;
                case 4:
                    return Color.Yellow;
                case 5:
                    return Color.Gray;
                case 6:
                    return Color.Green;
                case 7:
                    return Color.HotPink;
                case 8:
                    return Color.LightBlue;
                case 9:
                    return Color.MintCream;
                default:
                    return Color.White;
            }
        }
        /// <summary>
        /// Find list location
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private IList<Location> FindListLocation(int i, int j)
        {
            var rs = new List<Location>();
            var m = i;
            var n = j;
            var w = image.GetLength(0);
            var h = image.GetLength(1);
            var prex = Direction.Center;
            while (true)
            {
                m = i;
                n = j;
                var temp = Utils.PixcelToLocation(m, n, w, h, bounds);
                var isExist = false;
                var count = Utils.Count(i, j, w, h, image, 1);
                if (count == 0)
                {
                    image[m, n] = 0;
                    rs.Add(temp);
                    /*
                    count = Utils.Count(i, j, w, h, image, -1);
                    if (count == 1)
                    {
                        var directions = Utils.FindDirection(i, j, w, h, image, -1);
                        prex = directions.FirstOrDefault();
                        if (Utils.IsTruePoint(i, j + 1, w, h, image, -1))
                        {
                            j++;
                        }
                        else if (Utils.IsTruePoint(i, j - 1, w, h, image, -1))
                        {
                            j--;
                        }
                        else if (Utils.IsTruePoint(i + 1, j, w, h, image, -1))
                        {
                            i++;
                        }
                        else if (Utils.IsTruePoint(i - 1, j, w, h, image, -1))
                        {
                            i--;
                        }
                        else if (Utils.IsTruePoint(i + 1, j + 1, w, h, image, -1))
                        {
                            i++;
                            j++;
                        }
                        else if (Utils.IsTruePoint(i + 1, j - 1, w, h, image, -1))
                        {
                            i++;
                            j--;
                        }
                        else if (Utils.IsTruePoint(i - 1, j + 1, w, h, image, -1))
                        {
                            i--;
                            j++;
                        }
                        else if (Utils.IsTruePoint(i - 1, j - 1, w, h, image, -1))
                        {
                            i--;
                            j--;
                        }
                    }
                    else
                    {
                        break;
                    }*/
                    break;
                }
                else if (count == 1)
                {
                    image[m, n] = 0;
                    rs.Add(temp);
                    var directions = Utils.FindDirection(i, j, w, h, image, 1);
                    prex = directions.FirstOrDefault();
                    if (Utils.IsTruePoint(i, j + 1, w, h, image, 1))
                    {
                        j++;
                    }
                    else if (Utils.IsTruePoint(i, j - 1, w, h, image, 1))
                    {
                        j--;
                    }
                    else if (Utils.IsTruePoint(i + 1, j, w, h, image, 1))
                    {
                        i++;
                    }
                    else if (Utils.IsTruePoint(i - 1, j, w, h, image, 1))
                    {
                        i--;
                    }
                    else if (Utils.IsTruePoint(i + 1, j + 1, w, h, image, 1))
                    {
                        i++;
                        j++;
                    }
                    else if (Utils.IsTruePoint(i + 1, j - 1, w, h, image, 1))
                    {
                        i++;
                        j--;
                    }
                    else if (Utils.IsTruePoint(i - 1, j + 1, w, h, image, 1))
                    {
                        i--;
                        j++;
                    }
                    else if (Utils.IsTruePoint(i - 1, j - 1, w, h, image, 1))
                    {
                        i--;
                        j--;
                    }
                }
                else
                {
                    var directions = Utils.FindDirection(i, j, w, h, image, 1);
                    Location locaion = Utils.PixcelToLocation(i, j, w, h, bounds);
                    switch (prex)
                    {
                        case Direction.Center:
                            if (directions.Contains(Direction.Right))
                            {
                                prex = Direction.Right;
                                i++;
                            }
                            else if (directions.Contains(Direction.Bottom))
                            {
                                prex = Direction.Bottom;
                                j++;
                            }
                            else if (directions.Contains(Direction.Left))
                            {
                                prex = Direction.Left;
                                i--;
                            }
                            else if (directions.Contains(Direction.Top))
                            {
                                prex = Direction.Top;
                                j--;
                            }
                            else if (directions.Contains(Direction.TopRight))
                            {
                                prex = Direction.TopRight;
                                i++;
                                j--;
                            }
                            else if (directions.Contains(Direction.TopLeft))
                            {
                                prex = Direction.TopLeft;
                                i--;
                                j--;
                            }
                            else if (directions.Contains(Direction.BottomLeft))
                            {
                                prex = Direction.BottomLeft;
                                i--;
                                j++;
                            }
                            else if (directions.Contains(Direction.BottomRight))
                            {
                                prex = Direction.BottomRight;
                                i++;
                                i++;
                            }
                            break;
                        case Direction.Top:
                            if (directions.Contains(Direction.Top))
                            {
                                prex = Direction.Top;
                                j--;
                            }
                            else if (directions.Contains(Direction.TopRight))
                            {
                                prex = Direction.TopRight;
                                i++;
                                j--;
                            }
                            else if (directions.Contains(Direction.TopLeft))
                            {
                                prex = Direction.TopLeft;
                                i--;
                                j--;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        case Direction.Bottom:
                            if (directions.Contains(Direction.Bottom))
                            {
                                prex = Direction.Bottom;
                                j++;
                            }
                            else if (directions.Contains(Direction.BottomLeft))
                            {
                                prex = Direction.BottomLeft;
                                i--;
                                j++;
                            }
                            else if (directions.Contains(Direction.BottomRight))
                            {
                                prex = Direction.BottomRight;
                                i++;
                                i++;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        case Direction.Left:
                            if (directions.Contains(Direction.Left))
                            {
                                prex = Direction.Left;
                                i--;
                            }
                            else if (directions.Contains(Direction.TopLeft))
                            {
                                prex = Direction.TopLeft;
                                i--;
                                j--;
                            }
                            else if (directions.Contains(Direction.BottomLeft))
                            {
                                prex = Direction.BottomLeft;
                                i--;
                                j++;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        case Direction.Right:
                            if (directions.Contains(Direction.Right))
                            {
                                prex = Direction.Right;
                                i++;
                            }
                            else if (directions.Contains(Direction.TopRight))
                            {
                                prex = Direction.TopRight;
                                i++;
                                j--;
                            }
                            else if (directions.Contains(Direction.BottomRight))
                            {
                                prex = Direction.BottomRight;
                                i++;
                                i++;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        case Direction.TopLeft:
                            if (directions.Contains(Direction.TopLeft))
                            {
                                prex = Direction.TopLeft;
                                i--;
                                j--;
                            }
                            else if (directions.Contains(Direction.Left))
                            {
                                prex = Direction.Left;
                                i--;
                            }
                            else if (directions.Contains(Direction.Top))
                            {
                                prex = Direction.Top;
                                j--;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        case Direction.TopRight:
                            if (directions.Contains(Direction.TopRight))
                            {
                                prex = Direction.TopRight;
                                i++;
                                j--;
                            }
                            else if (directions.Contains(Direction.Right))
                            {
                                prex = Direction.Right;
                                i++;
                            }
                            else if (directions.Contains(Direction.Top))
                            {
                                prex = Direction.Top;
                                j--;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        case Direction.BottomLeft:
                            if (directions.Contains(Direction.BottomLeft))
                            {
                                prex = Direction.BottomLeft;
                                i--;
                                j++;
                            }
                            else if (directions.Contains(Direction.Bottom))
                            {
                                prex = Direction.Bottom;
                                j++;
                            }
                            else if (directions.Contains(Direction.Left))
                            {
                                prex = Direction.Left;
                                i--;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        case Direction.BottomRight:
                            if (directions.Contains(Direction.BottomRight))
                            {
                                prex = Direction.BottomRight;
                                i++;
                                i++;
                            }
                            else if (directions.Contains(Direction.Right))
                            {
                                prex = Direction.Right;
                                i++;
                            }
                            else if (directions.Contains(Direction.Bottom))
                            {
                                prex = Direction.Bottom;
                                j++;
                            }
                            else
                            {
                                isExist = true;
                            }
                            break;
                        default:
                            break;
                    }
                    if (isExist)
                    {
                        break;
                    }
                    else
                    {
                        image[m, n] = 0;
                        rs.Add(temp);
                    }
                }
            }
            return rs;
        }

        /// <summary>
        /// Xử lý loại bỏ các điểm trên cùng 1 đường thẳng
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private IList<Location> ProjectWay(IList<Location> temp)
        {
            var list = new List<Location>();
            if (temp.Count > 3)
            {
                var f = 0;
                var l = temp.Count - 1;
                var a = f;
                var b = l;
                var max = 0;
                while(f != l)
                {
                    var count = 0;
                    for (var i = f + 1; i < l - 1; i++)
                    {
                        if (IsWay(temp[f], temp[i], temp[l]))
                        {
                            count++;
                        }
                    }
                    if (count > max)
                    {
                        max = count;
                        a = f;
                        b = l;
                    }
                    if (l - f < max)
                    {
                        break;
                    }
                    l--;
                }
                for (int i = 0; i <= a; i++)
                {
                    list.Add(temp[i]);
                }
                for (var i = a + 1; i < b; i++)
                {
                    if (!IsWay(temp[a], temp[i], temp[b]))
                    {
                        list.Add(temp[i]);
                    }
                }
                for (int i = b; i < temp.Count; i++)
                {
                    list.Add(temp[i]);
                }
            }
            else
            {
                return temp;
            }
            return list;
        }

        /// <summary>
        /// 3 điểm a,b,c có nằm trên cùng đường thẳng hay không
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsWay(Location a, Location b, Location c)
        {
            var ab = Math.Sqrt(Math.Pow(a.Lat - b.Lat, 2) + Math.Pow(a.Lng - b.Lng, 2));
            var bc = Math.Sqrt(Math.Pow(b.Lat - c.Lat, 2) + Math.Pow(b.Lng - c.Lng, 2));
            var ac = Math.Sqrt(Math.Pow(a.Lat - c.Lat, 2) + Math.Pow(a.Lng - c.Lng, 2));
            if (Utils.Compare(ac, ab+bc, 0.000005))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private int[,] ImageToByte(bool[,] image)
        {
            var w = image.GetLength(0);
            var h = image.GetLength(1);
            var rs = new int[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    rs[i, j] = image[i, j] ? 1 : 0;
                }
            }
            return rs;
        }

    }
}
