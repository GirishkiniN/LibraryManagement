using BusinessContracts;
using DatabaseModels;
using DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
   //[Authorize(Policy = "StudentOnly")]
    public class StudentController : ControllerBase
    {
        IStudentService studentService;
        public StudentController(IStudentService _studentService)
        {
            studentService = _studentService;
        }

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Student get";
        }

        [HttpGet]
        [Route("GetStudentProfile")]
        public StudentProfile GetStudentProfile(int id)
        {
           //IEnumerable<StudentProfile> studentProfile = studentService.GetStudentProfileDetail(id);
            StudentProfile studentProfile = studentService.GetStudentProfileDetail(id).FirstOrDefault();
            return studentProfile;
        }

        [HttpPost]
        [Route("GetBookList")]
        public JsonResult GetBookLists([FromBody] SpParameters parameters)
        {
            IEnumerable<Book> books = studentService.GetBookList(parameters);

            return new JsonResult(books);
        }

        [HttpPost]
        [Route("GetOrderDetails")]
        public JsonResult GetOrderDetails([FromBody] SpParameters parameters)
        {
            IEnumerable<OrderDetails> orderDetails = studentService.GetOrderDetails(parameters);

/*
            foreach (OrderDetails order in orderDetails)
            {
                order.Fine = (order.ExpectedReturnDate < DateTime.Today) ? Convert.ToInt32((DateTime.Today - order.ExpectedReturnDate).TotalDays) * 5 : 0;
            }*/

            return new JsonResult(orderDetails);
        }

        [HttpPost]
        [Route("PlaceOrder")]
        public JsonResult PlaceOrder([FromBody] SpParameters parameters)
        {
            IEnumerable<SpResponse> book = studentService.PlaceOrder(parameters);

            return new JsonResult(book);
        }

        [HttpPost]
        [Route("ReturnBook")]
        public JsonResult ReturnBook([FromBody] SpParameters parameters)
        {
            IEnumerable<SpResponse> book = studentService.ReturnBook(parameters);

            return new JsonResult(book);
        }

    }
}
