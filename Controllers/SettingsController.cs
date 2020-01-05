using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly DataSettingsRepository _DataSettingsRepository;

        public SettingsController(GHDbContext ghDbContext, ILogger<ModelUser> logger)
        {
            _DataSettingsRepository = new DataSettingsRepository(ghDbContext);
        }

        [HttpGet]
        public async Task<ModelSettingServer> GetAsync()
        {
            return await _DataSettingsRepository.GetOneByIdAsync(0); 
        }

        [HttpPut]
        public async Task<string> PutAsync([FromForm]ModelSettingServer P_Entity)
        {
            ModelResult actResult = new ModelResult();
            if (P_Entity != null)
            {
                if (await _DataSettingsRepository.UpdateAsync(P_Entity) > 0)
                {
                    actResult.SetValues(0, "设置成功");
                }
                else
                {
                    actResult.SetValues(1, "设置失败");
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }
    }
}
