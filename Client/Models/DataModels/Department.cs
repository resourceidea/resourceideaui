using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Models.DataModels
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
