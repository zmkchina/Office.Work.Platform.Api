using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly string _FileBaseDir;
        private readonly PlanFileRepository _FileRepository;
        public PlanFileController(IConfiguration configuration, GHDbContext ghDbContext)
        {
            _FileRepository = new PlanFileRepository(ghDbContext);
            if (configuration != null)
            {
                _FileBaseDir = Path.Combine(configuration["StaticFileDir"], "PlanFiles");
            }
        }

        /// <summary>
        /// 获取所有文件记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PlanFile>> GetAsync()
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
        public async Task<PlanFile> GetAsync(string Id)
        {
            return await _FileRepository.GetOneByIdAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 用指定的条件类查询文件信息。
        /// </summary>
        /// <param name="mSearchFile"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<PlanFileSearchResult> GetFilesAsync([FromQuery]PlanFileSearch mSearchFile)
        {
            return await _FileRepository.GetEntitiesAsync(mSearchFile).ConfigureAwait(false);
        }

        /// <summary>
        /// 更新文件的描述信息
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> PutAsync([FromBody]PlanFile PEntity)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _FileRepository.UpdateAsync(PEntity).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(p_state: 0, p_msg: "文件信息更新成功", p_tag: PEntity?.Id);
            }
            else
            {
                actResult.SetValues(1, "文件信息更新失败!");
            }
            return JsonConvert.SerializeObject(actResult);
        }

        ///// <summary>
        ///// 流式文件上传
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("UploadingStream")]
        //public async Task<IActionResult> UploadingStream()
        //{

        //    //获取boundary
        //    var boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(Request.ContentType).Boundary).Value;
        //    //得到reader
        //    var reader = new MultipartReader(boundary, HttpContext.Request.Body);
        //    //{ BodyLengthLimit = 2000 };//
        //    var section = await reader.ReadNextSectionAsync();

        //    //读取section
        //    while (section != null)
        //    {
        //        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
        //        if (hasContentDispositionHeader)
        //        {
        //            var trustedFileNameForFileStorage = Path.GetRandomFileName();
        //            await WriteFileAsync(section.Body, Path.Combine(_targetFilePath, trustedFileNameForFileStorage));
        //        }
        //        section = await reader.ReadNextSectionAsync();
        //    }
        //    return Created(nameof(FileController), null);
        //}

        /// <summary>
        /// 新增一个文件信息，包括将文件内容保存到磁盘上。
        /// </summary>
        /// <param name="PFile">上传的文件描述信息</param>
        /// <returns></returns>
        [HttpPost("UpLoadFile")]
        [DisableRequestSizeLimit]
        public async Task<string> PostUpLoadFileAsync([FromForm]PlanFile PFile)
        {
            ExcuteResult actResult = new ExcuteResult();

            if (Request.Form.Files.Count > 0 && PFile != null)
            {
                try
                {
                    string FileId = AppCodes.AppStaticClass.GetIdOfDateTime();
                    if (!System.IO.Directory.Exists(_FileBaseDir))
                    {
                        System.IO.Directory.CreateDirectory(_FileBaseDir);
                    }
                    string FileName = Path.Combine(_FileBaseDir, $"{FileId}{PFile.ExtendName}");// _configuration["StaticFileDir"] + $"\\WorkFiles\\{pfile.Id}{pfile.ExtendName}";
                    using (FileStream fs = System.IO.File.Create(FileName))
                    {
                        await Request.Form.Files[0].CopyToAsync(fs).ConfigureAwait(false);
                        fs.Flush();
                    }
                    if (System.IO.File.Exists(FileName))
                    {
                        //文件写入成功后，再保存文件信息到数据表
                        PFile.UpDateTime = DateTime.Now;
                        if (await _FileRepository.AddAsync(PFile, FileId).ConfigureAwait(false) > 0)
                        {
                            actResult.SetValues(0, "上传成功", p_tag: FileId);
                        }
                        else
                        {
                            actResult.SetValues(-3, "文件保存到服务器数据库时出错。");
                        }
                    }
                    else
                    {
                        actResult.SetValues(-1, "文件保存到服务器磁盘时出错。");
                    }
                }
                catch (System.Exception err)
                {
                    actResult.SetValues(1, err.Message);
                }
            }
            return JsonConvert.SerializeObject(actResult);
        }

        ///// <summary>
        ///// 写文件导到磁盘
        ///// </summary>
        ///// <param name="stream">流</param>
        ///// <param name="path">文件保存路径</param>
        ///// <returns></returns>
        //public static async Task<int> WriteFileAsync(System.IO.Stream stream, string path)
        //{
        //    const int FILE_WRITE_SIZE = 84975;//写出缓冲区大小
        //    int writeCount = 0;
        //    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, FILE_WRITE_SIZE, true))
        //    {
        //        byte[] byteArr = new byte[FILE_WRITE_SIZE];
        //        int readCount = 0;
        //        while ((readCount = await stream.ReadAsync(byteArr, 0, byteArr.Length)) > 0)
        //        {
        //            await fileStream.WriteAsync(byteArr, 0, readCount);
        //            writeCount += readCount;
        //        }
        //    }
        //    return writeCount;
        //}

        /// <summary>
        /// 根据Id号从磁盘上下载文件。
        /// </summary>
        /// <param name="FileId">文件Id号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoadFile/{FileId}")]
        public async Task<ActionResult> GetDownLoadFileAsync(string FileId)
        {
            PlanFile FileInfo = await _FileRepository.GetOneByIdAsync(FileId).ConfigureAwait(false);
            if (FileInfo != null)
            {
                string FileName = $"{FileInfo.Name}({FileInfo.Id}){FileInfo.ExtendName}";
                string fileFullName = Path.Combine(_FileBaseDir, $"{FileInfo.Id}{FileInfo.ExtendName}");
                if (System.IO.File.Exists(fileFullName))
                {
                    return PhysicalFile(fileFullName, "application/octet-stream", FileName);
                }
            }
            return NotFound();
        }

        /// <summary>
        /// 删除一个文件信息，包括磁盘上的具体文件。
        /// </summary>
        /// <param name="P_FileId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<string> Delete(string FileId)
        {
            ExcuteResult actResult = new ExcuteResult();
            if (await _FileRepository.DeleteByIdAsync(_FileBaseDir, FileId).ConfigureAwait(false) > 0)
            {
                actResult.SetValues(0, "删除成功!");
            }
            else
            {
                actResult.SetValues(1, "删除失败!");
            }
            return JsonConvert.SerializeObject(actResult);
        }
    }
}
