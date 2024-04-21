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
    public class StudentService : IStudentService
    {
        LibraryManagementContext context = new LibraryManagementContext();

        public string AddStudent(Student student)
        {
            try
            {
                context.Students.Add(student);
                context.SaveChanges();
                return "pass";
            }
            catch (Exception E)
            {

                return "fail" + E;
            }
            
        }

        public IEnumerable<Book> GetBookList(SpParameters parameters)
        {
            IEnumerable<Book> books = context.BookLists.FromSqlRaw<Book>("EXEC usp_BookSearchList {0}", parameters.StudentId);

            return books;
        }

        public IEnumerable<OrderDetails> GetOrderDetails(SpParameters parameters)
        {
            IEnumerable<OrderDetails> orderDetails = context.OrderDetails.FromSqlRaw("EXEC usp_OrderDetails {0}", parameters.StudentId);


            return orderDetails;
        }

        public Student GetStudentId(string usn)
        {
            Student studentId = context.Students.FromSqlRaw<Student>("SELECT * FROM Students WHERE USN = {0}", usn).FirstOrDefault();

            return studentId;
        }

        public IEnumerable<StudentProfile> GetStudentProfileDetail(int stdId)
        {
            var studentProfile = context.StudentProfiless.FromSqlRaw<StudentProfile>(" EXEC [usp_getStudentProfile] {0} ",stdId).ToList();

            return studentProfile;
        }

        public IEnumerable<SpResponse> PlaceOrder(SpParameters parameters)
        {
            IEnumerable<SpResponse> orderDetails = context.PlaceAndReturn.FromSqlRaw("EXEC usp_PlaceOrder {0}, {1}", parameters.StudentId, parameters.BookId);

            return orderDetails;
        }

        public IEnumerable<SpResponse> ReturnBook(SpParameters parameters)
        {
            IEnumerable<SpResponse> orderDetails = context.PlaceAndReturn.FromSqlRaw("EXEC usp_ReturnBook {0}, {1}, {2}", parameters.StudentId, parameters.OrderId, parameters.BookId);

            return orderDetails;
        }
    }
}
