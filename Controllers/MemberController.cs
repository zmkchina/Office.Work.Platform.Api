﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class MemberController : ControllerBase
    {
        private readonly string _FileBaseDir;
        private readonly MemberFileRepository _FileRepository;
        private readonly MemberRepository _MemberRepository;
        public MemberController(IConfiguration configuration, GHDbContext ghDbContext)
        {
            _MemberRepository = new MemberRepository(ghDbContext);
            _FileRepository = new MemberFileRepository(ghDbContext);
            if (configuration != null)
            {
                _FileBaseDir = Path.Combine(configuration["StaticFileDir"], "MemberFiles");
            }
        }
        [HttpGet]
        public async Task<IEnumerable<Member>> GetAsync()
        {
            return await _MemberRepository.GetAllAsync().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<Member> GetAsync(string Id)
        {
            return await _MemberRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询记录。
        /// </summary>
        /// <param name="mSearchMember"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<Member>> GetMemberByConditionAsync([FromQuery]MemberSearch mSearchMember)
        {
            return await _MemberRepository.GetEntitiesAsync(mSearchMember).ConfigureAwait(false);
        }
        /// <summary>
        /// 新增一个记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostAsync([FromBody]Member PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (await _MemberRepository.AddAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "保存成功");
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
 
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> Put([FromBody]Member PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (await _MemberRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
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
        public async Task<string> PostAddOrUpdateAsync([FromBody] Member PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (await _MemberRepository.AddOrUpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "保存成功");
            }
            else
            {
                actResult.SetValues(1, "保存失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<string> Delete(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _MemberRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
