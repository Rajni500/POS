using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using POS.Contract;
using POS.Controllers.Core;
using POS.Models;
using POS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Controllers
{
    [EnableCors("myAllowSpecificOrigins")]
    [Route("Account/")]
    public class AccountController : GenericController<User>
    {
        public AccountController(IAccountRepository repository, IServiceProvider serviceProvider, ICache cache)
            : base(repository,cache)
        {
            base.genericRepository = repository;
            base.serviceProvider = serviceProvider;
            this.cache = cache;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterUser")]
        public ActionResult RegisterUser([FromBody] User entity)
        {
            try
            {
                if(!Regex.IsMatch(entity.Email, "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$"))
                {
                    return ExceptionHandler.throwException("\"" + entity.Email + "\" is not a valid email");
                }
                if(!Regex.IsMatch(entity.Password, "^.*(?=.{8,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$"))
                {
                    return ExceptionHandler.throwException("\"" + entity.Password + "\" is not a valid password. It should be 8 characters or more and should contain atleast a Digit, a Lowercse letter, a Uppercase letter and a Special character.");
                }
                if (!Regex.IsMatch(entity.PhoneNumber, "^[6-9]{1}[0-9]{9}$"))
                {
                    return ExceptionHandler.throwException("\"" + entity.PhoneNumber + "\" is not a valid phone number.");
                }
                if (string.IsNullOrWhiteSpace(entity.Title))
                {
                    return ExceptionHandler.throwException("Name should not be empty.");
                }

                var existingUser = FindByEmail(entity);
                if (existingUser != null && existingUser.Count > 0)
                    return ExceptionHandler.throwException("Email \"" + entity.Email + "\" is already taken");

                byte[] passwordHash, passwordSalt;
                EncryptionManager.ComputeHashForPassword(entity.Password, out passwordHash, out passwordSalt);

                entity.PasswordHash = passwordHash;
                entity.PasswordSalt = passwordSalt;

                var entityAdded = genericRepository.Add(entity);
                entityAdded.Password = null;
                entityAdded.PasswordHash = null;
                entityAdded.PasswordSalt = null;

                GenerateTokenString(entityAdded, out string tokenString);

                SetCache(tokenString, entityAdded);

                return Json(new
                {
                    ProfileDetails = entityAdded,
                    TokenString = tokenString
                });
            }
            catch(Exception ex)
            {
                return ExceptionHandler.throwException(ex.Message);
            }
        }

        private List<User> FindByEmail(User entity)
        {
            return genericRepository.FindBy(x => x.Email == entity.Email, new ViewModels.SearchViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("LogOn")]
        public ActionResult LogOn([FromBody] User entity)
        {
            try
            {
                string tokenString = string.Empty;
                User user;
                user = Authenticate(entity);

                if (user == null)
                {
                    return ExceptionHandler.throwException("Username or password is incorrect");
                }
                else
                {
                    GenerateTokenString(user, out tokenString);

                    SetCache(tokenString, user);
                }

                return Json(new
                {
                    ProfileDetails = user,
                    TokenString = tokenString
                });
            }
            catch(Exception ex)
            {
                return ExceptionHandler.throwException(ex.Message);
            }
        }

        private User Authenticate(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                    return null;

                var userDetail = FindByEmail(user).FirstOrDefault();

                // check if username exists
                if (userDetail == null)
                    return null;

                // check if password is correct
                if (!EncryptionManager.VerifyPasswordHash(user.Password, userDetail.PasswordHash, userDetail.PasswordSalt))
                    return null;

                return userDetail;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateTokenString(User user, out string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Constants.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenString = tokenHandler.WriteToken(token);
        }

        private void SetCache(string tokenString, User profile)
        {
            string loggedInUserkey = string.Format("{0}_{1}", tokenString, Constants.LoggedInUser);
            var cacheEntryOptions = GetCacheEntryOptions();

            cache.Set(tokenString, tokenString, cacheEntryOptions);
            cache.Set(loggedInUserkey, profile, cacheEntryOptions);
        }

        private MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            var timeSpan = TimeSpan.FromMinutes(Constants.IdleTimeout);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
           .SetSlidingExpiration(timeSpan);
            cacheEntryOptions.SetAbsoluteExpiration(timeSpan);
            cacheEntryOptions.SetPriority(CacheItemPriority.NeverRemove);
            return cacheEntryOptions;
        }

        [HttpPost]
        [AuthorizeLogin]
        [Route("LogOff")]
        public ActionResult LogOff()
        {
            string bearerToken = HttpContext.Request.Headers["Authorization"];
            string headerToken = bearerToken.Split(' ')[1];

            RemoveFromCache(headerToken);
            return Json("Logged Out");
        }

        private void RemoveFromCache(string token)
        {
            string loggedInUserkey = string.Format("{0}_{1}", token, Constants.LoggedInUser);

            object loggedInUserCache = cache.Get(loggedInUserkey);

            if (loggedInUserCache != null)
            {
                cache.Remove(loggedInUserkey);
            }

            object tokenCache = cache.Get(token);

            if (tokenCache != null)
            {
                cache.Remove(token);
            }
        }
    }
}
