using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DatabaseModels
{
    [Table("Roles")]
    public partial class Role
    {
        public Role()
        {
            Students = new HashSet<Student>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
