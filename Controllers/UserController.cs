using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly DataUserRepository _UserRepository;
        public UserController(GHDbContext ghDbContet, ILogger<ModelUser> logger)
        {
            _UserRepository = new DataUserRepository(ghDbContet);
        }
        /// <summary>
        /// 读取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ModelUser>> GetAsync()
        {
            return await _UserRepository.GetAllAsync();
        }
        /// <summary>
        /// 根据ID读取单个用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<ModelUser> GetAsync(string Id)
        {
            return await _UserRepository.GetOneByIdAsync(Id);
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> PostAsync(ModelUser userModel)
        {
            return await _UserRepository.AddNew(userModel);
        }
    }
}
