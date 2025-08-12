using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Server.DTO;
using Server.Models;
using Server.Utils;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        //Create user
        [HttpPost("create-user/{role}")]
        public async Task<IActionResult> CreateSecretary(string role, [FromQuery] int creatorId, [FromBody] UserRegistrationDto dto)
        {

            role = role.Trim().ToLower();

            if (!new[] { "admin", "secretary", "dealer", "boy" }.Contains(role))
                return BadRequest(new { message = "invalid role specified" });

            // fetch creator from db
            var creator = await _context.Users.FindAsync(creatorId);
            if (creator == null)
                return NotFound(new { message = "creator not found" });
            var creatorRole = creator.Role.ToLower();

            // Block creation if not Admin or Secretary
            if (creatorRole != "admin" && creatorRole != "secretary")
                return BadRequest(new { message = "Only admins and secretaries can create new users." });

            // Role-based creation rules
            if (creatorRole == "admin" && role != "secretary" && role != "admin")
                return BadRequest(new { message = "Admins can only create secretaries." });

            if (creatorRole == "secretary" && !new[] { "dealer", "boy" }.Contains(role))
                return BadRequest(new { message = "Secretaries can only create dealers or boys." });


            string? loginId = null;

            if (role == "secretary" || role == "admin")
            {
                loginId = AuthUtils.GenerateLoginId();
            }

            var user = new User
            {
                FullName = dto.FullName,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Sex = dto.Sex,
                Nin = dto.Nin,
                Branch = dto.Branch,
                Role = char.ToUpper(role[0]) + role.Substring(1),
                ProfilePicture = dto.ProfilePicture,
                LoginId = loginId,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = $"{role} created successfully.",
                LoginId = loginId,
            });
        }

        // Get user
        [HttpGet("get-users/{UserId}")]
        public async Task<IActionResult> GetUsers(int UserId, [FromQuery] string role, string? name)
        {
            role = role.Trim().ToLower();
            if (!new[] { "dealer", "admin", "secretary", "boy" }.Contains(role))
                return BadRequest(new { message = "Invalid Role requested" });

            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
                return BadRequest(new { message = "User not found" });

            var userRole = user.Role.ToLower();

            if (userRole != "secretary" && userRole != "admin")
                return BadRequest(new { message = "only admins and secretaries can access this information" });

            if (role == "secretary" || role == "admin" && userRole != "admin")
                return BadRequest(new { message = "only admins can access this information" });

            var query = _context.Users.Where(u => u.Role.ToLower() == role);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.FullName == name);
            }

            var Users = await query.ToListAsync();

            return Ok(Users);
        }

        // Login
        [HttpPost("login/send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] UserLoginDto loginDto)
        {

            if (string.IsNullOrEmpty(loginDto.LoginId))
                return BadRequest(new { message = "loginId needed" });

            // find user by loginId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginId == loginDto.LoginId);

            if (user == null)
                return BadRequest(new { message = "user not found" });

            // generate Otp and send via email
            var otpCode = new Random().Next(100000, 999999);
            var subject = "your login otp is";
            var body = $"Hello {user.FullName}, \n\nYour Otp Code is: {otpCode}\nThis code will expire in 5 minutes.";

            var _emailService = new MailService();

            await _emailService.SendEmailAsync(user.Email, subject, body);

            // storing otp in db
            user.otp = otpCode;
            user.OtpSentAt = DateTime.UtcNow;
            user.isAuthenticated = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = "otp sent to your email" });

        }

        [HttpPost("login/verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "LoginId and otp required" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginId == loginDto.LoginId);

            if (user == null)
                return BadRequest(new { message = "user not found" });

            if (user.otp == null)
                return BadRequest(new { message = "Otp hasnt been sent to this account yet" });

            if ((DateTime.UtcNow - user.OtpSentAt.Value.ToUniversalTime()).TotalMinutes > 5)
            {
                user.otp = null;
                user.OtpSentAt = null;
                await _context.SaveChangesAsync();
                return BadRequest(new { message = "Otp expired" });
            }
            

            if (user.otp != loginDto.otp)
                return BadRequest(new { message = "otp mismatch" });

            //save changes and delete otp from db
            user.isAuthenticated = true;
            user.otp = null;
            user.OtpSentAt = null;
            user.AuthenticatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();


            return Ok(new { message = "user successfully verified", loginId = user.LoginId });
        }

        [HttpGet("check-auth/{loginId}")]
        public async Task<IActionResult> CheckAuth(string loginId)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "LoginId Required" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginId == loginId);

            if (user == null)
                return BadRequest(new { message = "user not found" });

            if (user.Role != "Secretary" && user.Role != "Admin")
                return BadRequest(new { message = "only and secretaries admins can be authenticated" });

            if (user.isAuthenticated != true )
                return BadRequest(new { message = "user is not authenticated" });
            

            if ((DateTime.UtcNow - user.AuthenticatedAt.Value.ToUniversalTime()).TotalHours > 7)
            {
                user.isAuthenticated = false;
                user.AuthenticatedAt = null;
                await _context.SaveChangesAsync();
                return BadRequest(new { message = "Session Expired" });
            }

            var dbUser = new
            {
                user.LoginId,
                user.FullName,
                user.Branch,
                user.Address,
                user.Email,
                user.Nin,
                user.ProfilePicture
            };

            return Ok(new { message = "user authenticated",user = dbUser });

        }
    }
}