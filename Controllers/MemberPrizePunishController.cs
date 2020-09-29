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
    public class MemberPrizePunishController : ControllerBase
    {
        private readonly MemberPrizePunishRepository _DataRepository;

        public MemberPrizePunishController(GHDbContext ghDbContet,IMapper mapper)
        {
            _DataRepository = new MemberPrizePunishRepository(ghDbContet, mapper);
        }

        ///// <summary>
        ///// 查询指定编号的记录
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpGet("ReadEntity")]
        //[Route("{Id}")]
        //public async Task<MemberPrizePunishEntity> ReadEntity(string Id)
        //{
        //    return await _DataRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        //}

        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
      
        [HttpGet("ReadDtos")]
        public async Task<ActionResult<List<Lib.MemberPrizePunishDto>>> ReadDtos<MemberPrizePunishDto, MemberPrizePunishSearch>([FromQuery] Lib.MemberPrizePunishSearch SearchCondition)
        {
            return await _DataRepository.GetEntitiesAsync(SearchCondition).ConfigureAwait(false);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPost("AddEntity")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<string>> PostAsync([FromBody]MemberPrizePunishEntity PEntity)
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
        [HttpPut("UpdateEntity")]
        public async Task<ActionResult<string>> PutAsync([FromBody]MemberPrizePunishEntity PEntity)
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
        [Route("DeleteEntity/{Id}")]
        public async Task<ActionResult<string>> DeleteAsync(string Id)
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
