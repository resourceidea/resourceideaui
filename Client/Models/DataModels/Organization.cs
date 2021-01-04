using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models.DataModels
{
    public class Organization
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameSlug { get; set; }

        public string Status { get; set; }
    }
}
