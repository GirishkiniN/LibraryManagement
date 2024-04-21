using DatabaseModels;
using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessContracts
{
    public interface IAdminService
    {
        public string AddBooks(Book book);
        public string UpdateBooks(Book book);
        public IEnumerable<Book> GetBooks();
        public IEnumerable<AdminOrderDetails> ViewOrders();
    }
}
