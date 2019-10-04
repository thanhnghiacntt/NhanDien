using NhanDien.IOTLink.Process.Model;
using System.Collections.Generic;

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
        /// Constructor
        /// </summary>
        /// <param name="image"></param>
        public FindWay(bool[,] image)
        {
            this.image = (bool[,])image.Clone();
            hash = new HashSet<string>();
        }


        /// <summary>
        /// Update geojson
        /// </summary>
        private GeoJson FindGeoJson()
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
                    temp[1].Lat
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
            var w = image.GetLength(0);
            var h = image.GetLength(1);
            var m = i;
            var n = j;
            while (true)
            {
                if (m < w - 1 && m > 1 && n < h - 1 && n > 1)
                {
                    var count = 0;
                    for (int k = m - 1; k <= m + 1; k++)
                    {
                        for (int l = n - 1; l <= n + 1; l++)
                        {
                            if (image[k,l])
                            {
                                count++;
                            }
                        }
                    }
                    if (count == 4)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return rs;
        }
    }
}
