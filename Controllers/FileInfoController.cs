using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Office.Work.Platform.Api.AppCodes;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class FileInfoController : ControllerBase
    {
        private readonly DataFileRepository _FileRepository;
        private readonly IConfiguration _configuration;
        public FileInfoController(IConfiguration configuration, GHDbContext ghDbContext, ILogger<ModelUser> logger)
        {
            _FileRepository = new DataFileRepository(ghDbContext);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<ModelFile>> GetAsync()
        {
            return await _FileRepository.GetAllAsync();
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ModelFile> GetAsync(string Id)
        {
            return await _FileRepository.GetOneByIdAsync(Id);
        }
        /// <summary>
        /// 用指定的条件类查询文件。
        /// </summary>
        /// <param name="mSearchFile"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IEnumerable<ModelFile>> GetPlanFilesAsync([FromQuery]MSearchFile mSearchFile)
        {
            return await _FileRepository.GetEntitiesAsync(mSearchFile);
        }

        /// <summary>
        /// 上传一个文件，包括文件的相关描述
        /// </summary>
        /// <param name="P_FileInfo"></param>
        /// <param name="P_UpLoadFile"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<string> Post([FromForm]ModelFile P_FileInfo, [FromForm]IFormFile P_UpLoadFile)
        {
            ModelResult actResult = new ModelResult();
            if (P_UpLoadFile != null && P_FileInfo != null)
            {
                try
                {
                    var fileName = _configuration["StaticFileDir"] + $"\\{P_FileInfo.Id}{P_FileInfo.ExtendName}";
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        await P_UpLoadFile.CopyToAsync(fs);
                        fs.Flush();
                    }
                    if (System.IO.File.Exists(fileName))
                    {
                        await _FileRepository.AddAsync(P_FileInfo);
                    }
                    actResult.SetValues(0, "上传成功", @"GHStaticFiles/" + P_FileInfo.Id + P_FileInfo.ExtendName);
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
        /// <param name="P_FileInfo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> Put([FromForm]ModelFile P_FileInfo)
        {
            ModelResult actResult = new ModelResult();
            if (P_FileInfo != null)
            {
                if (await _FileRepository.UpdateAsync(P_FileInfo) > 0)
                {
                    actResult.SetValues(0, "更新成功!", @"GHStaticFiles/" + P_FileInfo.Id + P_FileInfo.ExtendName);
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
        public async Task<string> Delete(string P_FileId, string P_FileExtName)
        {
            ModelResult actResult = new ModelResult();
            if (P_FileId != null)
            {
                if (await _FileRepository.DeleteAsync(P_FileId) > 0)
                {
                    var fileName = _configuration["StaticFileDir"] + $"\\{P_FileId}{P_FileExtName}";
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
    }
}
