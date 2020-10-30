using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceIdeaUI.Shared.Models
{
    public class User
    {
        public string username { get; set; }

        public string password { get; set; }

        public Token Token { get; set; }
    }
}
