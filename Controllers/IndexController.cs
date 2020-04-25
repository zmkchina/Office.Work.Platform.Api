using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class IndexController : ControllerBase
    {
        private readonly SettingsRepository _SettingsRepository;
        private readonly UserRepository _UserRepository;

        public IndexController(GHDbContext ghDbContext)
        {
            _SettingsRepository = new SettingsRepository(ghDbContext);
            _UserRepository = new UserRepository(ghDbContext);

        }
        [HttpGet]
        public string Index()
        {
            return "This is Ok";
        }
        [HttpGet("{pwd}")]
        public async System.Threading.Tasks.Task<string> IndexAsync(string pwd)
        {
            if (pwd != null && pwd.Equals("initdbdata", StringComparison.Ordinal))
            {
                await _SettingsRepository.AddNew(new SettingServer()
                {
                    Id = 1,
                    WorkContentType = "总支议事,党的建设,党风廉政,意识形态,组织人事,劳动工资,制度建设,绩效考核,其他类别",
                    Deparmentts = "政工科,综合科,养护科等",
                    IntervalOne = 20,
                    IntervalTwo = 30

                }).ConfigureAwait(false);
                List<User> SysUsers = new List<User>()
                {
                    new User{Id="admin", PassWord="admin1155",Name="管理员", OrderIndex=0, Department="工作平台",Post="管理员",Grants="AllGrants"},
                    new User{Id="zmk", PassWord="123",Name="翟明科", OrderIndex=1, Department="政工科",Post="部门负责人",Grants="PlanFileDele,PlanDele,PlanFileAdd,PlanResetState"},
                    new User{Id="wt", PassWord="123", Name="吴  桐", OrderIndex=2, Department="政工科",Post="办事员",Grants="PlanFileDele,PlanDele,PlanFileAdd"},
                    new User{Id="gyq", PassWord="123",Name="高亚琼", OrderIndex=3, Department="政工科",Post="办事员",Grants="PlanFileDele,PlanDele,PlanFileAdd"},
                    new User{Id="zhr", PassWord="123",Name="钟浩然", OrderIndex=4, Department="政工科",Post="办事员",Grants="PlanFileDele,PlanDele,PlanFileAdd"}
                };
                foreach (User item in SysUsers)
                {
                    await _UserRepository.AddNew(item).ConfigureAwait(false);

                }
                //以下代码报错：在上一个操作完成之前，在此上下文上启动的第二个操作。 这通常是由使用同一个 DbContext 实例的不同线程引起的，但不保证实例成员是线程安全的。
                //说明“ForEach”与“foreach”内部实现不一样，前者是循环一个委托。
                //SysUsers.ForEach(async u =>
                //{
                //    await _UserRepository.AddNew(u).ConfigureAwait(true);
                //});
                return "Data added ok.";
            }
            return "password is error!";
        }
    }
}
