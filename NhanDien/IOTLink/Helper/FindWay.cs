using NhanDien.IOTLink.Process.Model;
using System;
using System.Collections.Generic;
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
        private bool[,] image;

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
            this.image = (bool[,])image.Clone();
            this.bounds = bounds;
            hash = new HashSet<string>();
            Utils.SaveColorText(@"D:/test100.txt", image);
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
                    if (image[i, j])
                    {
                        features.Add(FindFeature(i, j));
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
            Feature feature = new Feature()
            {
                Type = "Feature",
                Geometry = new Geometry
                {
                    Type = type,
                    Coordinates = coordinates
                },
                Properties = new Dictionary<string, object>()
            };
            return feature;
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
                var isExist = false;
                var count = Utils.Count(i, j, w, h, image);
                if (count == 0)
                {
                    image[m, n] = false;
                    var temp = Utils.PixcelToLocation(m, n, w, h, bounds);
                    rs.Add(temp);
                    break;
                }
                else if (count == 1)
                {
                    image[m, n] = false;
                    var temp = Utils.PixcelToLocation(m, n, w, h, bounds);
                    rs.Add(temp);
                    if (Utils.IsTruePoint(i, j + 1, w, h, image))
                    {
                        j++;
                    }
                    if (Utils.IsTruePoint(i, j - 1, w, h, image))
                    {
                        j--;
                    }
                    if (Utils.IsTruePoint(i + 1, j, w, h, image))
                    {
                        i++;
                    }
                    if (Utils.IsTruePoint(i - 1, j, w, h, image))
                    {
                        i--;
                    }
                    if (Utils.IsTruePoint(i + 1, j + 1, w, h, image))
                    {
                        i++;
                        j++;
                    }
                    if (Utils.IsTruePoint(i + 1, j - 1, w, h, image))
                    {
                        i++;
                        j--;
                    }
                    if (Utils.IsTruePoint(i - 1, j + 1, w, h, image))
                    {
                        i--;
                        j++;
                    }
                    if (Utils.IsTruePoint(i - 1, j - 1, w, h, image))
                    {
                        i--;
                        j--;
                    }
                    var directions = Utils.FindDirection(i, j, w, h, image);
                    prex = directions.FirstOrDefault();
                }
                else
                {
                    var directions = Utils.FindDirection(i, j, w, h, image);
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
                        image[m, n] = false;
                        var temp = Utils.PixcelToLocation(m, n, w, h, bounds);
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
            Location a = temp[0];
            Location b = null;
            Location c = null;
            var list = new List<Location>
            {
                a
            };
            if (temp.Count > 3)
            {
                for (int i = 2; i < temp.Count; i++)
                {
                    b = temp[i - 1];
                    c = temp[i];
                    if (!IsWay(a,b,c))
                    {
                        list.Add(b);
                        a = b;
                    }
                }
                list.Add(c);
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
            if (Utils.Compare(ac, ab+bc))
            {
                return true;
            }
            return false;
        }
    }
}
