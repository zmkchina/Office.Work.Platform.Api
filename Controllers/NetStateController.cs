using System;
using Microsoft.AspNetCore.Mvc;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class NetStateController : ControllerBase
    {
        //private readonly SettingsRepository _DataSettingsRepository;

        public NetStateController() //GHDbContext ghDbContext)
        {
            //_DataSettingsRepository = new SettingsRepository(ghDbContext);
        }

        [HttpGet("DateTime")]
        public ActionResult<NetStateDto> GetTime()
        {

            return new NetStateDto() { State = 200, ServerTime = DateTime.Now };
        }
    }
}
