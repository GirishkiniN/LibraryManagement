using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseModels
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public int StudentId { get; set; }
        public string Password { get; set; }
        public bool LoginStatus { get; set; }

        public virtual Student Student { get; set; }
    }
}
