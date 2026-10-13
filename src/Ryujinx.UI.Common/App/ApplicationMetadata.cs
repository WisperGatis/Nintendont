using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Ryujinx.UI.App.Common
{
    public class ApplicationMetadata
    {
        public const int MaxTagLength = 32;

        public string Title { get; set; }
        public bool Favorite { get; set; }

        [JsonPropertyName("tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Tags { get; set; }

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
        /// Replaces <see cref="Tags"/> with a trimmed, de-duplicated copy of <paramref name="tags"/>.
        /// </summary>
        public void SetTags(IEnumerable<string> tags)
        {
            List<string> cleaned = (tags ?? Enumerable.Empty<string>())
                .Select(tag => tag?.Trim())
                .Where(tag => !string.IsNullOrEmpty(tag))
                .Select(tag => tag.Length > MaxTagLength ? tag[..MaxTagLength] : tag)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            Tags = cleaned.Count > 0 ? cleaned : null;
        }

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
