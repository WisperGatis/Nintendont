using System;
using System.Text.Json.Serialization;

namespace Ryujinx.UI.App.Common
{
    public class ApplicationMetadata
    {
        public string Title { get; set; }
        public bool Favorite { get; set; }

        [JsonPropertyName("timespan_played")]
        public TimeSpan TimePlayed { get; set; } = TimeSpan.Zero;

        [JsonPropertyName("last_played_utc")]
        public DateTime? LastPlayed { get; set; } = null;

        [JsonPropertyName("time_played")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public double TimePlayedOld { get; set; }

        [JsonPropertyName("last_played")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string LastPlayedOld { get; set; }

        /// <summary>
        /// Updates <see cref="LastPlayed"/>.
        /// </summary>
        public void UpdatePreGame()
        {
            LastPlayed = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds <paramref name="duration"/> to <see cref="TimePlayed"/> and updates <see cref="LastPlayed"/>.
        /// </summary>
        public void AddPlayTime(TimeSpan duration)
        {
            UpdatePreGame();

            if (duration > TimeSpan.Zero)
            {
                TimePlayed += duration;
            }
        }
    }
}
