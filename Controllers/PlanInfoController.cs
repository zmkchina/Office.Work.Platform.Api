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
    public class PlanInfoController : ControllerBase
    {
        private readonly DataPlanRepository _PlanRepository;
        private readonly IConfiguration _configuration;

        public PlanInfoController(IConfiguration configuration, GHDbContext ghDbContet, ILogger<ModelUser> logger)
        {
            _PlanRepository = new DataPlanRepository(ghDbContet);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<ModelPlan>> GetAsync()
        {
            return await _PlanRepository.GetAllAsync().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ModelPlan> GetAsync(string Id)
        {
            return await _PlanRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<ModelPlan>> GetAsync([FromQuery]MSearchPlan mSearchPlan)
        {
            return await _PlanRepository.GetEntitiesAsync(mSearchPlan).ConfigureAwait(false);
        }


        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromForm]ModelPlan FileInfo)
        {
            ModelResult actResult = new ModelResult();
            if (FileInfo != null)
            {
                if (await _PlanRepository.AddOrUpdateAsync(FileInfo).ConfigureAwait(false) > 0)
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
        public async Task<string> PutAsync([FromForm]ModelPlan Entity)
        {
            ModelResult actResult = new ModelResult();
            if (Entity != null)
            {
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
        [HttpDelete]
        public async Task<string> DeleteAsync(string Id)
        {
            ModelResult actResult = new ModelResult();
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
