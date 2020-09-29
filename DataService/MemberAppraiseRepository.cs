using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberAppraiseRepository
    {
        private readonly GHDbContext _GhDbContext;
        private readonly IMapper _Mapper;
        public MemberAppraiseRepository(GHDbContext GhDbContext, IMapper mapper)
        {
            _GhDbContext = GhDbContext;
            _Mapper = mapper;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberAppraiseDto>> GetAllAsync()
        {
            var Entities = await _GhDbContext.dsMemberAppraise.ToListAsync().ConfigureAwait(false);

            return _Mapper.Map<List<MemberAppraiseDto>>(Entities);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberAppraiseDto> GetOneByIdAsync(string Id)
        {
            var Entity = await _GhDbContext.dsMemberAppraise.FindAsync(Id).ConfigureAwait(false);

            return _Mapper.Map<MemberAppraiseDto>(Entity);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<List<MemberAppraiseDto>> GetEntitiesAsync(MemberAppraiseSearch SearchCondition)
        {
            IQueryable<MemberAppraiseEntity> Items = _GhDbContext.dsMemberAppraise.AsNoTracking() as IQueryable<MemberAppraiseEntity>;
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
                if (!string.IsNullOrWhiteSpace(SearchCondition.Year))
                {
                    Items = Items.Where(e => SearchCondition.Year.Equals(e.Year, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Result))
                {
                    Items = Items.Where(e => SearchCondition.Result.Contains(e.Result, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Remark))
                {
                    Items = Items.Where(e => e.Remark.Contains(SearchCondition.Remark, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }

                var Entities = await Items.ToListAsync().ConfigureAwait(false);

                return _Mapper.Map<List<MemberAppraiseDto>>(Entities);
            }
            return new List<MemberAppraiseDto>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberAppraiseEntity PEntity)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberAppraise.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberAppraiseEntity PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberAppraise.Update(PEntity);
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
            MemberAppraiseEntity tempPlan = _GhDbContext.dsMemberAppraise.Find(Id);
            _GhDbContext.dsMemberAppraise.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
