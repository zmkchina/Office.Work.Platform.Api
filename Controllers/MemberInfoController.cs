using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    /// <summary>
    /// 控制器：用于管理本单位职工信息。
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class MemberInfoController : ControllerBase
    {
        private readonly string _FileBaseDir;
        private readonly MemberFileRepository _FileRepository;
        private readonly MemberInfoRepository _MemberRepository;
        public MemberInfoController(IConfiguration configuration, GHDbContext ghDbContext, IMapper mapper)
        {
            _MemberRepository = new MemberInfoRepository(ghDbContext, mapper);
            _FileRepository = new MemberFileRepository(ghDbContext, mapper);
            if (configuration != null)
            {
                _FileBaseDir = Path.Combine(configuration["StaticFileDir"], "MemberFiles");
            }
        }

        /// <summary>
        /// 根据Id查询单个职工信息。
        /// </summary>
        /// <param name="Id">职工的Id号</param>
        /// <returns></returns>
        [HttpGet("Entity/{Id}")]
        public async Task<ActionResult<MemberInfoEntity>> ReadEntity(string Id)
        {
            return await _MemberRepository.GetMemberInfoEntityAsync(Id).ConfigureAwait(false);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<MemberInfoDto>> ReadDto(string Id)
        {
            return await _MemberRepository.GetMemberInfoDtoAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询记录。
        /// </summary>
        /// <param name="mSearchMember"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberInfoDto>>> ReadDtos([FromQuery] MemberInfoSearch mSearchMember)
        {
            IEnumerable<MemberInfoDto> Dtos = await _MemberRepository.GetEntitiesAsync(mSearchMember).ConfigureAwait(false);
            return Ok(Dtos);
        }
        /// <summary>
        /// 新增一个记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> AddEntity([FromBody] MemberInfoEntity PEntity)
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
        public async Task<string> UpdateEntity([FromBody] MemberInfoEntity PEntity)
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
        public async Task<string> AddOrUpdateEntity([FromBody] MemberInfoEntity PEntity)
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
        [HttpDelete("{Id}")]
        public async Task<string> DeleteEntity(string Id)
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
