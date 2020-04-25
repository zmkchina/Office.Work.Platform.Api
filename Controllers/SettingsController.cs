using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsRepository _DataSettingsRepository;

        public SettingsController(GHDbContext ghDbContext, ILogger<User> logger)
        {
            _DataSettingsRepository = new SettingsRepository(ghDbContext);
        }

        [HttpGet]
        public async Task<SettingServer> GetAsync()
        {
            return await _DataSettingsRepository.ReadAsync().ConfigureAwait(false); 
        }

        [HttpPut]
        public async Task<string> PutAsync([FromForm]SettingServer Entity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (Entity != null)
            {
                if (await _DataSettingsRepository.UpdateAsync(Entity).ConfigureAwait(false) > 0)
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
