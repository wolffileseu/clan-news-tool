using System.Text.Json.Serialization;

namespace ClanNewsTool
{
    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = "";

        [JsonPropertyName("post_id")]
        public int? PostId { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

    public class ClanInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("tag")]
        public string Tag { get; set; } = "";

        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
    }

    public class VersionInfo
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = "";

        [JsonPropertyName("download_url")]
        public string? DownloadUrl { get; set; }

        [JsonPropertyName("changelog")]
        public string Changelog { get; set; } = "";

        [JsonPropertyName("force_update")]
        public bool ForceUpdate { get; set; }
    }

    public class NewsPost
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";

        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; set; }
    }

    public class EventPost
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";

        [JsonPropertyName("event_date")]
        public string EventDate { get; set; } = "";

        [JsonPropertyName("event_location")]
        public string? EventLocation { get; set; }
    }

    public class MatchPost
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";

        [JsonPropertyName("match_opponent")]
        public string MatchOpponent { get; set; } = "";

        [JsonPropertyName("match_result")]
        public string? MatchResult { get; set; }

        [JsonPropertyName("match_map")]
        public string? MatchMap { get; set; }

        [JsonPropertyName("event_date")]
        public string? EventDate { get; set; }
    }

    public class RecruitmentPost
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";

        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; set; }
    }
}