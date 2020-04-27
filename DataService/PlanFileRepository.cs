using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class PlanFileRepository
    {
        private readonly GHDbContext _ghDbContext;
        public PlanFileRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PlanFile>> GetAllAsync()
        {
            return await _ghDbContext.dsPlanFiles.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<PlanFile> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.dsPlanFiles.FindAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 按指定的条件查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PlanFile>> GetEntitiesAsync(PlanFileSearch mSearchFile)
        {
            IQueryable<PlanFile> Items = _ghDbContext.dsPlanFiles.Include(x=>x.Plan) as IQueryable<PlanFile>;
            if (mSearchFile != null && !string.IsNullOrWhiteSpace(mSearchFile.UserId))
            {
                //判断请求用户是否有权限(必须对该文件所属计划有读取权限)
                Items = Items.Where(e => e.Plan.ReadGrant == null || e.Plan.ReadGrant.Contains(mSearchFile.UserId, System.StringComparison.Ordinal));
                if (!string.IsNullOrWhiteSpace(mSearchFile.PlanId))
                {
                    Items = Items.Where(e => e.PlanId.Equals(mSearchFile.PlanId, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.ContentType))
                {
                    Items = Items.Where(e => e.Plan.PlanType.Equals(mSearchFile.ContentType, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.SearchFromNameDesc))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchFile.SearchFromNameDesc, System.StringComparison.Ordinal) || e.Describe.Contains(mSearchFile.SearchFromNameDesc, System.StringComparison.Ordinal));
                }
                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<PlanFile>();
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(PlanFile Entity)
        {
            if (Entity == null) return 0;
            bool IsExist = await _ghDbContext.dsPlanFiles.AnyAsync(e => e.Id == Entity.Id).ConfigureAwait(false);
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.dsPlanFiles.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanFile Entity)
        {
            _ghDbContext.dsPlanFiles.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {

            PlanFile tempPlan = _ghDbContext.dsPlanFiles.Find(Id);
            _ghDbContext.dsPlanFiles.Remove(tempPlan);

            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
