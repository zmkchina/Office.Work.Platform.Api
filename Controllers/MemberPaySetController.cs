using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class MemberPaySetController : ControllerBase
    {
        private readonly MemberPaySetRepository _PaySetRepository;

        public MemberPaySetController(GHDbContext ghDbContet)
        {
            _PaySetRepository = new MemberPaySetRepository(ghDbContet);
        }
        /// <summary>
        /// 返回所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<MemberPaySet>> GetAsync()
        {
            return await _PaySetRepository.GetAllAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 查询指定编号的记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberPaySet> GetAsync(string Id)
        {
            return await _PaySetRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<MemberPaySet>> GetAsync([FromQuery]MemberPaySetSearch SearchCondition)
        {
            return await _PaySetRepository.GetEntitiesAsync(SearchCondition).ConfigureAwait(false);
        }

        /// <summary>
        /// 批量新增或者更新数据信息（如数据库中没有则新增之，如有则更新之）
        /// </summary>
        /// <param name="PaySetList"></param>
        /// <returns></returns>
        [HttpPost("AddOrUpdate")]
        [DisableRequestSizeLimit]
        public async Task<string> PostAddOrUpdateAsync([FromBody]List<MemberPaySet> PaySetList)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PaySetRepository.AddOrUpdateAsync(PaySetList).ConfigureAwait(false) > 0)
            {

                actResult.SetValues(p_state: 0, p_msg: "保存成功");
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromBody]MemberPaySet PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PaySetRepository.AddAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(p_state: 0, p_msg: "保存成功");
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> PutAsync([FromBody]MemberPaySet PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PaySetRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
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
        public async Task<string> DeleteAsync(string MemberId)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PaySetRepository.DeleteAsync(MemberId).ConfigureAwait(false) > 0)
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
