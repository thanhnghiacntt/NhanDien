using Newtonsoft.Json;

namespace NhanDien.IOTLink.Process.Model
{
    /// <summary>
    /// Vị trí trên bản đồ
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Latitude
        /// </summary>
        [JsonProperty("lat")]
        public double Lat { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}
