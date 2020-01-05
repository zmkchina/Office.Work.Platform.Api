using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;
using System.IO;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class FileDownController : ControllerBase
    {
        private readonly DataFileRepository _FileRepository;
        private readonly IConfiguration _configuration;
        public FileDownController(IConfiguration configuration, GHDbContext ghDbContext, ILogger<ModelUser> logger)
        {
            _FileRepository = new DataFileRepository(ghDbContext);
            _configuration = configuration;
        }
        /// <summary>
        /// 根据文件的Id号下载一个文件。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult> Get(string Id)
        {
            ModelFile TheFile = await _FileRepository.GetOneByIdAsync(Id);
            FileStream downFileStream = null;
            string FileName = null;
            if (TheFile != null)
            {
                FileName = $"{TheFile.Name}({TheFile.Id}){TheFile.ExtendName}";
                string fileFullName = _configuration["StaticFileDir"] + $"\\{TheFile.Id}{TheFile.ExtendName}";
                if (System.IO.File.Exists(fileFullName))
                {
                    downFileStream = new FileStream(fileFullName, FileMode.Open);
                }
            }
            if (downFileStream == null)
            {
                return NotFound();
            }
            else
            {
                return File(downFileStream, "application/octet-stream", FileName);
            }
        }
    }
}
