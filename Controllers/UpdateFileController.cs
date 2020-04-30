using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    /// <summary>
    /// 此类用于获取文件升级目录下文件信息，以便客户端与本地文件进行比较。
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class UpdateFileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UpdateFileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IEnumerable<UpdateFile> Get()
        {
            string UpdateFileDir = System.IO.Path.Combine(_configuration["StaticFileDir"], "UpdateFiles");
            if (System.IO.Directory.Exists(UpdateFileDir))
            {
                System.IO.Directory.CreateDirectory(UpdateFileDir);
            }
            string[] UpdateFiles = System.IO.Directory.GetFiles(UpdateFileDir);
            List<UpdateFile> UpFileList = new List<UpdateFile>();
            foreach (string item in UpdateFiles)
            {
                System.IO.FileInfo curFile = new System.IO.FileInfo(item);
                string FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(item).FileVersion;
                UpFileList.Add(new UpdateFile
                {
                    FileName = curFile.Name,
                    Version = FileVersion,
                    LastWriteTime = curFile.LastWriteTime
                });
            }
            return UpFileList;
        }
    }
}
