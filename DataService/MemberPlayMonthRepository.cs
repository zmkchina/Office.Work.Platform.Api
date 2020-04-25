using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberPlayMonthRepository
    {
        private readonly GHDbContext _ghDbContext;
        public MemberPlayMonthRepository(GHDbContext ghDbContext)
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
        /// 向数据库表添加一个新的记录，如果该记录已经存在，则更新之。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(MemberPayMonth Entity)
        {
            bool IsExist = await _ghDbContext.dsMemberPayMonth.FirstOrDefaultAsync(e => e.Id == Entity.Id).ConfigureAwait(false) != null;
            if (IsExist)
            {
                _ghDbContext.dsMemberPayMonth.Update(Entity);
            }
            else
            {
                _ghDbContext.dsMemberPayMonth.Add(Entity);
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

        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPayMonth>> GetEntitiesAsync(MemberSearch mSearchMember)
        {
            IQueryable<MemberPayMonth> Items = _ghDbContext.dsMemberPayMonth as IQueryable<MemberPayMonth>;
            if (mSearchMember != null)
            {
                //if (!string.IsNullOrWhiteSpace(mSearchMember.Name))
                //{
                //    Items = Items.Where(e => e.Name.Contains(mSearchMember.Name,StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                //}
                //if (!string.IsNullOrWhiteSpace(mSearchMember.EducationTop))
                //{
                //    Items = Items.Where(e => e.EducationTop.Contains(mSearchMember.EducationTop, StringComparison.Ordinal));
                //}
                //if (!string.IsNullOrWhiteSpace(mSearchMember.TechnicalTitle))
                //{
                //    Items = Items.Where(e => e.TechnicalTitle.Contains(mSearchMember.TechnicalTitle, StringComparison.Ordinal));
                //}
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
            MemberPayMonth tempPlan = _ghDbContext.dsMemberPayMonth.Find(Id);
            _ghDbContext.dsMemberPayMonth.Remove(tempPlan);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
