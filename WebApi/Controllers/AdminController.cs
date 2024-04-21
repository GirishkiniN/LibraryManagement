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
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Policy = "AdminOnly")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminService adminService;

        public AdminController(IAdminService _adminService)
        {
            adminService = _adminService;
        }

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Admin get";
        }

        [HttpPost]
        [Route("Addbooks")]
        public JsonResult AddBooks([FromBody] Book book)
        {
            string result = adminService.AddBooks(book);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Updatebooks")]
        public JsonResult UpdateBooks([FromBody] Book book)
        {
            string result = adminService.UpdateBooks(book);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("getbooks")]
        public JsonResult GetBooks()
        {
            IEnumerable<Book> result = adminService.GetBooks();

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("vieworders")]
        public JsonResult ViewOrders()
        {
            IEnumerable<AdminOrderDetails> result = adminService.ViewOrders();

            return new JsonResult(result);
        }

    }
}
