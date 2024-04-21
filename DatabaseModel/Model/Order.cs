using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DatabaseModels
{
    [Table("Orders")]
    public partial class Order
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public virtual Book Book { get; set; }
        public virtual Student Student { get; set; }
    }
}
