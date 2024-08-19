using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Topic2.Models.Dto;
using Topic2.Models.Dto.Respon;
using Topic2.Models.IRepository;

namespace Topic2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepositories tokenRepositories;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepositories tokenRepositories)
        {
            this._userManager = userManager;
            this.tokenRepositories = tokenRepositories;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestdDto registerRequestdDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestdDto.Username,
                Email = registerRequestdDto.Username
            };
            var Iresult = await _userManager.CreateAsync(identityUser, registerRequestdDto.Password);
            return Ok(new { message = "Register successfully", data = Iresult });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRegisterDto loginRegisterDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRegisterDto.Username);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                var Iresult = await _userManager.CheckPasswordAsync(user, loginRegisterDto.Password);
                if (!Iresult)
                {
                    return BadRequest(new { message = "Incorrect password" });
                }

                // Create JWT token
                var jwtToken = tokenRepositories.CreateJWTToken(user);
                var response = new ResultCustom<string>()
                {
                    Status = Models.Dto.Respon.StatusCode.CREATED,
                    Data = jwtToken,
                    Message = new string[] { "Login successfully" }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine("Exception during login: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                return BadRequest(new { message = "Login failed", details = ex.Message });
            }
        }
    }
}