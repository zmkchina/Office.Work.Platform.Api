﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberPrizePunishRepository
    {
        private readonly GHDbContext _GhDbContext;
        private readonly IMapper _Imaper;

        public MemberPrizePunishRepository(GHDbContext GhDbContext, IMapper mapper)
        {
            _GhDbContext = GhDbContext;
            _Imaper = mapper;
        }

        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPrizePunishEntity>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberPrizePunish.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberPrizePunishEntity> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMemberPrizePunish.FindAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<List<MemberPrizePunishDto>> GetEntitiesAsync(MemberPrizePunishSearch SearchCondition)
        {
            IQueryable<MemberPrizePunishEntity> Items = _GhDbContext.dsMemberPrizePunish.AsNoTracking() as IQueryable<MemberPrizePunishEntity>;
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(SearchCondition.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.MemberId))
                {
                    Items = Items.Where(e => e.MemberId.Equals(SearchCondition.MemberId, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.PrizrOrPunishType))
                {
                    Items = Items.Where(e => SearchCondition.PrizrOrPunishType.Contains(e.PrizrOrPunishType, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Remark))
                {
                    Items = Items.Where(e => e.Remark.Contains(SearchCondition.Remark, StringComparison.Ordinal));
                }
                List<MemberPrizePunishEntity> Entities = await Items.ToListAsync().ConfigureAwait(false);
                return _Imaper.Map<List<MemberPrizePunishDto>>(Entities);
            }
            return new List<MemberPrizePunishDto>();
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberPrizePunishEntity PEntity)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberPrizePunish.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberPrizePunishEntity PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberPrizePunish.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            if (Id == null) { return 0; }
            MemberPrizePunishEntity tempPlan = _GhDbContext.dsMemberPrizePunish.Find(Id);
            _GhDbContext.dsMemberPrizePunish.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
