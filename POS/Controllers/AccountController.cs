using Microsoft.AspNetCore.Mvc;
using POS.Contract;
using POS.Controllers.Core;
using POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Controllers
{
    [Route("Account/")]
    public class AccountController : GenericController<User>
    {
        public AccountController(IAccountRepository repository, IServiceProvider serviceProvider)
        {
            base.genericRepository = repository;
            base.serviceProvider = serviceProvider;
        }
    }
}
