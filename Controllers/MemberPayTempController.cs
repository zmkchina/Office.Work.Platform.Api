using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class MemberPayTempController : ControllerBase
    {
        private readonly MemberPlayTempRepository _PayTempRepository;
        private readonly IConfiguration _configuration;

        public MemberPayTempController(IConfiguration configuration, GHDbContext ghDbContet, ILogger<User> logger)
        {
            _PayTempRepository = new MemberPlayTempRepository(ghDbContet);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<MemberPayTemp>> GetAsync()
        {
            return await _PayTempRepository.GetAllAsync().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberPayTemp> GetAsync(string Id)
        {
            return await _PayTempRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<MemberPayTemp>> GetAsync([FromQuery]MemberSearch mSearchPlan)
        {
            return await _PayTempRepository.GetEntitiesAsync(mSearchPlan).ConfigureAwait(false);
        }

        /// <summary>
        /// 新增或更新计划
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromForm]MemberPayTemp EntityInfo)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (EntityInfo != null)
            {
                if (await _PayTempRepository.AddOrUpdateAsync(EntityInfo).ConfigureAwait(false) > 0)
                {
                    actResult.SetValues(0, "保存成功");
                }
                else
                {
                    actResult.SetValues(1, "保存失败");
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }
        [HttpPut]
        public async Task<string> PutAsync([FromForm]MemberPayTemp Entity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (Entity != null)
            {
                if (await _PayTempRepository.UpdateAsync(Entity).ConfigureAwait(false) > 0)
                {
                    actResult.SetValues(0, "更新成功");
                }
                else
                {
                    actResult.SetValues(1, "更新失败");
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }
        [HttpDelete]
        public async Task<string> DeleteAsync(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (!string.IsNullOrEmpty(Id))
            {
                if (await _PayTempRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
                {
                    actResult.SetValues(0, "删除成功");
                }
                else
                {
                    actResult.SetValues(1, "删除失败");
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }
    }
}
