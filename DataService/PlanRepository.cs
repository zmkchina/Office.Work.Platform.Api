using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class PlanRepository
    {
        private readonly GHDbContext _ghDbContext;
        public PlanRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            return await _ghDbContext.dsPlans.ToListAsync().ConfigureAwait(false);
        }
        public async Task<Plan> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.dsPlans.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNewAsync(Plan Entity)
        {
            if (Entity == null || Entity.Id != null)
            {
                return -2;
            }
            Entity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            _ghDbContext.dsPlans.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Plan Entity)
        {
            _ghDbContext.dsPlans.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchPlan">计划查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<Plan>> GetEntitiesAsync(PlanSearch mSearchPlan)
        {
            IQueryable<Plan> Items = _ghDbContext.dsPlans as IQueryable<Plan>;
            if (mSearchPlan != null)
            {
                if (!string.IsNullOrWhiteSpace(mSearchPlan.CreateUserId))
                {
                    Items = Items.Where(e => e.CreateUserId.Equals(mSearchPlan.CreateUserId, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchPlan.CurrectState))
                {
                    Items = Items.Where(e => mSearchPlan.CurrectState.Contains(e.CurrectState, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchPlan.KeysInMultiple))
                {
                    Items = Items.Where(e => e.Caption.Contains(mSearchPlan.KeysInMultiple, StringComparison.Ordinal) || e.Content.Contains(mSearchPlan.KeysInMultiple, StringComparison.Ordinal));
                }
            }
            return await Items.ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteAsync(string Id)
        {
            Plan tempPlan = _ghDbContext.dsPlans.Find(Id);
            if (tempPlan == null)
            {
                return 0;
            }
            _ghDbContext.dsPlans.Remove(tempPlan);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
