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
    public class MemberRelationsController : ControllerBase
    {
        private readonly MemberRelationsRepository _DataRepository;

        public MemberRelationsController(GHDbContext ghDbContet)
        {
            _DataRepository = new MemberRelationsRepository(ghDbContet);
        }
        /// <summary>
        /// 返回所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<MemberRelations>> GetAsync()
        {
            return await _DataRepository.GetAllAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 查询指定编号的记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberRelations> GetAsync(string Id)
        {
            return await _DataRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<MemberRelations>> GetAsync([FromQuery]MemberRelationsSearch SearchCondition)
        {
            return await _DataRepository.GetEntitiesAsync(SearchCondition).ConfigureAwait(false);
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromBody]MemberRelations PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _DataRepository.AddAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(p_state: 0, p_msg: "保存成功", p_tag: PEntity?.Id);
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
        public async Task<string> PutAsync([FromBody]MemberRelations PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _DataRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "更新成功");
            }
            else
            {
                actResult.SetValues(1, "更新失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<string> DeleteAsync(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _DataRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
