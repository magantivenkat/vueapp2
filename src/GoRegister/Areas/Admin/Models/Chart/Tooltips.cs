using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Tooltips
    {
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("bodyFontColor")]
        public string BodyFontColor { get; set; }

        [JsonProperty("titleMarginBottom")]
        public int TitleMarginBottom { get; set; }

        [JsonProperty("titleFontColor")]
        public string TitleFontColor { get; set; }

        [JsonProperty("titleFontSize")]
        public int TitleFontSize { get; set; }

        [JsonProperty("borderColor")]
        public string BorderColor { get; set; }

        [JsonProperty("borderWidth")]
        public int BorderWidth { get; set; }

        [JsonProperty("xPadding")]
        public int XPadding { get; set; }

        [JsonProperty("yPadding")]
        public int YPadding { get; set; }

        [JsonProperty("displayColors")]
        public bool DisplayColors { get; set; }

        [JsonProperty("intersect")]
        public bool Intersect { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("caretPadding")]
        public int CaretPadding { get; set; }
    }
}