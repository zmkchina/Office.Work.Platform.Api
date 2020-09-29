using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly string _FileBaseDir;
        private readonly PlanRepository _PlanRepository;
        private readonly PlanFileRepository _FileRepository;

        public PlanController(IConfiguration configuration, GHDbContext ghDbContext, IMapper mapper)
        {
            _PlanRepository = new PlanRepository(ghDbContext, mapper);
            _FileRepository = new PlanFileRepository(ghDbContext, mapper);
            if (configuration != null)
            {
                _FileBaseDir = Path.Combine(configuration["StaticFileDir"], "PlanFiles");
            }
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult<PlanInfoDto>> GetAsync(string Id)
        {
            var EntityDto= await _PlanRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
            return EntityDto;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<ActionResult<PlanInfoDtoPages>> GetAsync([FromQuery] PlanInfoSearch mSearchPlan)
        {
            return await _PlanRepository.GetEntitiesAsync(mSearchPlan).ConfigureAwait(false);
        }

        /// <summary>
        /// 新增计划，如相同编号的计划已经存在，则返回-2
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<string>> PostAsync([FromBody]PlanEntity Entity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (await _PlanRepository.AddAsync(Entity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(p_state: 0, p_msg: "保存成功", p_tag: Entity?.Id);
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        [HttpPut]
        public async Task<ActionResult<string>> PutAsync([FromBody]PlanEntity PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (PEntity != null && !PEntity.CurrectState.Equals(PlanStatus.WaitBegin, StringComparison.Ordinal))
            {
                PEntity.FinishDate = DateTime.Now;
            }
            if (await _PlanRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "更新成功");
            }
            else
            {
                actResult.SetValues(1, "更新失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        //删除指定的计划信息
        [HttpDelete("{Id}")]
        public async Task<ActionResult<string>> DeleteAsync(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PlanRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
            {
                await _FileRepository.DeleteByOwnerIdAsync(_FileBaseDir, Id).ConfigureAwait(false);
                actResult.SetValues(0, "删除成功");
            }
            else
            {
                actResult.SetValues(1, "删除失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
    }
}
