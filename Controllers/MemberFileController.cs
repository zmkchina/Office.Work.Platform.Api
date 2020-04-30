using System;
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
        public MemberFileController(IConfiguration configuration, GHDbContext ghDbContext, ILogger<MemberFileController> logger)
        {
            _FileRepository = new MemberFileRepository(ghDbContext);
            _configuration = configuration;
        }
        /// <summary>
        /// 获取所有文件记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<MemberFile>> GetAsync()
        {
            return await _FileRepository.GetAllAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 获取指定Id文件信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<MemberFile> GetAsync(string Id)
        {
            return await _FileRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id号从磁盘上下载文件。
        /// </summary>
        /// <param name="FileId">文件Id号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoadFile/{FileId}")]
        public async Task<ActionResult> GetDownLoadFileAsync(string FileId)
        {
            MemberFile FileInfo = await _FileRepository.GetOneByIdAsync(FileId).ConfigureAwait(false);
            if (FileInfo != null)
            {
                string FileName = $"{FileInfo.Name}({FileInfo.Id}){FileInfo.ExtendName}";
                string fileFullName = Path.Combine(_configuration["StaticFileDir"], "MemberFiles", $"{FileInfo.Id}{FileInfo.ExtendName}");
                if (System.IO.File.Exists(fileFullName))
                {
                    return PhysicalFile(fileFullName, "application/octet-stream", FileName);
                }
            }
            return NotFound();
        }
        /// <summary>
        /// 用指定的条件类查询文件信息(不下载具体文件）。
        /// </summary>
        /// <param name="mSearchFile"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<MemberFile>> GetPlanFilesAsync([FromQuery]MemberFileSearch mfSearchFile)
        {
            return await _FileRepository.GetEntitiesAsync(mfSearchFile).ConfigureAwait(false);
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
            if (FileInfo != null && FileInfo.Id != null)
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
        /// 删除一个文件信息，包括磁盘上的具体文件。
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
                    var fileName = _configuration["StaticFileDir"] + $"\\MemberFiles\\{FileId}{FileExtName}";
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
        /// 新增一个文件信息，包括将文件内容保存到磁盘上。
        /// </summary>
        /// <param name="PFile">上传的文件描述信息</param>
        /// <returns></returns>
        [HttpPost("UpLoadFile")]
        [DisableRequestSizeLimit]
        public async Task<string> PostUpLoadFileAsync([FromForm]MemberFile PFile)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (Request.Form.Files.Count > 0 && PFile != null)
            {
                try
                {
                    string FilePath = Path.Combine(_configuration["StaticFileDir"], "MemberFiles");
                    if (!System.IO.Directory.Exists(FilePath))
                    {
                        System.IO.Directory.CreateDirectory(FilePath);
                    }
                    string FileName = Path.Combine(FilePath, $"{PFile.Id}{PFile.ExtendName}");
                    using (FileStream fs = System.IO.File.Create(FileName))
                    {
                        await Request.Form.Files[0].CopyToAsync(fs).ConfigureAwait(false);
                        fs.Flush();
                    }
                    if (System.IO.File.Exists(FileName))
                    {
                        //文件写入成功后，再保存文件信息到数据表
                        PFile.UpDateTime = DateTime.Now;
                        await _FileRepository.AddAsync(PFile).ConfigureAwait(false);
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


    }
}
