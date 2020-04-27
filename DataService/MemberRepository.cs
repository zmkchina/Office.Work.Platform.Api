using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberRepository
    {
        private readonly GHDbContext _ghDbContext;
        public MemberRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _ghDbContext.dsMembers.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<Member> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.dsMembers.FindAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，则更新之。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNewAsync(Member Entity)
        {
            bool IsExist = await _ghDbContext.dsMembers.AnyAsync(e => e.Id == Entity.Id).ConfigureAwait(false);
            if (IsExist)
            {
                return -2;
            }
            else
            {
                _ghDbContext.dsMembers.Add(Entity);
            }
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 向数据库表添加一批记录。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddRangeAsync(List<Member> Entitys)
        {
            if (Entitys != null && Entitys.Count > 0)
            {
                _ghDbContext.dsMembers.AddRange(Entitys);
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
        public async Task<int> UpdateAsync(Member Entity)
        {
            _ghDbContext.dsMembers.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<Member>> GetEntitiesAsync(MemberSearch mSearchMember)
        {
            IQueryable<Member> Items = _ghDbContext.dsMembers as IQueryable<Member>;
            if (mSearchMember != null)
            {
                if (!string.IsNullOrWhiteSpace(mSearchMember.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(mSearchMember.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.Name))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchMember.Name, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.EducationTop))
                {
                    Items = Items.Where(e => e.EducationTop.Contains(mSearchMember.EducationTop, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.TechnicalTitle))
                {
                    Items = Items.Where(e => e.TechnicalTitle.Contains(mSearchMember.TechnicalTitle, StringComparison.Ordinal));
                }
            }

            return await Items.ToListAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            Member tempPlan = _ghDbContext.dsMembers.Find(Id);
            _ghDbContext.dsMembers.Remove(tempPlan);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
