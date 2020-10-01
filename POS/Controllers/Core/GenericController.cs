using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Contract.Core;
using POS.Models;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace POS.Controllers.Core
{
    public abstract class GenericController<M> : Controller
        where M : ModelBase
    {
        protected IGenericRepository<M> genericRepository;
        protected IServiceProvider serviceProvider;

        public GenericController() { }
        public GenericController(IGenericRepository<M> genericrepository)
        {
            this.genericRepository = genericrepository;
        }

        [HttpPost]
        //[Authorize]
        [Route("Add")]
        public virtual ActionResult Add([FromBody] M entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ValidationException("Invalid input");
                }

                User profile = GetLoggedInUser();
                return Json(genericRepository.Add(entity));
            }
            catch (ValidationException ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("Delete")]
        public virtual ActionResult Delete([FromBody] SearchViewModel search)
        {
            int id = search.Id;
            try
            {
                int deletedBy = GetLoggedInUser().Id;
                genericRepository.Delete(item => item.Id == id, deletedBy);
                return Json(true);
            }
            catch (ValidationException ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("Edit")]
        public virtual ActionResult Edit([FromBody] M entity, bool hasMap = false)
        {
            try
            {
                if (entity == null)
                {
                    throw new ValidationException("Invalid input");
                }
                genericRepository.Edit(entity);
                return Json(true);
            }
            catch (ValidationException ex)
            {
                throw ex;
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("GetAll")]
        public virtual ActionResult GetAll([FromBody] SearchViewModel searchViewModel)
        {
            int totalRecords = 0;
            IList<M> allRecords = new List<M>();
            Expression<Func<M, bool>> predicate;

            if (!String.IsNullOrEmpty(searchViewModel.Title))
            {
                predicate = a => a.Title != null && a.Title.IndexOf(searchViewModel.Title,
                        StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
            else
            {
                predicate = i => true;
            }

            allRecords = genericRepository.FindBy(predicate, searchViewModel);

            totalRecords = genericRepository.GetTotalNoOfRecords(predicate, searchViewModel);

            return this.Json(new
            {
                Data = allRecords,
                TotalNoOfRecords = totalRecords
            });
        }

        //[Authorize]
        [HttpPost]
        [Route("GetById")]
        public virtual ActionResult GetById([FromBody]SearchViewModel search)
        {
            return Json(genericRepository.SingleOrDefault(item => item.Id == search.Id, search));
        }

        //[Authorize]
        [HttpPost]
        [Route("GetTotalNoOfRecords")]
        public virtual ActionResult GetTotalNoOfRecords([FromBody] SearchViewModel searchViewModel)
        {
            return Json(genericRepository.GetTotalNoOfRecords(item => true, searchViewModel));

        }

        protected User GetLoggedInUser()
        {
            var request = HttpContext.Request;

            var profile = GetLoggedInUser(request);

            return profile;
        }
        public static User GetLoggedInUser(HttpRequest request)
        {
            string headerToken = null;
            if (request.Headers.ContainsKey("Authorization"))
            {
                string bearerToken = request.Headers["Authorization"];
                headerToken = bearerToken.Split(' ')[1];
            }
            else
            {
                headerToken = request.Form["__RequestVerificationToken"];
            }

            string loggedInUserkey = string.Format("{0}_{1}", headerToken, "LoggedInUser");

            User profileObject = null; // SessionHandler.cache.Get(loggedInUserkey);

            if (profileObject != null)
            {
                return profileObject;
            }

            return new User();
        }

        //TODO Create Session Handler in shared Folder
        public class SessionHandler
        {
            //    private static ICache cacheManager;
            //    public static ICache cache => cacheManager;

            //    public SessionHandler(ICache cache)
            //    {
            //        cacheManager = cache;
            //    }
        }
    }
}
