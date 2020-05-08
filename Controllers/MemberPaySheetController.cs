using System;
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
        public async Task<string> GetMemberPaySheet([FromQuery]MemberPaySheetSearch sc)
        {
            JArray JResult = new JArray();
            //1.读取所有符合条件的发放记录

            List<MemberPay> MemberPayList = new List<MemberPay>();
            MemberPayList = await _GhDbContext.dsMemberPay.Where(x => x.PayYear == sc.PayYear && x.PayMonth == sc.PayMonth
              && x.PayUnitName.Equals(sc.PayUnitName, System.StringComparison.Ordinal)
              && x.MemberType.Equals(sc.MemberType, System.StringComparison.Ordinal)
              && x.InTableType.Equals(sc.PayTableType, System.StringComparison.Ordinal)).ToListAsync().ConfigureAwait(false);

            //2.获得不重复且排序后的人员身份证号数组
            string[] PayMemberIdArr = MemberPayList.OrderBy(x => x.MemberIndex).Select(x => x.MemberId).Distinct().ToArray();

            //3.获取所有排序后的已发放项目列表
            string[] PayItemNameArr = MemberPayList.OrderBy(x => x.OrderIndex).Select(x => x.PayName).Distinct().ToArray();

            //4.添加数据
            for (int i = 0; i < PayMemberIdArr.Length; i++)
            {
                JObject TempJobj = new JObject();
                //获取该人员的所有发放记录。
                List<MemberPay> TempPayList = MemberPayList.Where(x => x.MemberId.Equals(PayMemberIdArr[i], System.StringComparison.Ordinal)).ToList();
                TempJobj.Add("姓名", TempPayList[0].MemberName);
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
        ///快速生成工资发放记录（将符合条件人员的指定月份的工资从上月开始拷贝）。
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostMemberPaySheet")]
        public async Task<ExcuteResult> PostMemberPaySheet([FromBody]MemberPayFastByPaySet PayFastInfo)
        {
            if (PayFastInfo == null || PayFastInfo.PayYear == 0 || PayFastInfo.PayMonth == 0 || string.IsNullOrWhiteSpace(PayFastInfo.PayUnitName))
            {
                return new ExcuteResult(0, "参数传递不正确！");
            }
            //1.查询所有需要快速发放工资的人员及需拷贝的发放项目信息
            List<MemberPaySet> MemberPaySetList = new List<MemberPaySet>();

            IQueryable<MemberPaySet> Items = _GhDbContext.dsMemberPaySet.Join(_GhDbContext.dsMembers, x => x.MemberId, k => k.Id, (x, k) => new MemberPaySet
            {
                PayUnitName = x.PayUnitName,
                MemberId = x.MemberId,
                MemberName = k.Name,
                MemberType = k.MemberType,
                OrderIndex = k.OrderIndex,
                MemberUnitName = k.UnitName,
                PayItemNames = x.PayItemNames,
                UserId = x.UserId,
                UpDateTime = x.UpDateTime
            }) as IQueryable<MemberPaySet>;

            if (!string.IsNullOrWhiteSpace(PayFastInfo.PayUnitName))
            {
                Items = Items.Where(e => e.PayUnitName.Equals(PayFastInfo.PayUnitName, StringComparison.Ordinal));
            }
            if (!string.IsNullOrWhiteSpace(PayFastInfo.MemberType))
            {
                Items = Items.Where(e => e.MemberType.Equals(PayFastInfo.MemberType, StringComparison.Ordinal));
            }
            if (!string.IsNullOrWhiteSpace(PayFastInfo.MemberId))
            {

                Items = Items.Where(e => e.MemberId.Equals(PayFastInfo.MemberId, StringComparison.Ordinal));
            }

            MemberPaySetList = await Items.ToListAsync().ConfigureAwait(false);

            if (MemberPaySetList == null || MemberPaySetList.Count < 1)
            {
                return new ExcuteResult(0, "请先配置需要快速发放待遇的人员信息！");
            }

            //2.查询所有可发放项目信息数据，待使用。
            List<MemberPayItem> AllPayItems = await _GhDbContext.dsMemberPayItem.ToListAsync().ConfigureAwait(false);

            //3.处理、年和月。对所有需要快速发放待遇的人员，进行逐一遍历。
            int PrePayYear = PayFastInfo.PayYear;
            int PrePayMonth = PayFastInfo.PayMonth;
            if (PrePayMonth == 1)
            {
                PrePayYear -= 1;
                PrePayMonth = 12;
            }
            else
            {
                PrePayMonth -= 1;
            }
            //4.对所有需要快速发放待遇的人员，进行逐一遍历。
            foreach (MemberPaySet item in MemberPaySetList)
            {
                
                //查询指定人员、所有已配置的需要快速发放的项目的上月发放记录。
                List<MemberPay> OneMemberPayList = await _GhDbContext.dsMemberPay.Where(x => item.PayItemNames.Contains(x.PayName, StringComparison.Ordinal) &&
                x.MemberId.Equals(item.MemberId, StringComparison.Ordinal) && x.PayYear == PrePayYear && x.PayMonth == PrePayMonth).ToListAsync().ConfigureAwait(false);

                foreach (MemberPay CurPayInfo in OneMemberPayList)
                {
                    MemberPayItem CurPayItem = AllPayItems.Where(x => x.Name.Equals(CurPayInfo.PayName, StringComparison.Ordinal)).FirstOrDefault();
                    if (CurPayItem == null)
                    {
                        //如果最新的项目配置信息中，该项目已经不存在，则跳过之不再拷贝。
                        continue;
                    }
                    //检查将要拷贝的月份数据，指定的项目是否已经发放过（比如已通过程序手动发放）。
                    bool hasPayed = await _GhDbContext.dsMemberPay.AnyAsync(x => x.MemberId == item.MemberId && x.PayName.Equals(CurPayInfo.PayName, StringComparison.Ordinal)
                    && x.PayYear == PayFastInfo.PayYear && x.PayMonth == PayFastInfo.PayMonth).ConfigureAwait(false);
                    if (!hasPayed)
                    {
                        //将该发放项目相关信息更新为与最新设置同步。
                        CurPayInfo.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
                        CurPayInfo.PayYear = PayFastInfo.PayYear;
                        CurPayInfo.PayMonth = PayFastInfo.PayMonth;
                        CurPayInfo.AddOrCut = CurPayItem.AddOrCut;
                        CurPayInfo.InCardinality = CurPayItem.InCardinality;
                        CurPayInfo.InTableType = CurPayItem.InTableType;
                        CurPayInfo.OrderIndex = CurPayItem.OrderIndex;
                        CurPayInfo.PayUnitName = item.PayUnitName;
                        CurPayInfo.MemberName = item.MemberName;
                        CurPayInfo.MemberType = item.MemberType;
                        CurPayInfo.MemberIndex = item.OrderIndex;
                        CurPayInfo.UpDateTime = DateTime.Now;
                        CurPayInfo.UserId = PayFastInfo.UserId;
                        await _GhDbContext.dsMemberPay.AddAsync(CurPayInfo).ConfigureAwait(false);
                    }
                }
            }
            int AddCount = await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
            return new ExcuteResult(0, $"待遇快速发放成功，共发放{AddCount}条记录");
        }
    }
}

