using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Api.AppCodes;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class DataPlanRepository
    {
        private readonly GHDbContext _ghDbContext;
        public DataPlanRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelPlan>> GetAllAsync()
        {
            return await _ghDbContext.Plans.ToListAsync();
        }
        public async Task<ModelPlan> GetOneByIdAsync(string P_Id)
        {
            return await _ghDbContext.Plans.FindAsync(P_Id);
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，则更新之。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(ModelPlan P_Entity)
        {
            bool IsExist = _ghDbContext.Plans.Count<ModelPlan>(e => e.Id == P_Entity.Id) > 0;
            if (IsExist)
            {
                _ghDbContext.Plans.Update(P_Entity);
                //_ghDbContext.Entry(P_Entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                _ghDbContext.Plans.Add(P_Entity);
            }
            return await _ghDbContext.SaveChangesAsync();

        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ModelPlan P_Entity)
        {
            _ghDbContext.Plans.Update(P_Entity);
            return await _ghDbContext.SaveChangesAsync();
        }


        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchPlan">计划查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<ModelPlan>> GetEntitiesAsync(MSearchPlan mSearchPlan)
        {
            IQueryable<ModelPlan> Items = _ghDbContext.Plans as IQueryable<ModelPlan>;

            if (!string.IsNullOrWhiteSpace(mSearchPlan.CreateUserId))
            {
                Items = Items.Where(e => e.CreateUserId.Equals(mSearchPlan.CreateUserId));
            }
            if (!string.IsNullOrWhiteSpace(mSearchPlan.CurrectState))
            {
                Items = Items.Where(e => mSearchPlan.CurrectState.Contains(e.CurrectState));
            }
            if (!string.IsNullOrWhiteSpace(mSearchPlan.KeysInMultiple))
            {
                Items = Items.Where(e => e.Caption.Contains(mSearchPlan.KeysInMultiple) || e.Content.Contains(mSearchPlan.KeysInMultiple));
            }

            return await Items.ToListAsync();
        }

        public async Task<int> DeleteAsync(string P_Id)
        {
            ModelPlan tempPlan = _ghDbContext.Plans.Find(P_Id);
            if (tempPlan == null) {
                return 0;
            }
            _ghDbContext.Plans.Remove(tempPlan);
            return await _ghDbContext.SaveChangesAsync();
        }
    }
}
