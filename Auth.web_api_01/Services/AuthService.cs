using Auth.Database_01.Data;
using Auth.Database_01.Entities;
using Auth.web_api_01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.web_api_01.Services
{
    public class AuthService
    {

        private readonly AppDbContext _context;

        private readonly IConfiguration _congrifiguration;

        public AuthService(AppDbContext context, IConfiguration congrifiguration)
        {
            _context = context;
            _congrifiguration = congrifiguration;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterReqModel request)
        {
            //check already user exist
            if (await _context.AppUsers.AnyAsync(u =>u.Email == request.Email))
            {
                return default!;
            }

            var newUser = new AppUser() 
            { Email = request.Email, Name = request.Name, Password = request.Password ,RoleId = request.RoleId};

            var hashPassword = new PasswordHasher<AppUser>().HashPassword(newUser, request.Password);

            newUser.Password = hashPassword;

            await _context.AppUsers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return new RegisterResponse { Name = newUser.Name ,Email = newUser.Email};
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel request)
        {


            var user = await _context.AppUsers.AsNoTracking()
                .Include(user => user.Role).ThenInclude(r => r!.RolePermissions)!.ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            //_context.Entry<AppUser>(user!).Reference(u => u.Role).Load();
            //var role = user.Role;
            //_context.Entry<Role>(role!).Collection(r => r.RolePermissions!).Load();
            //var rps = user.Role.RolePermissions;

            if (user is null) return default!;

            var checkPassword = new PasswordHasher<AppUser>().VerifyHashedPassword(user, user.Password, request.Password);

            if (user.Role is null) return default!;
            
            var roleName = user.Role?.RoleName ?? "";

            if (user.Role?.RolePermissions!.Count <= 0) return default!;
            
            var permissions = user!.Role!.RolePermissions!
                .Select(rp => rp.Permission!.PermissionName)
                .ToList();

            //var ps = await (from rp in _context.RolePermissions
            //                join p in _context.Permissions
            //                on rp.PermissionId equals p.PermissionId
            //                where rp.RoleId == user!.RoleId
            //                select p.PermissionName).ToListAsync();


            if(checkPassword == PasswordVerificationResult.Failed) return default!;

            var token =await this.GenerateToken(user);

            if(string.IsNullOrEmpty(token)) return default!;

            return new LoginResponseModel
            {
                Email = user.Email,
                Name = user.Name,
                Token = token,
                Role = roleName,
                Permissions = permissions,
            };

        }

        private async Task<string> GenerateToken(AppUser user)
        {
            List<Claim> claims = 
                [
                    new Claim(ClaimTypes.NameIdentifier , user.AppUserId.ToString()),
                    new Claim(ClaimTypes.Email , user.Email),
                ];

            if (user!.Role?.RoleName is not null || !string.IsNullOrEmpty(user!.Role?.RoleName))
            {
                var roleName = user.Role?.RoleName ?? "";
                claims.Add(new Claim(ClaimTypes.Role,roleName));
            }

            if (user!.Role!.RolePermissions is not null)
            {
                var permissions = user!.Role!.RolePermissions!
                .Select(rp => rp.Permission!.PermissionName)
                .ToList();

                if (permissions.Count > 0)
                {
                    foreach (var permission in permissions)
                    {
                        claims.Add(new Claim("permission",permission));
                    }
                }

            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_congrifiguration.GetValue<string>("JWT:SECRET")!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                    claims:claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                //issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                //audience: configuration.GetValue<string>("AppSettings:Audience")
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
    }
}
