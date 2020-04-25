using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class PlanFileController : ControllerBase
    {
        private readonly PlanFileRepository _FileRepository;
        private readonly IConfiguration _configuration;
        public PlanFileController(IConfiguration configuration, GHDbContext ghDbContext, ILogger<User> logger)
        {
            _FileRepository = new PlanFileRepository(ghDbContext);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<PlanFile>> GetAsync()
        {
            return await _FileRepository.GetAllAsync().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<PlanFile> GetAsync(string Id)
        {
            return await _FileRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 用指定的条件类查询文件。
        /// </summary>
        /// <param name="mSearchFile"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<PlanFile>> GetPlanFilesAsync([FromQuery]PlanFileSearch mSearchFile)
        {
            return await _FileRepository.GetEntitiesAsync(mSearchFile).ConfigureAwait(false);
        }



        /// <summary>
        /// 更新文件的描述信息
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> Put([FromForm]PlanFile FileInfo)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (FileInfo != null)
            {
                if (await _FileRepository.UpdateAsync(FileInfo).ConfigureAwait(false) > 0)
                {
                    actResult.SetValues(0, "文件信息更新成功!");
                }
                else
                {
                    actResult.SetValues(1, "文件信息更新失败!");
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }


        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="P_FileId"></param>
        /// <param name="P_FileExtName"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<string> Delete(string FileId, string FileExtName)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (FileId != null)
            {
                if (await _FileRepository.DeleteAsync(FileId).ConfigureAwait(false) > 0)
                {
                    var fileName = _configuration["StaticFileDir"] + $"\\PlanFiles\\{FileId}{FileExtName}";
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    actResult.SetValues(0, "删除成功!");
                }
                else
                {
                    actResult.SetValues(1, "删除失败!");
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }
        /// <summary>
        /// 上传一个文件，包括文件的相关描述
        /// </summary>
        /// <param name="FileInfo">上传的文件描述信息</param>
        /// <returns></returns>
        [HttpPost("UpLoadFile")]
        [DisableRequestSizeLimit]
        public async Task<string> PostUpLoadFileAsync([FromForm]PlanFile pfile)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (Request.Form.Files.Count > 0 && pfile != null)
            {
                try
                {
                    var fileName = _configuration["StaticFileDir"] + $"\\PlanFiles\\{pfile.Id}{pfile.ExtendName}";
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        await Request.Form.Files[0].CopyToAsync(fs).ConfigureAwait(false);
                        fs.Flush();
                    }
                    if (System.IO.File.Exists(fileName))
                    {
                        await _FileRepository.AddAsync(pfile).ConfigureAwait(false);
                    }
                    actResult.SetValues(0, "上传成功");
                }
                catch (System.Exception err)
                {
                    actResult.SetValues(1, err.Message);
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }
        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoadFile/{FileId}")]
        public async Task<ActionResult> GetDownLoadFileAsync(string FileId)
        {
            PlanFile FileInfo = await _FileRepository.GetOneByIdAsync(FileId).ConfigureAwait(false);
            if (FileInfo != null)
            {
                string FileName = $"{FileInfo.Name}({FileInfo.Id}){FileInfo.ExtendName}";
                FileStream downFileStream = null;

                await Task.Run(() =>
                {
                    if (FileInfo != null)
                    {
                        string fileFullName = Path.Combine(_configuration["StaticFileDir"], "PlanFiles", $"{FileInfo.Id}{FileInfo.ExtendName}");
                        if (System.IO.File.Exists(fileFullName))
                        {
                            downFileStream = new FileStream(fileFullName, FileMode.Open);
                        }
                    }
                }).ConfigureAwait(false);

                if (downFileStream != null)
                {
                    return File(downFileStream, "application/octet-stream", FileName);
                }
            }
            return NotFound();
        }
        public IActionResult BannerImage()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(),
                                    "MyStaticFiles", "images", "banner1.svg");

            return PhysicalFile(file, "image/svg+xml");
        }
    }
}
