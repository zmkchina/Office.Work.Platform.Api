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
        private readonly MemberPayItemRepository _PayItemRepository;

        public IndexController(GHDbContext ghDbContext)
        {
            _SettingsRepository = new SettingsRepository(ghDbContext);
            _UserRepository = new UserRepository(ghDbContext);
            _PayItemRepository = new MemberPayItemRepository(ghDbContext);

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
                await _SettingsRepository.AddNew(new SettingServer()).ConfigureAwait(false);
                List<User> SysUsers = new List<User>()
                {
                    new User{Id="admin", PassWord="admin1155",Name="管理员", UnitName="市港航事业发展中心", UnitShortName="市港航中心",  OrderIndex=0, Department="工作平台",Post="管理员",Grants="AllGrants"},
                    new User{Id="zmk", PassWord="123",Name="翟明科",UnitName="市港航事业发展中心", UnitShortName="市港航中心", OrderIndex=1, Department="政工科",Post="部门负责人",Grants="PlanFileDele,PlanDele,PlanFileAdd,PlanResetState"},
                    new User{Id="wt", PassWord="123", Name="吴  桐",UnitName="市港航事业发展中心", UnitShortName="市港航中心",  OrderIndex=2, Department="政工科",Post="办事员",Grants="PlanFileDele,PlanDele,PlanFileAdd"},
                    new User{Id="gyq", PassWord="123",Name="高亚琼",UnitName="市港航事业发展中心", UnitShortName="市港航中心",  OrderIndex=3, Department="政工科",Post="办事员",Grants="PlanFileDele,PlanDele,PlanFileAdd"},
                    new User{Id="zhr", PassWord="123",Name="钟浩然",UnitName="市港航事业发展中心", UnitShortName="市港航中心",  OrderIndex=4, Department="政工科",Post="办事员",Grants="PlanFileDele,PlanDele,PlanFileAdd"}
                };
                foreach (User item in SysUsers)
                {
                    await _UserRepository.AddAsync(item).ConfigureAwait(false);

                }
                //以下代码报错：在上一个操作完成之前，在此上下文上启动的第二个操作。 这通常是由使用同一个 DbContext 实例的不同线程引起的，但不保证实例成员是线程安全的。
                //说明“ForEach”与“foreach”内部实现不一样，前者是循环一个委托。
                //SysUsers.ForEach(async u =>
                //{
                //    await _UserRepository.AddNew(u).ConfigureAwait(true);
                //});
                List<MemberPayItem> PayItems = new List<MemberPayItem>()
                {
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="岗位工资",PayType="应发待遇",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=0, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="薪级工资",PayType="应发待遇",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制",OrderIndex=1, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="岗位津贴",PayType="应发待遇",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制",OrderIndex=2, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="生活补贴",PayType="应发待遇",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制",OrderIndex=3, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="奖励绩效(月)",PayType="应发待遇",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制",OrderIndex=4, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="养老保险",PayType="个人交纳",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=5, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="职业年金",PayType="个人交纳",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制",OrderIndex=6, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="住房公积金",PayType="个人交纳",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=7, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="失业保险",PayType="个人交纳",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=8, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="医疗保险",PayType="个人交纳",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=9, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="工会费",PayType="个人交纳",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=10, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="个税",PayType="个人交纳",InCardinality="是",InTableType="月度工资表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=11, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="伙食补助",PayType="应发待遇",InCardinality="是",InTableType="月度补贴表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=20, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="住房补贴",PayType="应发待遇",InCardinality="是",InTableType="月度补贴表",MemberTypes="聘用合同制",OrderIndex=21, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="交通补助",PayType="应发待遇",InCardinality="是",InTableType="月度补贴表",MemberTypes="聘用合同制",OrderIndex=22, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="奖励绩效(年)",PayType="应发待遇",InCardinality="是",InTableType="其他待遇表",MemberTypes="聘用合同制",OrderIndex=30, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="绩效考核奖",PayType="应发待遇",InCardinality="是",InTableType="其他待遇表",MemberTypes="聘用合同制,劳动合同制,劳务合同制",OrderIndex=31, UserId="zmk"},
                    new MemberPayItem{UnitName="市港航事业发展中心",Name="护理费",PayType="应发待遇",InCardinality="是",InTableType="其他待遇表",MemberTypes="聘用合同制",OrderIndex=32, UserId="zmk"}
                };
                foreach (MemberPayItem item in PayItems)
                {
                    await _PayItemRepository.AddAsync(item).ConfigureAwait(false);

                }
                return "Data added ok.";
            }
            return "password is error!";
        }
    }
}
