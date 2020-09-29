using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberInfoRepository
    {
        private readonly GHDbContext _GhDbContext;
        private readonly IMapper _Imapper;

        public MemberInfoRepository(GHDbContext GhDbContext, IMapper mapper)
        {
            _GhDbContext = GhDbContext;
            _Imapper = mapper;
        }

        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberInfoEntity>> GetAllAsync()
        {
            return await _GhDbContext.dsMembers.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象 Entity
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberInfoEntity> GetMemberInfoEntityAsync(string Id)
        {
            return await _GhDbContext.dsMembers.FindAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象 Dto
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberInfoDto> GetMemberInfoDtoAsync(string Id)
        {
            MemberInfoEntity RecordEntity = await _GhDbContext.dsMembers.FindAsync(Id).ConfigureAwait(false);

            return _Imapper.Map<MemberInfoDto>(RecordEntity);
        }

        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberInfoDto>> GetEntitiesAsync(MemberInfoSearch mSearchMember)
        {
            IQueryable<MemberInfoEntity> Items = _GhDbContext.dsMembers.AsNoTracking() as IQueryable<MemberInfoEntity>;
            if (mSearchMember != null)
            {
                if (!string.IsNullOrWhiteSpace(mSearchMember.Id))
                {
                    Items = Items.Where(e => e.Id.Trim().Equals(mSearchMember.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.Name))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchMember.Name, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.UnitName))
                {
                    Items = Items.Where(e => e.UnitName.Contains(mSearchMember.UnitName, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.Age))
                {
                    if (int.TryParse(mSearchMember.Age, out int Age))
                    {
                        Items = Items.Where(e => DateTime.Now.Year - e.Birthday.Year > Age);
                    }
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.EducationTop))
                {
                    Items = Items.Where(e => e.EducationTop.Contains(mSearchMember.EducationTop, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.TechnicalTitle))
                {
                    Items = Items.Where(e => e.TechnicalTitle.Contains(mSearchMember.TechnicalTitle, StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.PoliticalStatus))
                {
                    Items = Items.Where(e => e.PoliticalStatus.Contains(mSearchMember.PoliticalStatus, StringComparison.Ordinal));
                }
                List<MemberInfoEntity> RecordEntities = await Items.ToListAsync().ConfigureAwait(false);
                return _Imapper.Map<List<MemberInfoDto>>(RecordEntities);
            }
            return new List<MemberInfoDto>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，由返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberInfoEntity PEntity)
        {
            //此记录的Id为员工的身份证号码，必须输入
            if (PEntity == null || PEntity.Id == null || PEntity.Name == null) { return 0; }
            bool IsExist = await _GhDbContext.dsMembers.AnyAsync(e => e.Id == PEntity.Id).ConfigureAwait(false);
            if (IsExist)
            {
                return -2;
            }
            else
            {
                PEntity.UpDateTime = DateTime.Now;
                _GhDbContext.dsMembers.Add(PEntity);
            }
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberInfoEntity PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMembers.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 新增或更新一个实体。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(MemberInfoEntity PEntity)
        {
            //此记录的Id为员工的身份证号码，必须输入
            if (PEntity == null || PEntity.Id == null || PEntity.Name == null) { return 0; }
            bool IsExist = await _GhDbContext.dsMembers.AnyAsync(e => e.Id == PEntity.Id).ConfigureAwait(false);
            if (IsExist)
            {
                return await UpdateAsync(PEntity).ConfigureAwait(false);
            }
            else
            {
                PEntity.UpDateTime = DateTime.Now;
                _GhDbContext.dsMembers.Add(PEntity);
            }
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
            MemberInfoEntity tempPlan = _GhDbContext.dsMembers.Find(Id);
            _GhDbContext.dsMembers.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
