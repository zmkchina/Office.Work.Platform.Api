using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Api.AppCodes;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class DataMemberRepository
    {
        private readonly GHDbContext _ghDbContext;
        public DataMemberRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelMember>> GetAllAsync()
        {
            return await _ghDbContext.Members.ToListAsync();
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<ModelMember> GetOneByIdAsync(string P_Id)
        {
            return await _ghDbContext.Members.FindAsync(P_Id);
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，则更新之。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(ModelMember P_Entity)
        {
            bool IsExist = _ghDbContext.Members.Count(e => e.Id == P_Entity.Id) > 0;
            if (IsExist)
            {
                _ghDbContext.Members.Update(P_Entity);
            }
            else
            {
                _ghDbContext.Members.Add(P_Entity);
            }
            return await _ghDbContext.SaveChangesAsync();

        }

        /// <summary>
        /// 向数据库表添加一批记录。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddRangeAsync(List<ModelMember> P_Entitys)
        {
            if (P_Entitys != null && P_Entitys.Count > 0)
            {
                _ghDbContext.Members.AddRange(P_Entitys);
                return await _ghDbContext.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ModelMember P_Entity)
        {
            _ghDbContext.Members.Update(P_Entity);
            return await _ghDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<ModelMember>> GetEntitiesAsync(MSearchMember mSearchMember)
        {
            IQueryable<ModelMember> Items = _ghDbContext.Members as IQueryable<ModelMember>;

            if (!string.IsNullOrWhiteSpace(mSearchMember.Name))
            {
                Items = Items.Where(e => e.Name.Contains(mSearchMember.Name));
            }
            if (!string.IsNullOrWhiteSpace(mSearchMember.EducationTop))
            {
                Items = Items.Where(e => e.EducationTop.Contains(mSearchMember.EducationTop));
            }
            if (!string.IsNullOrWhiteSpace(mSearchMember.TechnicalTitle))
            {
                Items = Items.Where(e => e.TechnicalTitle.Contains(mSearchMember.TechnicalTitle));
            }
            return await Items.ToListAsync();
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string P_Id)
        {
            ModelMember tempPlan = _ghDbContext.Members.Find(P_Id);
            _ghDbContext.Members.Remove(tempPlan);
            return await _ghDbContext.SaveChangesAsync();
        }
    }
}
