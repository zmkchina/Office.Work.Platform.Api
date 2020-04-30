using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberPayMonthRepository
    {
        private readonly GHDbContext _ghDbContext;
        public MemberPayMonthRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPayMonth>> GetAllAsync()
        {
            return await _ghDbContext.dsMemberPayMonth.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberPayMonth> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.dsMemberPayMonth.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPayMonth>> GetEntitiesAsync(MemberPayMonthSearch SearchCondition)
        {
            IQueryable<MemberPayMonth> Items = _ghDbContext.dsMemberPayMonth as IQueryable<MemberPayMonth>;
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(SearchCondition.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.MemberId))
                {
                    Items = Items.Where(e => e.MemberId.Equals(SearchCondition.MemberId, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Remark))
                {
                    Items = Items.Where(e => e.Remark.Contains(SearchCondition.Remark, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (SearchCondition.PayYear > 0)
                {
                    Items = Items.Where(e => e.PayYear == SearchCondition.PayYear);
                }
                if (SearchCondition.PayMonth > 0)
                {
                    Items = Items.Where(e => e.PayMonth == SearchCondition.PayMonth);
                }
                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<MemberPayMonth>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberPayMonth PEntity)
        {
            bool IsExist = await _ghDbContext.dsMemberPayMonth.AnyAsync(e => e.Id.Equals(PEntity.Id, StringComparison.Ordinal)).ConfigureAwait(false);
            if (IsExist)
            {
                return -2;
            }
            else
            {
                _ghDbContext.dsMemberPayMonth.Add(PEntity);
            }
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 向数据库表添加一批记录。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddRangeAsync(List<MemberPayMonth> Entitys)
        {
            if (Entitys != null && Entitys.Count > 0)
            {
                _ghDbContext.dsMemberPayMonth.AddRange(Entitys);
                return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberPayMonth Entity)
        {
            _ghDbContext.dsMemberPayMonth.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            MemberPayMonth tempPlan = _ghDbContext.dsMemberPayMonth.Find(Id);
            _ghDbContext.dsMemberPayMonth.Remove(tempPlan);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
