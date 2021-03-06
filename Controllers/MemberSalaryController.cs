﻿using System.Collections.Generic;
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
    public class MemberSalaryController : ControllerBase
    {
        private readonly MemberSalaryRepository _PayRepository;

        public MemberSalaryController(GHDbContext ghDbContet)
        {
            _PayRepository = new MemberSalaryRepository(ghDbContet);
        }
       
        /// <summary>
        /// 查询指定编号的记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberSalaryEntity> GetAsync(string Id)
        {
            return await _PayRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<MemberSalaryDto>> GetAsync([FromQuery] MemberSalarySearch SearchCondition)
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
        public async Task<string> PostAsync([FromBody]MemberSalaryEntity PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PayRepository.AddAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(p_state: 0, p_msg: "保存成功", p_tag: PEntity?.Id);
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        [HttpPut]
        public async Task<string> PutAsync([FromBody]MemberSalaryEntity PEntity)
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

        /// <summary>
        /// 新增或更新一个记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPost("AddOrUpdate")]
        public async Task<string> PostAddOrUpdateAsync([FromBody] MemberSalaryEntity PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (await _PayRepository.AddOrUpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(p_state: 0, p_msg: "保存成功", p_tag: PEntity?.Id);
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }

        [HttpDelete]
        public async Task<string> DeleteAsync(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _PayRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
