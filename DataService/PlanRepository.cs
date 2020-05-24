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
        /// <param name="SearchCondition">计划查询类对象</param>
        /// <returns></returns>
        public async Task<PlanSearchResult> GetEntitiesAsync(PlanSearch SearchCondition)
        {
            PlanSearchResult SearchResult = new PlanSearchResult();
            IQueryable<Plan> Items = _GhDbContext.dsPlans.AsNoTracking() as IQueryable<Plan>;
            if (SearchCondition != null)
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.CreateUserId))
                {
                    Items = Items.Where(e => e.CreateUserId.Equals(SearchCondition.CreateUserId, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.UnitName))
                {
                    Items = Items.Where(e => e.UnitName.Equals(SearchCondition.UnitName, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.ResponsiblePerson))
                {
                    Items = Items.Where(e => e.ResponsiblePerson.Equals(SearchCondition.ResponsiblePerson, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.CurrectState))
                {
                    Items = Items.Where(e => SearchCondition.CurrectState.Contains(e.CurrectState, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Helpers))
                {
                    Items = Items.Where(e => e.Helpers.Contains(SearchCondition.Helpers, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.KeysInMultiple))
                {
                    Items = Items.Where(e => e.Caption.Contains(SearchCondition.KeysInMultiple, StringComparison.Ordinal) || e.Content.Contains(SearchCondition.KeysInMultiple, StringComparison.Ordinal));
                }
                SearchResult.SearchCondition.RecordCount = await Items.CountAsync().ConfigureAwait(false);
                SearchResult.RecordList = await Items.OrderByDescending(x => x.UpDateTime).Skip((SearchCondition.PageIndex - 1) * SearchCondition.PageSize).Take(SearchCondition.PageSize).ToListAsync().ConfigureAwait(false);
            }
            return SearchResult;
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
