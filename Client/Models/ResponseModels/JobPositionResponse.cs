using Client.Models.DataModels;
using System;
using System.Text.Json.Serialization;

namespace Client.Models.ResponseModels
{
    public class JobPositionResponse
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("organization")]
        public Organization Organization { get; set; }

        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("hierarchy_order")]
        public int HierarchyOrder { get; set; }

        [JsonPropertyName("department")]
        public Department Department { get; set; }
    }
}
