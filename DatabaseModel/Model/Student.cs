using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DatabaseModels
{
    [Table("Students")]
    public partial class Student
    {
        public Student()
        {
            Orders = new HashSet<Order>();
        }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Usn { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Login Login { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
