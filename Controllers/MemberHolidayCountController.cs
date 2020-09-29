using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class MemberHolidayCountController : ControllerBase
    {
        private readonly GHDbContext _GHDbContext;
        public MemberHolidayCountController(GHDbContext ghDbContet)
        {
            _GHDbContext = ghDbContet;
        }
        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<List<Lib.MemberHolidayCountDto>> GetRecordsAsync([FromQuery] MemberHolidayCountSearch SearchCondition)
        {
            List<Lib.MemberHolidayEntity> AllMemberHolidaies = await _GHDbContext.dsMemberHoliday.Include(x => x.Member).
                Where(x => x.UnitName.Equals(SearchCondition.UnitName,StringComparison.Ordinal) && Convert.ToString(x.EndDate.Year, System.Globalization.CultureInfo.InvariantCulture).Equals(SearchCondition.YearNumber, StringComparison.Ordinal))
                .OrderBy(x => x.Member.OrderIndex)
                .ToListAsync().ConfigureAwait(false);
            List<IGrouping<string, Lib.MemberHolidayEntity>> ListIgroupMemberHolidaies = AllMemberHolidaies.GroupBy(x => x.MemberId).ToList();
            List<MemberHolidayCountDto> memberScoreCounts = new List<MemberHolidayCountDto>();
            for (int i = 0; i < ListIgroupMemberHolidaies.Count; i++)
            {
                List<Lib.MemberHolidayEntity> memberScores = ListIgroupMemberHolidaies[i].ToList();
                memberScoreCounts.Add(new MemberHolidayCountDto
                {
                    MemberId = ListIgroupMemberHolidaies[i].Key, //分组key，本例中即为：MemberId
                    UnitName = SearchCondition?.UnitName,
                    MemberName = memberScores[0].Member.Name,
                    PersonalLeaveCount = memberScores.Where(x => x.HolidayType.Equals("事假", StringComparison.Ordinal)).Sum(x => (x.EndDate - x.BeginDate).Days),
                    AnnualCount = memberScores.Where(x => x.HolidayType.Equals("年休假", StringComparison.Ordinal)).Sum(x => (x.EndDate - x.BeginDate).Days),
                    SickLeaveCount = memberScores.Where(x => x.HolidayType.Equals("病假", StringComparison.Ordinal)).Sum(x => (x.EndDate - x.BeginDate).Days),
                    OtherHolidayCount = memberScores.Where(x => !(new string[] { "事假", "年休假", "病假" }).Contains(x.HolidayType)).Sum(x => (x.EndDate - x.BeginDate).Days),
                    YearNumber = SearchCondition.YearNumber,
                    CountDate = DateTime.Now
                });
            }
            return memberScoreCounts;
        }
    }
}
