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
    public class UserController : ControllerBase
    {
        private readonly UserRepository _UserRepository;
        public UserController(GHDbContext ghDbContet, IMapper mapper)
        {
            _UserRepository = new UserRepository(ghDbContet, mapper);
        }
        /// <summary>
        /// 读取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lib.UserDto>>> GetAsync()
        {
            var Dtos = await _UserRepository.GetAllAsync().ConfigureAwait(false);
            return Ok(Dtos);
        }
        /// <summary>
        /// 根据ID读取单个用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<Lib.UserDto>> GetAsync(string Id)
        {

            var Dto = await _UserRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
            return Ok(Dto);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> PostAsync([FromBody] UserEntity PEntity)
        {
            if (await _UserRepository.AddAsync(PEntity).ConfigureAwait(false) > 0)
            {
                return Ok(PEntity);
            }
            else
            {
                return  Ok(PEntity);
            }
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> PutAsync([FromBody] UserEntity PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (await _UserRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "更新成功");
            }
            else
            {
                actResult.SetValues(1, "更新失败");
            }
            return JsonConvert.SerializeObject(actResult);
        }
        //删除指定信息
        [HttpDelete("{Id}")]
        public async Task<string> DeleteAsync(string Id)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _UserRepository.DeleteAsync(Id).ConfigureAwait(false) > 0)
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
