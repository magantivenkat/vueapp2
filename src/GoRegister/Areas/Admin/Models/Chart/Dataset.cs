using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Dataset
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("lineTension")]
        public float LineTension { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("borderColor")]
        public string BorderColor { get; set; }

        [JsonProperty("pointRadius")]
        public int PointRadius { get; set; }

        [JsonProperty("pointBackgroundColor")]
        public string PointBackgroundColor { get; set; }

        [JsonProperty("pointBorderColor")]
        public string PointBorderColor { get; set; }

        [JsonProperty("pointHoverRadius")]
        public int PointHoverRadius { get; set; }

        [JsonProperty("pointHoverBackgroundColor")]
        public string PointHoverBackgroundColor { get; set; }

        [JsonProperty("pointHoverBorderColor")]
        public string PointHoverBorderColor { get; set; }

        [JsonProperty("pointHitRadius")]
        public int PointHitRadius { get; set; }

        [JsonProperty("pointBorderWidth")]
        public int PointBorderWidth { get; set; }

        [JsonProperty("data")]
        public int[] Data { get; set; }
    }
}