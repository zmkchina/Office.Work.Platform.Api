using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
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
        public IEnumerable<ModelUpdateFile> Get()
        {
            string UpdateFileDir = System.IO.Path.Combine(_configuration["StaticFileDir"], "Update");
            string[] UpdateFiles = System.IO.Directory.GetFiles(UpdateFileDir);
            List<ModelUpdateFile> UpFileList = new List<ModelUpdateFile>();
            foreach (string item in UpdateFiles)
            {
                System.IO.FileInfo curFile = new System.IO.FileInfo(item);
                string FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(item).FileVersion;
                UpFileList.Add(new ModelUpdateFile
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
