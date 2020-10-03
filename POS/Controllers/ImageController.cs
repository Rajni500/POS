using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;

namespace POS.Controllers
{
    [EnableCors("*","*","*")]
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return "value";
        }
    }
}
