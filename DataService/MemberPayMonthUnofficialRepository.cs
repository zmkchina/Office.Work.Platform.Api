﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberPayMonthUnofficialRepository
    {
        private readonly GHDbContext _GhDbContext;
        public MemberPayMonthUnofficialRepository(GHDbContext ghDbContext)
        {
            _GhDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPayMonthUnofficial>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberPayMonthUnofficial.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberPayMonthUnofficial> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMemberPayMonthUnofficial.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPayMonthUnofficial>> GetEntitiesAsync(MemberPayMonthUnofficialSearch SearchCondition)
        {
            IQueryable<MemberPayMonthUnofficial> Items = _GhDbContext.dsMemberPayMonthUnofficial as IQueryable<MemberPayMonthUnofficial>;
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
            return new List<MemberPayMonthUnofficial>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberPayMonthUnofficial PEntity)
        {
            if (PEntity==null || PEntity.Id!=null)
            {
                return -2;
            }
            PEntity.Id=AppCodes.AppStaticClass.GetIdOfDateTime();
            _GhDbContext.dsMemberPayMonthUnofficial.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 向数据库表添加一批记录。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddRangeAsync(List<MemberPayMonthUnofficial> Entitys)
        {
            if (Entitys != null && Entitys.Count > 0)
            {
                _GhDbContext.dsMemberPayMonthUnofficial.AddRange(Entitys);
                return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
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
        public async Task<int> UpdateAsync(MemberPayMonthUnofficial Entity)
        {
            _GhDbContext.dsMemberPayMonthUnofficial.Update(Entity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            MemberPayMonthUnofficial tempPlan = _GhDbContext.dsMemberPayMonthUnofficial.Find(Id);
            _GhDbContext.dsMemberPayMonthUnofficial.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
   
}
