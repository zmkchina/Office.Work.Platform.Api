using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class UserController : ControllerBase
    {
        private readonly UserRepository _UserRepository;
        public UserController(GHDbContext ghDbContet)
        {
            _UserRepository = new UserRepository(ghDbContet);
        }
        /// <summary>
        /// 读取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _UserRepository.GetAllAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据ID读取单个用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<User> GetAsync(string Id)
        {
           
            return await _UserRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostAsync([FromBody]User PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _UserRepository.AddAsync(PEntity).ConfigureAwait(false) > 0)
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
        /// 更新记录
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> PutAsync([FromBody]User PEntity)
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
