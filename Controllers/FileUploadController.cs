using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Office.Work.Platform.Api.DataService;

namespace Office.Work.Platform.Api.Controllers
{
    /// <summary>
    /// 本类暂未使用。
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class FileUpLoadController : ControllerBase
    {
        //private IWebHostEnvironment _webhostEnv;

        //public FileUpLoadController(IWebHostEnvironment webhostEnv)
        //{
        //    _webhostEnv = webhostEnv;
        //}

        private readonly string _FileBaseDir;
        private readonly PlanFileRepository _FileRepository;
        public FileUpLoadController(IConfiguration configuration, GHDbContext ghDbContext)
        {
            _FileRepository = new PlanFileRepository(ghDbContext);
            if (configuration != null)
            {
                _FileBaseDir = Path.Combine(configuration["StaticFileDir"], "PlanFiles");
            }
        }

        /// <summary>
        /// 上传文件到API服务器 较小的文件上传 20M
        /// </summary>
        /// <returns>返回文件的saveKeys</returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        //[RequestSizeLimit(20971520)]
        public async Task<List<string>> UploadFile(List<IFormFile> files)
        {
            List<string> saveKeys = new List<string>();

            foreach (var formFile in files)
            {
                //相对路径
                string saveKey = GetRelativePath(formFile.FileName);

                //完整路径
                string path = GetAbsolutePath(saveKey);
                await WriteFileAsync(formFile.OpenReadStream(), path).ConfigureAwait(false);
                saveKeys.Add(saveKey);
            }
            return saveKeys;
        }
        /// <summary>
        /// 大文件上传到API服务器
        /// 流式接收文件，不缓存到服务器内存/// </summary>
        /// <att name="DisableRequestSizeLimit">不限制请求的大小</att>
        /// <returns>返回文件的saveKeys</returns>
        [HttpPost]
        [Route("BigFileUpload")]
        [DisableRequestSizeLimit]
        public async Task<List<string>> BigFileUpload()
        {
            List<string> saveKeys = new List<string>();

            //获取boundary
            var boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(Request.ContentType).Boundary).Value;
            //得到reader
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync().ConfigureAwait(false);

            //读取 section  每个formData算一个 section  多文件上传时每个文件算一个 section
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
                if (hasContentDispositionHeader)
                {
                    if (contentDisposition.IsFileDisposition())
                    {
                        //相对路径
                        string saveKey = GetRelativePath(contentDisposition.FileName.Value);
                        //完整路径
                        string path = GetAbsolutePath(saveKey);

                        await WriteFileAsync(section.Body, path).ConfigureAwait(false);
                        saveKeys.Add(saveKey);
                    }
                    else
                    {
                        string str = await section.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
                section = await reader.ReadNextSectionAsync().ConfigureAwait(false);
            }
            return saveKeys;

        }
        /// <summary>
        /// 写文件导到磁盘
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="path">文件保存路径</param>
        /// <returns></returns>
        private static async Task<int> WriteFileAsync(System.IO.Stream stream, string path)
        {
            const int FILE_WRITE_SIZE = 84975;//写出缓冲区大小
            int writeCount = 0;
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, FILE_WRITE_SIZE, true))
            {
                byte[] byteArr = new byte[FILE_WRITE_SIZE];
                int readCount = 0;
                while ((readCount = await stream.ReadAsync(byteArr, 0, byteArr.Length).ConfigureAwait(false)) > 0)
                {
                    await fileStream.WriteAsync(byteArr, 0, readCount).ConfigureAwait(false);
                    writeCount += readCount;
                }
            }
            return writeCount;
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetRelativePath(string fileName)
        {
           // string str1 = CommonHelper.GetGuid().Replace("-", "");
            int start = fileName.LastIndexOf('.');
            //相对路径
            return $"/upload/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{(new Guid()).GetHashCode()}{fileName.Substring(start, fileName.Length - start)}";

        }
        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        private string GetAbsolutePath(string relativePath)
        {
            //完整路径
            //string path = Path.Join(_webhostEnv.WebRootPath, relativePath);
            string path = Path.Join(_FileBaseDir, relativePath);

            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return path;
        }
    }
}
