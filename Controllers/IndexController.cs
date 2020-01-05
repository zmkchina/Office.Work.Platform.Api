using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "This is Ok";
        }
    }
}
