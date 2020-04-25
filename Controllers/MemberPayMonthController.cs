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
    public class MemberPayMonthController : ControllerBase
    {
        private readonly MemberPlayMonthRepository _PayMonthRepository;
        private readonly IConfiguration _configuration;

        public MemberPayMonthController(IConfiguration configuration, GHDbContext ghDbContet, ILogger<User> logger)
        {
            _PayMonthRepository = new MemberPlayMonthRepository(ghDbContet);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<MemberPayMonth>> GetAsync()
        {
            return await _PayMonthRepository.GetAllAsync().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberPayMonth> GetAsync(string Id)
        {
            return await _PayMonthRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<MemberPayMonth>> GetAsync([FromQuery]MemberSearch mSearchPlan)
        {
            return await _PayMonthRepository.GetEntitiesAsync(mSearchPlan).ConfigureAwait(false);
        }

        /// <summary>
        /// 新增或更新计划
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromForm]MemberPayMonth EntityInfo)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (EntityInfo != null)
            {
                if (await _PayMonthRepository.AddOrUpdateAsync(EntityInfo).ConfigureAwait(false) > 0)
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
        public async Task<string> PutAsync([FromForm]MemberPayMonth Entity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (Entity != null)
            {
                if (await _PayMonthRepository.UpdateAsync(Entity).ConfigureAwait(false) > 0)
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
                if (await _PayMonthRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
