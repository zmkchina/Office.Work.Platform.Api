using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberPayRepository
    {
        private readonly GHDbContext _GhDbContext;
        public MemberPayRepository(GHDbContext ghDbContext)
        {
            _GhDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPay>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberPay.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberPay> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMemberPay.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberPay>> GetEntitiesAsync(MemberPaySearch SearchCondition)
        {
            IQueryable<MemberPay> Items = _GhDbContext.dsMemberPay.AsNoTracking() as IQueryable<MemberPay>;
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (SearchCondition.PayYear > 0)
                {
                    Items = Items.Where(e => e.PayYear == SearchCondition.PayYear);
                }
                if (SearchCondition.PayMonth > 0)
                {
                    Items = Items.Where(e => e.PayMonth == SearchCondition.PayMonth);
                }

                if (!string.IsNullOrWhiteSpace(SearchCondition.PayName))
                {
                    Items = Items.Where(e => e.PayName.Equals(SearchCondition.PayName, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.PayUnitName))
                {
                    Items = Items.Where(e => e.PayUnitName.Equals(SearchCondition.PayUnitName, StringComparison.Ordinal));//查询发放单位。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(SearchCondition.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.MemberId))
                {
                    Items = Items.Where(e => e.MemberId.Equals(SearchCondition.MemberId, StringComparison.Ordinal));//查询职工身份证号。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Remark))
                {
                    Items = Items.Where(e => e.Remark.Contains(SearchCondition.Remark, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<MemberPay>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberPay PEntity)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberPay.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }


        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberPay PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberPay.Update(PEntity);
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

            MemberPay tempPlan = _GhDbContext.dsMemberPay.Find(Id);
            _GhDbContext.dsMemberPay.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
