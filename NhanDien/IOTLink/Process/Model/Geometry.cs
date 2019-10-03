using Newtonsoft.Json;
using System.Collections.Generic;

namespace NhanDien.IOTLink.Process.Model
{
    /// <summary>
    /// Geometry
    /// </summary>
    public class Geometry
    {
        /// <summary>
        /// Coordinates
        /// </summary>
        [JsonProperty("coordinates")]
        public IList<object> Coordinates { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
