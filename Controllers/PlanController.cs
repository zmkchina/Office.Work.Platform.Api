using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly PlanRepository _PlanRepository;
        private readonly IConfiguration _configuration;

        public PlanController(IConfiguration configuration, GHDbContext ghDbContet, ILogger<User> logger)
        {
            _PlanRepository = new PlanRepository(ghDbContet);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<Plan>> GetAsync()
        {
            return await _PlanRepository.GetAllAsync().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<Plan> GetAsync(string Id)
        {
            return await _PlanRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<Plan>> GetAsync([FromQuery]PlanSearch mSearchPlan)
        {
            return await _PlanRepository.GetEntitiesAsync(mSearchPlan).ConfigureAwait(false);
        }

        /// <summary>
        /// 新增或更新计划
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromForm]Plan Entity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (Entity != null)
            {
                
                if (await _PlanRepository.AddNewAsync(Entity).ConfigureAwait(false) > 0)
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
        public async Task<string> PutAsync([FromForm]Plan Entity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (Entity != null)
            {
                if (!Entity.CurrectState.Equals(PlanStatus.WaitBegin, StringComparison.Ordinal))
                {
                    Entity.FinishDate = DateTime.Now;
                }
                if (await _PlanRepository.UpdateAsync(Entity).ConfigureAwait(false) > 0)
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
        //删除指定的计划信息
        [HttpDelete("{Id}")]
        public async Task<string> DeleteAsync(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (!string.IsNullOrEmpty(Id))
            {
                if (await _PlanRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
