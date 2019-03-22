using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Bvsp.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public class ReloadParam {

        [JsonConstructor]
        public ReloadParam() {
            BeatmapFile = string.Empty;
            BackgroundMusicFile = string.Empty;
            BackgroundVideoFile = string.Empty;
        }

        [JsonProperty("beatmap_file", Required = Required.DisallowNull)]
        [NotNull]
        public string BeatmapFile { get; set; }

        [JsonProperty("beatmap_index", Required = Required.DisallowNull)]
        public int BeatmapIndex { get; set; }

        [JsonProperty("beatmap_offset", Required = Required.DisallowNull)]
        public double BeatmapOffset { get; set; }

        [JsonProperty("background_music_file")]
        [CanBeNull]
        public string BackgroundMusicFile { get; set; }

        [JsonProperty("background_video_file")]
        [CanBeNull]
        public string BackgroundVideoFile { get; set; }

    }
}
