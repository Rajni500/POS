using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
                //Returns result that contains error details
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
}