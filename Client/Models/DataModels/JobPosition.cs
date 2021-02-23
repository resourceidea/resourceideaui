using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Models.DataModels
{
    public class JobPosition
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("hierarchy_order")]
        public int HierarchyOrder { get; set; } = 0;

        [JsonPropertyName("department")]
        public Guid DepartmentId { get; set; }
    }
}
