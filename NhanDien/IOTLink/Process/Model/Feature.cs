using Newtonsoft.Json;
using System.Collections.Generic;

namespace NhanDien.IOTLink.Process.Model
{
    /// <summary>
    /// Feature
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Properties
        /// </summary>
        [JsonProperty("properties")]
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Geometry
        /// </summary>
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }
}
