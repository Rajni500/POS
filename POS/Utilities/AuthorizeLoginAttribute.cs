using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using POS.Models;
using POS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Utilities
{
    /// <summary>
    /// Class Authorize Login Attribute .
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AuthorizeLoginAttribute : Attribute, IAuthorizationFilter
    {
        private Role role;
        public AuthorizeLoginAttribute()
        {
            role = Role.None;
        }

        public AuthorizeLoginAttribute( Role userRole)
        {
            role = userRole;
        }

        /// <summary>
        /// On authorization event handler.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ICache cache = (ICache)context.HttpContext.RequestServices.GetService(typeof(ICache));
            var request = context.HttpContext.Request;
            string headerToken = null;

            if (request.Headers.ContainsKey("Authorization"))
            {
                string bearerToken = context.HttpContext.Request.Headers["Authorization"];
                headerToken = bearerToken.Split(' ')[1];
            }
            else
            {
                headerToken = request.Form["__RequestVerificationToken"];
            }

            var token = cache.Get(headerToken);

            if (token == null)
            {
                TokenExpired(context);

            }

            if(role != Role.None)
            {
                VerifyUserRole(context, cache, headerToken);
            }
        }

        private void VerifyUserRole(AuthorizationFilterContext context, ICache cache, string headerToken)
        {
            string loggedInUserkey = string.Format("{0}_{1}", headerToken, Constants.LoggedInUser);

            User profileObject = (User)cache.Get(loggedInUserkey);

            if (profileObject != null)
            {
                if (profileObject.RoleId != role)
                {
                    UnAuthorizedAccess(context);
                }
            }
            else
            {
                TokenExpired(context);
            }
        }

        private static void UnAuthorizedAccess(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult("")
            {
                StatusCode = 400,
                ContentType = "application/json",
                Value = new
                {
                    success = false,
                    error = "User does not have rights to perform this action"
                }
            };
        }

        private static void TokenExpired(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult("")
            {
                StatusCode = 401,
                ContentType = "application/json",
                Value = new
                {
                    success = false,
                    error = "token expired"
                }
            };
        }
    }
}