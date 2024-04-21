using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DTO;
using BusinessContracts;
using DatabaseModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        IStudentService studentService;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IStudentService _studentService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.studentService = _studentService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var identityUser = new IdentityUser() { UserName = userDetails.UserName, Email = userDetails.Email };
            var result = await userManager.CreateAsync(identityUser, userDetails.Password);

            int role = (userDetails.UserName == "admin") ? 1 : 2;

            Student student = new Student()
            {
                StudentName = userDetails.Name,
                Usn = userDetails.UserName,
                RoleId = role
            };

            string addStdRes = studentService.AddStudent(student);

            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new { Message = "User Registration Failed " + addStdRes, Errors = dictionary });
            }

            return Ok(new { Message = "Pass" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            try
            {
                if (!ModelState.IsValid || credentials == null)
                {
                    return new BadRequestObjectResult(new { Message = "Login failed: Invalid credentials" });
                }

                var identityUser = await userManager.FindByNameAsync(credentials.Username);
                if (identityUser == null)
                {
                    return new BadRequestObjectResult(new { Message = "Login failed: User not Found" });
                }

                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return new BadRequestObjectResult(new { Message = "Login failed: Wrong Password" });
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, identityUser.Email),
                    new Claim(ClaimTypes.Name, identityUser.UserName)
                };

                if (credentials.Username == "admin")
                {
                    claims.Add(new Claim("admin", "true"));
                }
                else
                {
                    claims.Add(new Claim("student", "true"));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                Student stdId = studentService.GetStudentId(credentials.Username);

                return Ok(new { Message = "You are logged in", studentId = stdId.StudentId });
            }
            catch(Exception ex) 
            {
                return null;
            }
            
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "You are logged out" });
        }
    }
}
