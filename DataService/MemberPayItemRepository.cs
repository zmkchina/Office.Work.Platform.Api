﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberPayItemRepository
    {
        private readonly GHDbContext _GhDbContext;
        private readonly IMapper _Mapper;
        public MemberPayItemRepository(GHDbContext ghDbContext, IMapper mapper)
        {
            _GhDbContext = ghDbContext;
            _Mapper = mapper;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberPayItemDto>> GetAllAsync()
        {
            var Entities= await _GhDbContext.dsMemberPayItem.ToListAsync().ConfigureAwait(false);
            return _Mapper.Map<List<MemberPayItemDto>>(Entities);
        }

        /// <summary>
        /// 根据Name获取一个对象,[Name]为Key
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberPayItemDto> GetOneByIdAsync(string Name)
        {
            var Entity= await _GhDbContext.dsMemberPayItem.FindAsync(Name).ConfigureAwait(false);
            return _Mapper.Map<MemberPayItemDto>(Entity);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<List<MemberPayItemDto>> GetEntitiesAsync(MemberPayItemSearch SearchCondition)
        {
            IQueryable<MemberPayItemEntity> Items = _GhDbContext.dsMemberPayItem.AsNoTracking() as IQueryable<MemberPayItemEntity>;
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.Name))
                {
                    Items = Items.Where(e => e.Name.Equals(SearchCondition.Name, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.PayUnitName))
                {
                    Items = Items.Where(e => e.UnitName.Equals(SearchCondition.PayUnitName, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }

                if (!string.IsNullOrWhiteSpace(SearchCondition.InTableType))
                {
                    Items = Items.Where(e => e.InTableType.Equals(SearchCondition.InTableType, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.InCardinality))
                {
                    Items = Items.Where(e => e.InCardinality.Equals(SearchCondition.InCardinality, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Name))
                {
                    Items = Items.Where(e => e.Name.Equals(SearchCondition.Name, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Remark))
                {
                    Items = Items.Where(e => e.Remark.Contains(SearchCondition.Remark, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                var Entities = await Items.ToListAsync().ConfigureAwait(false);
                return _Mapper.Map<List<MemberPayItemDto>>(Entities);
            }
            return new List<MemberPayItemDto>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberPayItemEntity PEntity)
        {
            if (PEntity == null || await _GhDbContext.dsMemberPayItem.AnyAsync(x => x.Name.Equals(PEntity.Name, StringComparison.Ordinal)).ConfigureAwait(false))
            {
                return -2;
            }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberPayItem.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }


        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberPayItemEntity PEntity)
        {
            if (PEntity == null) { return 0; }
            _GhDbContext.dsMemberPayItem.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Name)
        {
            if (Name == null) { return 0; }

            MemberPayItemEntity tempPlan = _GhDbContext.dsMemberPayItem.Find(Name);
            _GhDbContext.dsMemberPayItem.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }

}
