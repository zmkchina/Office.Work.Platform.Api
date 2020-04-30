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
    public class MemberPayMonthUnofficialController : ControllerBase
    {
        private readonly MemberPayMonthUnofficialRepository _PayRepository;
        private readonly IConfiguration _configuration;

        public MemberPayMonthUnofficialController(IConfiguration configuration, GHDbContext ghDbContet, ILogger<User> logger)
        {
            _PayRepository = new MemberPayMonthUnofficialRepository(ghDbContet);
            _configuration = configuration;
        }
        /// <summary>
        /// 返回所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<MemberPayMonthUnofficial>> GetAsync()
        {
            return await _PayRepository.GetAllAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 查询指定编号的记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberPayMonthUnofficial> GetAsync(string Id)
        {
            return await _PayRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<MemberPayMonthUnofficial>> GetAsync([FromQuery]MemberPayMonthUnofficialSearch SearchCondition)
        {
            return await _PayRepository.GetEntitiesAsync(SearchCondition).ConfigureAwait(false);
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromForm]MemberPayMonthUnofficial EntityInfo)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (EntityInfo != null)
            {
                if (await _PayRepository.AddAsync(EntityInfo).ConfigureAwait(false) > 0)
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
        public async Task<string> PutAsync([FromForm]MemberPayMonthUnofficial Entity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (Entity != null)
            {
                if (await _PayRepository.UpdateAsync(Entity).ConfigureAwait(false) > 0)
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
                if (await _PayRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
