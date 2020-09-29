using System.Collections.Generic;
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
    public class MemberPayItemController : ControllerBase
    {
        private readonly MemberPayItemRepository _PayRepository;

        public MemberPayItemController(GHDbContext ghDbContet,IMapper mapper)
        {
            _PayRepository = new MemberPayItemRepository(ghDbContet, mapper);
        }
        /// <summary>
        /// 返回所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<MemberPayItemDto>>> GetAsync()
        {
            return await _PayRepository.GetAllAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 按主键查询记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Name}")]
        public async Task<ActionResult<MemberPayItemDto>> GetAsync(string Name)
        {
            return await _PayRepository.GetOneByIdAsync(Name).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<ActionResult<List<MemberPayItemDto>>> GetAsync([FromQuery] MemberPayItemSearch SearchCondition)
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
        public async Task<ActionResult<string>> PostAsync([FromBody]MemberPayItemEntity PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PayRepository.AddAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(p_state: 0, p_msg: "保存成功");
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        [HttpPut]
        public async Task<ActionResult<string>> PutAsync([FromBody]MemberPayItemEntity PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PayRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "更新成功");
            }
            else
            {
                actResult.SetValues(1, "更新失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        [HttpDelete]
        public async Task<ActionResult<string>> DeleteAsync(string Name)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PayRepository.DeleteAsync(Name).ConfigureAwait(false) > 0)
            {
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
