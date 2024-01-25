﻿using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;
namespace XtramileBackend.Services.AuthService
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly AppDBContext _dbContext;
        private IConfiguration _config;
        public AuthenticationService(IConfiguration configuration, AppDBContext dbContext) {
            _dbContext = dbContext;
            _config = configuration;

        }

        public string checkCredentials(Credentials credential) {
            //check whether the user exists
            //get the userid and create token if userid exists
            //return to controller
            
            var userData = from user in _dbContext.TBL_USER where credential.Email == user.Email && credential.Password == user.Password select new { UserId = user.EmpId };
            var userId = userData?.FirstOrDefault()?.UserId;

            if (userId == null)
                return null;
            var token = GenerateToken((int)userId);
            return token;   
        }

        private string GenerateToken(int userId) {
            //get all the data from users
            //get the individual user data
            //set the roles for the users
            //set the auth claims - rolename, empid and empname 
            //create the token
            //return the token


            var userData = from user in _dbContext.TBL_EMPLOYEE where user.EmpId == userId select user;
            var loginUserData = userData?.FirstOrDefault();

            var authClaim = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier,loginUserData.EmpId.ToString()),
            new Claim(ClaimTypes.Name,loginUserData.FirstName)
            };

            var roleData = from role in _dbContext.TBL_ROLES
                           join user in userData
                           on role.RoleId equals user.RoleId
                           select new { role.RoleName };

     
            foreach (var userRole in roleData) {
                authClaim.Add(new Claim(ClaimTypes.Role, userRole.RoleName));
            }

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var SecurityToken = new JwtSecurityToken(
               _config["Jwt:Issuer"],
               _config["Jwt:Issuer"],
               authClaim,
               expires: DateTime.Now.AddMinutes(60),
               signingCredentials: credentials
               );

            var token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            Console.WriteLine(token);
            return token;

        }

    }
}
