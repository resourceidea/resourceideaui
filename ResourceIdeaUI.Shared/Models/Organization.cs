using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceIdeaUI.Shared.Models
{
    public class Organization
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameSlug { get; set; }

        public string Status { get; set; }
    }
}
