using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceIdeaUI.Shared.Models
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Token Token { get; set; }
    }
}
