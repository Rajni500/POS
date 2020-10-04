using Microsoft.AspNetCore.Mvc;
using POS.Contract;
using POS.Controllers.Core;
using POS.Models;
using POS.Models.Enums;
using POS.Utilities;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Controllers
{
    [Route("Category/")]
    public class CategoryController : GenericController<Category>
    {
        public CategoryController(ICategoryRepository repository, IServiceProvider serviceProvider, ICache cache)
            : base(repository, cache)
        {
            base.genericRepository = repository;
            base.serviceProvider = serviceProvider;
        }

        [HttpPost]
        [AuthorizeLogin(userRole: Role.Admin)]
        [Route("Add")]
        public override ActionResult Add([FromBody] Category entity)
        {
            return base.Add(entity);
        }

        [HttpPost]
        [AuthorizeLogin(userRole: Role.Admin)]
        [Route("Edit")]
        public override ActionResult Edit([FromBody] Category entity, bool hasMap = false)
        {
            return base.Edit(entity, hasMap);
        }

        [HttpPost]
        [AuthorizeLogin(userRole: Role.Admin)]
        [Route("Delete")]
        public override ActionResult Delete([FromBody] SearchViewModel search)
        {
            return base.Delete(search);
        }
    }
}
