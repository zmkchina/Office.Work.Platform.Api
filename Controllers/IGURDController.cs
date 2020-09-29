using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    /// <summary>
    /// 定义控制器泛型接口
    /// </summary>
    interface IGURDController
    {
        Task<ActionResult<List<OutT>>> ReadDtos<OutT, InT>([FromQuery] InT SearchCondition);
    }
}
