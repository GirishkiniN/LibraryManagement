using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModels;
using DTO;

namespace BusinessContracts
{
    public interface IStudentService
    {
        public IEnumerable<OrderDetails> GetOrderDetails(SpParameters parameters);

        public IEnumerable<SpResponse> PlaceOrder(SpParameters parameters);

        public IEnumerable<SpResponse> ReturnBook(SpParameters parameters);

        public IEnumerable<Book> GetBookList(SpParameters parameters);

        public string AddStudent(Student student);

        public Student GetStudentId(string usn);
        public IEnumerable<StudentProfile> GetStudentProfileDetail(int stdId);
    }
}
