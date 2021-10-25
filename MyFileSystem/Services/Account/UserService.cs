using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.Account;
using MyFileSystem.Core.Entities;
using MyFileSystem.Services.Interfaces.Account;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MyFileSystem.Persistence.Interfaces;

namespace MyFileSystem.Services.Account
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IMapper mapper,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration config, IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<object> Login(LoginDto loginDto)
        {
            var logged = await _signInManager
                .PasswordSignInAsync(loginDto.UserName, loginDto.PasswordHash, false, false);

            if (!logged.Succeeded) throw new Exception("Invalid Username or Password! ");

            return new { Token = await GenerateJsonWebTokenAsync(loginDto) };
        }

        public async Task<object> AssignRoles(AssignRoleDto assignRoleDto)
        {
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId);

            if (user == null || assignRoleDto.Role == null)
                throw new Exception("User Not Found! ");

            if (await _userManager.IsInRoleAsync(user, assignRoleDto.Role))
                throw new Exception("Role Already Assigned! ");

            return await _userManager.AddToRoleAsync(user, assignRoleDto.Role);
        }

        public async Task<object> Logout()
        {
            await _signInManager.SignOutAsync();

            return "Logged out Successfully";
        }

        public async Task<object> Register(RegisterDto signUpDto)
        {
            try
            {
                var user = new ApplicationUser { UserName = signUpDto.UserName, Email = signUpDto.Email };
                var result = await _userManager.CreateAsync(user, signUpDto.Password);

                if (!result.Succeeded) throw new Exception("Register not completed ");

                return result;
            }
            catch (Exception ex)
            {
                return new { message = ex.Message };
            }
        }

        public async Task<PagedResultDto<LoginDto>> GetPagedUsers(int? pageIndex, int? pageSize)
        {
            var user = await _unitOfWork.AccountRepository
                .GetAllIncludedPagination(U => U.UserName != null, pageIndex, pageSize);

            return _mapper.Map<PagedResultDto<ApplicationUser>, PagedResultDto<LoginDto>>(user);
        }

        public async Task<LoginDto> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) throw new Exception("User Not Found! ");

            return _mapper.Map<ApplicationUser, LoginDto>(user);
        }

        private async Task<string> GenerateJsonWebTokenAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            var signingCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:SecurityKey"])),
                    SecurityAlgorithms.HmacSha256);

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(type: "Username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.Ticks.ToString(), ClaimValueTypes.Integer64)
            }.Union(userClaims);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}