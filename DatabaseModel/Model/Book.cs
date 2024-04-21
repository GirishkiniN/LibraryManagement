using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DatabaseModels
{
    [Table("Books")]
    public partial class Book
    {
        public Book()
        {
            Orders = new HashSet<Order>();
        }

        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string AuthorName { get; set; }
        public int Cost { get; set; }
        public int AvailableQuantity { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
