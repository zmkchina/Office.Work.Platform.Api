using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        /// <summary>
        /// 获取当前用升级文件信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUpdateInfo")]
        public AppUpdateInfo GetUpdateInfo()
        {
            AppUpdateInfo TempUpInfo = null;
            string UpdateFileDir = System.IO.Path.Combine(_configuration["StaticFileDir"], "UpdateFiles");
            if (!System.IO.Directory.Exists(UpdateFileDir))
            {
                System.IO.Directory.CreateDirectory(UpdateFileDir);
                return new AppUpdateInfo();
            }
            string UpdateFileName = System.IO.Path.Combine(UpdateFileDir, "UpdateInfo.json");
            if (!System.IO.File.Exists(UpdateFileName))
            {
                return new AppUpdateInfo();
            }
            string UpdateInfoStr = System.IO.File.ReadAllText(UpdateFileName).Replace("\r\n","",System.StringComparison.Ordinal);
            if (string.IsNullOrWhiteSpace(UpdateInfoStr))
            {
                return new AppUpdateInfo();
            }
            TempUpInfo = JsonConvert.DeserializeObject<AppUpdateInfo>(UpdateInfoStr);
            if (TempUpInfo == null)
            {
                return new AppUpdateInfo();
            }
            return TempUpInfo;
        }

        /// <summary>
        /// 下载新文件
        /// </summary>
        /// <returns></returns>
        [HttpGet("DownFile/{PDownFileName}")]
        public ActionResult GetDownLoadFile(string PDownFileName)
        {
            string fileFullName = System.IO.Path.Combine(_configuration["StaticFileDir"], "UpdateFiles", PDownFileName);
            if (System.IO.File.Exists(fileFullName))
            {
                return PhysicalFile(fileFullName, "application/octet-stream", PDownFileName);
            }
            return NotFound();
        }


        //[HttpGet]
        //public IEnumerable<UpdateFile> Get()
        //{
        //    string UpdateFileDir = System.IO.Path.Combine(_configuration["StaticFileDir"], "UpdateFiles");
        //    if (System.IO.Directory.Exists(UpdateFileDir))
        //    {
        //        System.IO.Directory.CreateDirectory(UpdateFileDir);
        //    }
        //    string[] UpdateFiles = System.IO.Directory.GetFiles(UpdateFileDir);
        //    List<UpdateFile> UpFileList = new List<UpdateFile>();
        //    foreach (string item in UpdateFiles)
        //    {
        //        System.IO.FileInfo curFile = new System.IO.FileInfo(item);
        //        string FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(item).FileVersion;
        //        UpFileList.Add(new UpdateFile
        //        {
        //            FileName = curFile.Name,
        //            Version = FileVersion,
        //            CreateDateTime = curFile.CreationTime
        //        });
        //    }
        //    return UpFileList;
        //}
    }
}
