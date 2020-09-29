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
    public class MemberScoreCountController : ControllerBase
    {
        private readonly GHDbContext _GHDbContext;
        public MemberScoreCountController(GHDbContext ghDbContet)
        {
            _GHDbContext = ghDbContet;
        }
        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <param name="mSearchPlan"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<List<MemberScoreCountDto>> GetRecordsAsync([FromQuery] MemberScoreCountSearch SearchCondition)
        {
            List<Lib.MemberScoreEntity> AllMemberScores = await _GHDbContext.dsMemberScores.Include(x => x.Member).
                Where(x => x.ScoreUnitName.Equals(SearchCondition.ScoreUnitName,StringComparison.Ordinal) && x.OccurDate.Year.ToString(System.Globalization.CultureInfo.InvariantCulture).Equals(SearchCondition.YearNumber,StringComparison.Ordinal))
                //.OrderBy(x=>x.MemberIndex)
                .ToListAsync().ConfigureAwait(false);
            List<IGrouping<string, Lib.MemberScoreEntity>> ListIgroupMemberScores = AllMemberScores.GroupBy(x => x.MemberId).ToList();
            List<MemberScoreCountDto> memberScoreCounts = new List<MemberScoreCountDto>();
            for (int i = 0; i < ListIgroupMemberScores.Count; i++)
            {
                List<Lib.MemberScoreEntity> memberScores = ListIgroupMemberScores[i].ToList();
                memberScoreCounts.Add(new MemberScoreCountDto
                {
                    MemberId = ListIgroupMemberScores[i].Key, //分组key，本例中即为：MemberId
                    MemberName = memberScores[0].Member.Name,
                    MemberIndex = memberScores[0].Member.OrderIndex,
                    ScoreCount = 100f + memberScores.Sum(x => x.Score),
                    YearNumber = SearchCondition.YearNumber,
                    CountDate = DateTime.Now,
                    ScoreUnitName = SearchCondition.ScoreUnitName,
                    UserId = SearchCondition.UserId,
                });
            }
            memberScoreCounts.Sort((x, y) => (int)(y.ScoreCount - x.ScoreCount));
            return memberScoreCounts;
        }
    }
}
