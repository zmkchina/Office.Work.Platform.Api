using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    /// <summary>
    /// 用户备忘信息查询类
    /// </summary>
    public class MemberScoreRepository
    {
        private readonly GHDbContext _GhDbContext;
        public MemberScoreRepository(GHDbContext ghDbContext)
        {
            _GhDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberScoreEntity>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberScores.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<MemberScoreEntity> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMemberScores.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<List<MemberScoreEntity>> GetEntitiesAsync(MemberScoreSearch SearchCondition)
        {
            List<MemberScoreEntity> RecordList = new List<MemberScoreEntity>();
            IQueryable<MemberScoreEntity> Items = _GhDbContext.dsMemberScores.AsNoTracking() as IQueryable<MemberScoreEntity>;
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(SearchCondition.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.MemberId))
                {
                    Items = Items.Where(e => e.MemberId.Equals(SearchCondition.MemberId, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.ScoreType))
                {
                    Items = Items.Where(e => e.ScoreType.Equals(SearchCondition.ScoreType, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.ScoreUnitName))
                {
                    Items = Items.Where(e => e.ScoreUnitName.Equals(SearchCondition.ScoreUnitName, StringComparison.Ordinal));
                }
               
                if (SearchCondition.OccurYear!=0)
                {
                    Items = Items.Where(e => e.OccurDate.Year.Equals(SearchCondition.OccurYear));
                }
                if (SearchCondition.OccurMonth != 0)
                {
                    Items = Items.Where(e => e.OccurDate.Month.Equals(SearchCondition.OccurMonth));
                }
                RecordList = await Items.OrderByDescending(x => x.UpDateTime).ToListAsync().ConfigureAwait(false);
            }
            return RecordList;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberScoreEntity PEntity)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            PEntity.UpDateTime = DateTime.Now;
            PEntity.Member = null;
            _GhDbContext.dsMemberScores.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }


        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberScoreEntity PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberScores.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            if (Id == null) { return 0; }
            MemberScoreEntity tempRecord = _GhDbContext.dsMemberScores.Find(Id);
            _GhDbContext.dsMemberScores.Remove(tempRecord);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
