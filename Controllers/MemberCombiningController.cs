using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Office.Work.Platform.Api.AppCodes;
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
        [HttpGet("GetMemberPayMonthOfficialSheet/{PayYear}/{PayMonth}")]
        public async Task<IEnumerable<MemberPayMonthOfficialSheet>> GetAsync(int PayYear, int PayMonth)
        {
            return await _GhDbContext.dsMemberPayMonthOfficial.Join(_GhDbContext.dsMembers, p => p.MemberId, k => k.Id, (p, k) => new
            {
                MemberName = k.Name,
                pm = p
            }).Join(_GhDbContext.dsMemberPayMonthInsurance, pet => pet.pm.MemberId, per => per.MemberId, (pet, per) =>
            new MemberPayMonthOfficialSheet
            {
                MemberName = pet.MemberName,
                PayYear = pet.pm.PayYear,
                PayMonth = pet.pm.PayMonth,
                MemberId = pet.pm.MemberId,
                PostPay = pet.pm.PostPay,
                PostAllowance = pet.pm.PostAllowance,
                ScalePay = pet.pm.ScalePay,
                LivingAllowance = pet.pm.LivingAllowance,
                HousingFund = per.HousingFund,
                OccupationalPension = per.OccupationalPension,
                PensionInsurance = per.PensionInsurance,
                UnemploymentInsurance = per.UnemploymentInsurance,
                MedicalInsurance = per.MedicalInsurance,
                UnionFees = per.UnionFees,
                Tax = per.Tax,
                Remark = $"{pet.pm.Remark} {per.Remark}"

            }).Where(e => e.PayYear == PayYear && e.PayMonth == PayMonth).Distinct().ToListAsync().ConfigureAwait(false);
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
            await Task.Run(() =>
            {
                List<Member> members = _GhDbContext.dsMembers.Where(e => e.EmploymentType.Equals("聘用合同制", System.StringComparison.Ordinal)).ToList();
                foreach (Member mitem in members)
                {
                    bool AddMem = false;
                    //1.新增发放记录
                    MemberPayMonthOfficial PmoPrevMonth = _GhDbContext.dsMemberPayMonthOfficial.Where(x =>
                    x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) && x.PayYear == PayYear && x.PayMonth == PayMonth - 1).FirstOrDefault();
                    if (PmoPrevMonth != null)
                    {
                        //说明该职工上个月有工资数据。

                        if (!_GhDbContext.dsMemberPayMonthOfficial.Any(x => x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) && x.PayYear == PayYear && x.PayMonth == PayMonth))
                        {
                            //说明本月没有记录，则新增之。
                            PmoPrevMonth.Id = AppStaticClass.GetIdOfDateTime();
                            PmoPrevMonth.PayYear = PayYear;
                            PmoPrevMonth.PayMonth = PayMonth;
                            _GhDbContext.dsMemberPayMonthOfficial.Add(PmoPrevMonth);
                            AddMem = true;
                        }
                    }

                    //2.新增扣款记录
                    MemberPayMonthInsurance PmiPrevMonth = _GhDbContext.dsMemberPayMonthInsurance.Where(x =>
                        x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) &&  x.PayYear == PayYear && x.PayMonth == PayMonth - 1).FirstOrDefault();

                    if (PmiPrevMonth != null)
                    {
                        //说明该职工上个月有扣款数据。

                        if (!_GhDbContext.dsMemberPayMonthInsurance.Any(x => x.MemberId.Equals(mitem.Id, System.StringComparison.Ordinal) && x.PayYear == PayYear && x.PayMonth == PayMonth))
                        {
                            //说明本月没有记录，则新增之。
                            PmiPrevMonth.Id = AppStaticClass.GetIdOfDateTime();
                            PmiPrevMonth.PayYear = PayYear;
                            PmiPrevMonth.PayMonth = PayMonth;
                            _GhDbContext.dsMemberPayMonthInsurance.Add(PmiPrevMonth);
                            AddMem = true;
                        }
                    }
                    if (AddMem) { AddMemberCount++; }
                }
                AddRecordCount = _GhDbContext.SaveChanges();

            }).ConfigureAwait(false);

            excuteResult = new ExcuteResult(0, $"共为{AddMemberCount}名职工，新增了{AddRecordCount}记录数据！");
            return excuteResult;
        }
    }
}

