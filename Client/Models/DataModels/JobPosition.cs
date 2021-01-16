using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Models.DataModels
{
    public class JobPosition
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("organization")]
        public Guid Organization { get; set; }

        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("hierarchy_order")]
        public int HierarchyOrder { get; set; }

        [JsonPropertyName("department")]
        public Guid Department { get; set; }
    }
}
