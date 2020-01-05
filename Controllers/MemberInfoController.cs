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
    public class MemberInfoController : ControllerBase
    {
        private readonly DataMemberRepository _MemberRepository;

        public MemberInfoController(GHDbContext ghDbContext, ILogger<ModelUser> logger)
        {
            _MemberRepository = new DataMemberRepository(ghDbContext);
        }

        [HttpGet]
        public async Task<IEnumerable<ModelMember>> GetAsync()
        {
            return await _MemberRepository.GetAllAsync();
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ModelMember> GetAsync(string Id)
        {
            return await _MemberRepository.GetOneByIdAsync(Id);
        }

        [HttpGet("Search")]
        public async Task<IEnumerable<ModelMember>> GetPlanByConditionAsync([FromQuery]MSearchMember mSearchMember)
        {
            return await _MemberRepository.GetEntitiesAsync(mSearchMember);
        }

        [HttpPost]
        public async Task<string> PostAsync([FromForm]ModelMember P_Entity)
        {
            ModelResult actResult = new ModelResult();

            if (P_Entity != null)
            {
                if (await _MemberRepository.AddOrUpdateAsync(P_Entity) > 0)
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
        public async Task<string> PostAsync([FromBody]List<ModelMember> P_Entitys)
        {
            ModelResult actResult = new ModelResult();

            if (P_Entitys != null && P_Entitys.Count > 0)
            {
                try
                {
                    if (await _MemberRepository.AddRangeAsync(P_Entitys) > 0)
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
        public async Task<string> Put([FromForm]ModelMember P_Entity)
        {
            ModelResult actResult = new ModelResult();

            if (P_Entity != null)
            {
                if (await _MemberRepository.UpdateAsync(P_Entity) > 0)
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
        public async Task<string> Delete(string P_Id)
        {
            ModelResult actResult = new ModelResult();
            if (P_Id != null)
            {
                if (await _MemberRepository.DeleteAsync(P_Id) > 0)
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
