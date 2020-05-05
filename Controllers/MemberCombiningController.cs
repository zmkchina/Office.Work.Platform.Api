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
    public class MemberCombiningController : ControllerBase
    {
        private readonly GHDbContext _GhDbContext;

        public MemberCombiningController(GHDbContext ghDbContet)
        {
            _GhDbContext = ghDbContet;
        }
        /// <summary>
        /// 获取正式人员月度工资表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPingYongMemberMonthPaySheet/{PayYear}/{PayMonth}")]
        public async Task<string> GetAsync(int PayYear, int PayMonth)
        {
            JArray JResult = new JArray();
            //1.查询聘用合同制人员列表
            List<Member> members = _GhDbContext.dsMembers.Where(e => e.EmploymentType.Equals("聘用合同制", System.StringComparison.Ordinal)).ToList();

            ////2.查询放在聘用合同制人员+需要放入月度工资表的项目的 Name+Id
            //List<MemberPayItem> payItems = await _GhDbContext.dsMemberPayItem
            //    .Where(x => x.InTableTypeList.Equals("月度工资表") && x.MemberType.Contains("聘用合同制", System.StringComparison.Ordinal))
            //    .ToListAsync().ConfigureAwait(false);
            //string[] payItemIds = payItems.Select(x => x.Id).ToArray();

            //3.搜索用户待遇表。
            foreach (Member item in members)
            {
                //(1)查询了某个聘用合同制人员需要放入“月度工资表”中的所有待遇信息

                List<MemberPay> mpList = await _GhDbContext.dsMemberPay
                    .Where(x => x.PayDate.Year == PayYear && x.PayDate.Month == PayMonth && x.MemberId.Equals(item.Id, System.StringComparison.Ordinal)
                    && x.InTableType.Equals("月度工资表", System.StringComparison.Ordinal)).ToListAsync().ConfigureAwait(false);
                JObject jObject = new JObject();
                jObject["姓名"] = item.Name;
                jObject["年份"] = PayYear;
                jObject["月份"] = PayMonth;
                foreach (MemberPay iPay in mpList)
                {
                    jObject[iPay.PayName] = iPay.Amount;
                }
                JResult.Add(jObject);
            }
            string jsonStr = JsonConvert.SerializeObject(JResult);
            return jsonStr;

            //return await _GhDbContext.dsMemberPayMonthOfficial.Join(_GhDbContext.dsMembers, p => p.MemberId, k => k.Id, (p, k) => new
            //{
            //    k.OrderIndex,
            //    MemberName = k.Name,
            //    pm = p
            //}).Join(_GhDbContext.dsMemberPayMonthInsurance, pet => pet.pm.MemberId, per => per.MemberId, (pet, per) =>
            //new MemberPayMonthOfficialSheet
            //{
            //    OrderIndex = pet.OrderIndex,
            //    MemberName = pet.MemberName,
            //    PayYear = pet.pm.PayYear,
            //    PayMonth = pet.pm.PayMonth,
            //    MemberId = pet.pm.MemberId,
            //    PostPay = pet.pm.PostPay,
            //    PostAllowance = pet.pm.PostAllowance,
            //    ScalePay = pet.pm.ScalePay,
            //    LivingAllowance = pet.pm.LivingAllowance,
            //    HousingFund = per.HousingFund,
            //    OccupationalPension = per.OccupationalPension,
            //    PensionInsurance = per.PensionInsurance,
            //    UnemploymentInsurance = per.UnemploymentInsurance,
            //    MedicalInsurance = per.MedicalInsurance,
            //    UnionFees = per.UnionFees,
            //    Tax = per.Tax,
            //    Remark = $"{pet.pm.Remark} {per.Remark}"

            //}).Where(e => e.PayYear == PayYear && e.PayMonth == PayMonth).Distinct().ToListAsync().ConfigureAwait(false);
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

