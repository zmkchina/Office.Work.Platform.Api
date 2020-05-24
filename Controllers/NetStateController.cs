using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class NetStateController : ControllerBase
    {
        //private readonly SettingsRepository _DataSettingsRepository;

        public NetStateController() //GHDbContext ghDbContext)
        {
            //_DataSettingsRepository = new SettingsRepository(ghDbContext);
        }

        [HttpGet("GetTime")]
        public NetState GetTime()
        {

            return new NetState() { State = 200, ServerTime = DateTime.Now };
        }
    }
}
