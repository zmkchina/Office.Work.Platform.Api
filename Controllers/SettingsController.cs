using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public SettingsController(GHDbContext ghDbContext, IMapper mapper)
        {
            _DataSettingsRepository = new SettingsRepository(ghDbContext, mapper);
        }

        [HttpGet]
        public async Task<SettingServerDto> GetAsync()
        {
            return await _DataSettingsRepository.ReadAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> PutAsync([FromBody]SettingServerEntity PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _DataSettingsRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "设置成功");
            }
            else
            {
                actResult.SetValues(1, "设置失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
    }
}
