using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class PlanFileRepository
    {
        private readonly GHDbContext _GhDbContext;
        public PlanFileRepository(GHDbContext GhDbContext)
        {
            _GhDbContext = GhDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PlanFile>> GetAllAsync()
        {
            return await _GhDbContext.dsPlanFiles.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<PlanFile> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsPlanFiles.FindAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 按指定的条件查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PlanFile>> GetEntitiesAsync(PlanFileSearch mSearchFile)
        {
            IQueryable<PlanFile> Items = _GhDbContext.dsPlanFiles.Include(x=>x.Plan) as IQueryable<PlanFile>;
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
                if (!string.IsNullOrWhiteSpace(mSearchFile.SearchNameOrDesc))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal) || e.Describe.Contains(mSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal));
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
        public async Task<int> AddAsync(PlanFile Entity,string FileId)
        {
            if (Entity == null || Entity.Id!=null)
            {
                return -2;
            }
            Entity.Id = FileId;
            _GhDbContext.dsPlanFiles.Add(Entity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanFile Entity)
        {
            _GhDbContext.dsPlanFiles.Update(Entity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {

            PlanFile tempPlan = _GhDbContext.dsPlanFiles.Find(Id);
            _GhDbContext.dsPlanFiles.Remove(tempPlan);

            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
