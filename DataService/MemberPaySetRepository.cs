using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberPaySetRepository
    {
        private readonly GHDbContext _GhDbContext;
        public MemberPaySetRepository(GHDbContext ghDbContext)
        {
            _GhDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPaySet>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberPaySet.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberPaySet> GetOneByIdAsync(string MemberId)
        {
            return await _GhDbContext.dsMemberPaySet.FindAsync(MemberId).ConfigureAwait(false);
        }
        private MemberPaySet GetPaySet(MemberPaySet x, Member y)
        {
            x.MemberName = y.Name;//拷贝名称
            return x;
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPaySet>> GetEntitiesAsync(MemberPaySetSearch SearchCondition)
        {
            IQueryable<MemberPaySet> Items = _GhDbContext.dsMemberPaySet.Join(_GhDbContext.dsMembers, x => x.MemberId, k => k.Id, (x, k) => new MemberPaySet
            {
                PayUnitName = x.PayUnitName,
                MemberId = x.MemberId,
                MemberName = k.Name,
                MemberType=k.MemberType,
                OrderIndex =k.OrderIndex,
                MemberUnitName = k.UnitName,
                PayItemNames = x.PayItemNames,
                UserId = x.UserId,
                UpDateTime = x.UpDateTime
            }) as IQueryable<MemberPaySet>;

            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.MemberId))
                {
                    Items = Items.Where(e => e.MemberId.Equals(SearchCondition.MemberId, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.PayUnitName))
                {
                    //return await _GhDbContext.dsMemberPaySet.Join(_GhDbContext.dsMembers, x => x.MemberId, k => k.Id, (x, k) => new
                    //MemberPaySet
                    //{
                    //    MemberId = x.MemberId,
                    //    MemberName = k.Name,
                    //    PayItemNames = x.PayItemNames,
                    //    UnitName = x.UnitName,
                    //    UserId = x.UserId,
                    //    UpDateTime = x.UpDateTime
                    //}).Where(e => e.UnitName.Equals(SearchCondition.UnitName, StringComparison.Ordinal)).Distinct().ToListAsync().ConfigureAwait(false);

                    Items = Items.Where(e => e.PayUnitName.Equals(SearchCondition.PayUnitName, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                return await Items.Distinct().ToListAsync().ConfigureAwait(false);
            }
            return new List<MemberPaySet>();
        }
        /// <summary>
        /// 单个新增或者更新数据信息（如数据库中没有则新增之，如有则更新之）
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(List<MemberPaySet> PaySetList)
        {
            if (PaySetList != null && PaySetList.Count > 0)
            {
                foreach (MemberPaySet item in PaySetList)
                {
                    if (await _GhDbContext.dsMemberPaySet.AnyAsync(x => x.MemberId == item.MemberId).ConfigureAwait(false))
                    {
                        _GhDbContext.dsMemberPaySet.Update(item);
                    }
                    else
                    {
                        _GhDbContext.dsMemberPaySet.Add(item);
                    }
                    item.UpDateTime = DateTime.Now;
                }

                return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            return -1;
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberPaySet PEntity)
        {
            if (PEntity == null || await _GhDbContext.dsMemberPaySet.AnyAsync(x => x.MemberId == PEntity.MemberId).ConfigureAwait(false))
            {
                return -2;
            }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberPaySet.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }


        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberPaySet PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberPaySet.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string MemberId)
        {
            MemberPaySet tempPlan = _GhDbContext.dsMemberPaySet.Find(MemberId);
            _GhDbContext.dsMemberPaySet.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
