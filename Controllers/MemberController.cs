using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class MemberController : ControllerBase
    {
        private readonly MemberRepository _MemberRepository;

        public MemberController(GHDbContext ghDbContext, ILogger<User> logger)
        {
            _MemberRepository = new MemberRepository(ghDbContext);
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

        [HttpGet("Search")]
        public async Task<IEnumerable<Member>> GetMemberByConditionAsync([FromQuery]MemberSearch mSearchMember)
        {
            return await _MemberRepository.GetEntitiesAsync(mSearchMember).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<string> PostAsync([FromForm]Member Entity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (Entity != null)
            {
                if (await _MemberRepository.AddNewAsync(Entity).ConfigureAwait(false) > 0)
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

        [HttpPost("AddRange")]
        public async Task<string> PostAsync([FromBody]List<Member> Entitys)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (Entitys != null && Entitys.Count > 0)
            {
                try
                {
                    if (await _MemberRepository.AddRangeAsync(Entitys).ConfigureAwait(false) > 0)
                    {
                        actResult.SetValues(0, "保存成功");
                    }
                    else
                    {
                        actResult.SetValues(1, "保存失败");
                    }
                }
                catch (Exception ex)
                {
                    actResult.SetValues(2, ex.Message);
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }

        [HttpPut]
        public async Task<string> Put([FromForm]Member Entity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (Entity != null)
            {
                if (await _MemberRepository.UpdateAsync(Entity).ConfigureAwait(false) > 0)
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
        public async Task<string> Delete(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (Id != null)
            {
                if (await _MemberRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
