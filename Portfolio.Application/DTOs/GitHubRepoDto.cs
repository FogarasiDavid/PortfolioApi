using System;
using System.Text.Json.Serialization;

namespace Portfolio.Application.DTOs
{
    //a github mit ad vissza es mi a json sora neki
    public class GitHubRepoDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }
    }
}