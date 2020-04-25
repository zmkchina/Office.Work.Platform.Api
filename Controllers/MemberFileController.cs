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
    public class MemberFileController : ControllerBase
    {
        private readonly MemberFileRepository _FileRepository;
        private readonly IConfiguration _configuration;
        public MemberFileController(IConfiguration configuration, GHDbContext ghDbContext, ILogger<User> logger)
        {
            _FileRepository = new MemberFileRepository(ghDbContext);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<MemberFile>> GetAsync()
        {
            return await _FileRepository.GetAllAsync().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberFile> GetAsync(string Id)
        {
            return await _FileRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 上传一个文件，包括文件的相关描述
        /// </summary>
        /// <param name="FileInfo">上传的文件描述信息</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> PostAsync([FromForm]MemberFile FileInfo)
        {
            ExcuteResult actResult = new ExcuteResult();
           
            if (Request.Form.Files.Count > 0 && FileInfo != null)
            {
                try
                {
                    var fileName = _configuration["StaticFileDir"] + $"\\{FileInfo.Id}{FileInfo.ExtendName}";
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        await Request.Form.Files[0].CopyToAsync(fs).ConfigureAwait(false);
                        fs.Flush();
                    }
                    if (System.IO.File.Exists(fileName))
                    {
                        await _FileRepository.AddAsync(FileInfo).ConfigureAwait(false);
                    }
                    actResult.SetValues(0, "上传成功", @"GHStaticFiles/" + FileInfo.Id + FileInfo.ExtendName);
                }
                catch (System.Exception err)
                {
                    actResult.SetValues(1, err.Message);
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }

        /// <summary>
        /// 更新文件的描述信息
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> Put([FromForm]MemberFile FileInfo)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (FileInfo != null)
            {
                if (await _FileRepository.UpdateAsync(FileInfo).ConfigureAwait(false) > 0)
                {
                    actResult.SetValues(0, "更新成功!", @"GHStaticFiles/" + FileInfo.Id + FileInfo.ExtendName);
                }
                else
                {
                    actResult.SetValues(1, "更新失败!");
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
                    var fileName = _configuration["StaticFileDir"] + $"\\{FileId}{FileExtName}";
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
        /// 下载文件。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult> GetAsync(MemberFile DownFileInfo)
        {
            if (DownFileInfo != null)
            {
                string FileName = $"{DownFileInfo.Name}({DownFileInfo.Id}){DownFileInfo.ExtendName}";
                FileStream downFileStream = null;

                await Task.Run(() =>
                {
                    string OwnerPath = "MemberFiles";
                    if (DownFileInfo != null)
                    {
                        string fileFullName = _configuration["StaticFileDir"] + $"\\{OwnerPath}\\{DownFileInfo.Id}{DownFileInfo.ExtendName}";
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
    }
}
