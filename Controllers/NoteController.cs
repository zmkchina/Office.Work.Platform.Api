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
    public class NoteController : ControllerBase
    {
        private readonly NoteRepository _NoteRepository;
        public NoteController(GHDbContext ghDbContet, ILogger<Note> logger)
        {
            _NoteRepository = new NoteRepository(ghDbContet);
        }
        /// <summary>
        /// 读取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Note>> GetAsync()
        {
            return await _NoteRepository.GetAllAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据ID读取记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<Note> GetAsync(string Id)
        {
            return await _NoteRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> PostAsync(Note newNode)
        {
            return await _NoteRepository.AddNew(newNode).ConfigureAwait(false);
        }
    }
}
