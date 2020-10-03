using Microsoft.AspNetCore.Mvc;
using POS.Contract;
using POS.Controllers.Core;
using POS.Models;
using POS.Utilities;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace POS.Controllers
{
    [Route("Product/")]
    public class ProductController : GenericController<Product>
    {
        private ICategoryRepository categoryRepository;

        public ProductController(IProductRepository repository, IServiceProvider serviceProvider, ICategoryRepository categoryRepository)
        {
            base.genericRepository = repository;
            base.serviceProvider = serviceProvider;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        [AuthorizeLogin]
        [Route("Add")]
        public override ActionResult Add([FromBody] Product entity)
        {
            if (entity.CategoryId == 0 && entity.Category != null)
            {
                entity.Category = categoryRepository.Add(entity.Category);
                entity.CategoryId = entity.Category.Id;
            }
            else if (entity.CategoryId == 0)
            {
                return ExceptionHandler.throwException("Select a Category for the product");
            }

            entity.Category = null;

            return base.Add(entity);
        }

        public override ActionResult GetAll([FromBody] SearchViewModel searchViewModel)
        {
            int totalRecords = 0;
            IList<Product> allRecords = new List<Product>();
            Expression<Func<Product, bool>> predicate;

            if (!String.IsNullOrEmpty(searchViewModel.Title))
            {
                predicate = a => (a.Title != null && a.Title.IndexOf(searchViewModel.Title,
                        StringComparison.CurrentCultureIgnoreCase) >= 0)
                        && (searchViewModel.CategoryId == 0 || searchViewModel.CategoryId == a.CategoryId);
            }
            else
            {
                predicate = i => searchViewModel.CategoryId == 0 || searchViewModel.CategoryId == i.CategoryId;
            }

            allRecords = genericRepository.FindBy(predicate, searchViewModel);

            totalRecords = genericRepository.GetTotalNoOfRecords(predicate, searchViewModel);

            return this.Json(new
            {
                Data = allRecords,
                TotalNoOfRecords = totalRecords
            });
        }
    }
}
