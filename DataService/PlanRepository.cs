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
        private readonly GHDbContext _GhDbContext;
        public PlanRepository(GHDbContext GhDbContext)
        {
            _GhDbContext = GhDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            return await _GhDbContext.dsPlans.ToListAsync().ConfigureAwait(false);
        }
        public async Task<Plan> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsPlans.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchPlan">计划查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<Plan>> GetEntitiesAsync(PlanSearch mSearchPlan)
        {
            IQueryable<Plan> Items = _GhDbContext.dsPlans as IQueryable<Plan>;
            if (mSearchPlan != null)
            {
                if (!string.IsNullOrWhiteSpace(mSearchPlan.CreateUserId))
                {
                    Items = Items.Where(e => e.CreateUserId.Equals(mSearchPlan.CreateUserId, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchPlan.UnitName))
                {
                    Items = Items.Where(e => e.UnitName.Equals(mSearchPlan.UnitName, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchPlan.ResponsiblePerson))
                {
                    Items = Items.Where(e => e.ResponsiblePerson.Equals(mSearchPlan.ResponsiblePerson, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchPlan.CurrectState))
                {
                    Items = Items.Where(e => mSearchPlan.CurrectState.Contains(e.CurrectState, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchPlan.Helpers))
                {
                    Items = Items.Where(e => e.Helpers.Contains(mSearchPlan.Helpers, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchPlan.KeysInMultiple))
                {
                    Items = Items.Where(e => e.Caption.Contains(mSearchPlan.KeysInMultiple, StringComparison.Ordinal) || e.Content.Contains(mSearchPlan.KeysInMultiple, StringComparison.Ordinal));
                }
            }
            return await Items.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(Plan PEntity)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsPlans.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Plan PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsPlans.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// 删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        public async Task<int> DeleteAsync(string Id)
        {
            if (Id == null) { return 0; }
            Plan tempPlan = _GhDbContext.dsPlans.Find(Id);
            if (tempPlan == null)
            {
                return 0;
            }
            _GhDbContext.dsPlans.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
