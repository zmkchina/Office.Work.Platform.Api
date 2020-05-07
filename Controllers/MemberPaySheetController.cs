using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class MemberPaySheetController : ControllerBase
    {
        private readonly GHDbContext _GhDbContext;

        public MemberPaySheetController(GHDbContext ghDbContet)
        {
            _GhDbContext = ghDbContet;
        }
        /// <summary>
        /// 获取所有待遇表类型
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        [HttpGet("GetPayTableTypes")]
        public async Task<string[]> GetPayTableTypes()
        {
            return await _GhDbContext.dsMemberPay.Select(x => x.InTableType).Distinct().ToArrayAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 获取正式人员月度工资表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMemberPaySheet")]
        public async Task<string> GetMemberPaySheet([FromBody]MemberPaySheetSearch sc)
        {
            JArray JResult = new JArray();
            //1.读取所有符合条件的发放记录

            List<MemberPay> MemberPayList = new List<MemberPay>();
            MemberPayList = await _GhDbContext.dsMemberPay.Where(x => x.PayYear == sc.PayYear && x.PayMonth == sc.PayMonth
              && x.PayUnitName.Equals(sc.PayUnitName, System.StringComparison.Ordinal)
              && x.InTableType.Equals(sc.PayTableType, System.StringComparison.Ordinal)).ToListAsync().ConfigureAwait(false);

            //2.获得不重复且排序后的人员姓名数组
            string[] PayMemberIdArr = MemberPayList.OrderBy(x => x.MemberIndex).Select(x => x.MemberId).Distinct().ToArray();

            //3.获取所有排序后的已发放项目列表
            string[] PayItemNameArr = MemberPayList.OrderBy(x => x.OrderIndex).Select(x => x.PayName).Distinct().ToArray();

            //4.添加数据
            for (int i = 0; i < PayMemberIdArr.Length; i++)
            {
                JObject TempJobj = new JObject();
                List<MemberPay> TempPayList = MemberPayList.Where(x => x.MemberId.Equals(PayMemberIdArr[i], System.StringComparison.Ordinal)).ToList();
                for (int j = 0; j < PayItemNameArr.Length; j++)
                {
                    MemberPay TempPay = TempPayList.Where(y => y.PayName.Equals(PayItemNameArr[j], System.StringComparison.Ordinal)).FirstOrDefault();
                    if (TempPay != null)
                    {
                        TempJobj.Add(PayItemNameArr[j], TempPay.Amount);
                    }
                    else
                    {
                        TempJobj.Add(PayItemNameArr[j], "");
                    }
                }
                JResult.Add(TempJobj);
            }
            string jsonStr = JsonConvert.SerializeObject(JResult);
            return jsonStr;
        }
        /// <summary>
        ///为正式人员月度工资表，生成数据。
        /// </summary>
        /// <returns></returns>
        [HttpGet("PostMemberPayMonthOfficialSheet/{PayYear}/{PayMonth}")]
        public async Task<ExcuteResult> PostAsync(int PayYear, int PayMonth)
        {
            int AddMemberCount = 0;
            int AddRecordCount = 0;
            ExcuteResult excuteResult = new ExcuteResult();
            //await Task.Run(() =>
            //{
            //    List<Member> members = _GhDbContext.dsMembers.Where(e => e.EmploymentType.Equals("聘用合同制", System.StringComparison.Ordinal)).ToList();
            //    foreach (Member mitem in members)
            //    {
            //        bool AddMem = false;
            //        //1.新增发放记录
            //        MemberPayMonthOfficial PmoPrevMonth = _GhDbContext.dsMemberPayMonthOfficial.Where(x =>
            //        x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) && x.PayYear == PayYear && x.PayMonth == PayMonth - 1).FirstOrDefault();
            //        if (PmoPrevMonth != null)
            //        {
            //            //说明该职工上个月有工资数据。

            //            if (!_GhDbContext.dsMemberPayMonthOfficial.Any(x => x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) && x.PayYear == PayYear && x.PayMonth == PayMonth))
            //            {
            //                //说明本月没有记录，则新增之。
            //                PmoPrevMonth.Id = AppStaticClass.GetIdOfDateTime();
            //                PmoPrevMonth.PayYear = PayYear;
            //                PmoPrevMonth.PayMonth = PayMonth;
            //                _GhDbContext.dsMemberPayMonthOfficial.Add(PmoPrevMonth);
            //                AddMem = true;
            //            }
            //        }

            //        //2.新增扣款记录
            //        MemberPayMonthInsurance PmiPrevMonth = _GhDbContext.dsMemberPayMonthInsurance.Where(x =>
            //            x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) && x.PayYear == PayYear && x.PayMonth == PayMonth - 1).FirstOrDefault();

            //        if (PmiPrevMonth != null)
            //        {
            //            //说明该职工上个月有扣款数据。

            //            if (!_GhDbContext.dsMemberPayMonthInsurance.Any(x => x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) && x.PayYear == PayYear && x.PayMonth == PayMonth))
            //            {
            //                //说明本月没有记录，则新增之。
            //                PmiPrevMonth.Id = AppStaticClass.GetIdOfDateTime();
            //                PmiPrevMonth.PayYear = PayYear;
            //                PmiPrevMonth.PayMonth = PayMonth;
            //                _GhDbContext.dsMemberPayMonthInsurance.Add(PmiPrevMonth);
            //                AddMem = true;
            //            }
            //        }
            //        if (AddMem) { AddMemberCount++; }
            //    }
            //    AddRecordCount = _GhDbContext.SaveChanges();

            //}).ConfigureAwait(false);

            //excuteResult = new ExcuteResult(0, $"共为{AddMemberCount}名职工，新增了{AddRecordCount}记录数据！");
            return excuteResult;
        }
    }
}

