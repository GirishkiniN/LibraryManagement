using BusinessContracts;
using DatabaseModels;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessImplementation
{
    public class AdminService : IAdminService
    {
        LibraryManagementContext context = new LibraryManagementContext();
        public string AddBooks(Book book)
        {
            try
            {
                context.Books.Add(book);
                context.SaveChanges();
                return "Pass";

            }
            catch (Exception E)
            {
                return "Fail: " + E;
            }
        }

        public IEnumerable<Book> GetBooks()
        {
            var books = context.Books;
            return books;
        }

        public string UpdateBooks(Book book)
        {

            try
            {
                context.Books.Update(book);
                context.SaveChanges();
                return "Pass";

            }
            catch (Exception E)
            {
                return "Fail: " + E;
            }
        }

        public IEnumerable<AdminOrderDetails> ViewOrders()
        {
            IEnumerable<AdminOrderDetails> orders = context.AdminOrderDetailSp.FromSqlRaw(" EXEC [usp_AdminOrderDetails] ");
            return orders;
        }
    }
}
