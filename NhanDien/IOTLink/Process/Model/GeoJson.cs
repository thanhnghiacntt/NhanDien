using Newtonsoft.Json;
using System.Collections.Generic;

namespace NhanDien.IOTLink.Process.Model
{
    public class GeoJson
    {
        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Features
        /// </summary>
        [JsonProperty("features")]
        public IList<Feature> Features { get; set; }
    }
}
