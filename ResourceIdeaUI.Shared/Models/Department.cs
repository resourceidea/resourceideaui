using System;
using System.Text.Json.Serialization;

namespace ResourceIdeaUI.Shared.Models
{
    public class Department
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("organization")]
        public Guid? Organization { get; set; }

        [JsonPropertyName("id")]
        public Guid? Id { get; set; }
    }
}
