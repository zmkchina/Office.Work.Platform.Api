﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly UserRepository _UserRepository;
        public UserController(GHDbContext ghDbContet, ILogger<User> logger)
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
        /// 新增用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> PostAsync(User userModel)
        {
            return await _UserRepository.AddNew(userModel).ConfigureAwait(false);
        }
    }
}
