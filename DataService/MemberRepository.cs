using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberRepository
    {
        private readonly GHDbContext _GhDbContext;
        public MemberRepository(GHDbContext GhDbContext)
        {
            _GhDbContext = GhDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _GhDbContext.dsMembers.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<Member> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMembers.FindAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<Member>> GetEntitiesAsync(MemberSearch mSearchMember)
        {
            IQueryable<Member> Items = _GhDbContext.dsMembers as IQueryable<Member>;
            if (mSearchMember != null)
            {
                if (!string.IsNullOrWhiteSpace(mSearchMember.Id))
                {
                    Items = Items.Where(e => e.Id.Trim().Equals(mSearchMember.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(mSearchMember.Name))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchMember.Name, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
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
                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<Member>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，由返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(Member PEntity)
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
        public async Task<int> UpdateAsync(Member PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMembers.Update(PEntity);
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
            Member tempPlan = _GhDbContext.dsMembers.Find(Id);
            _GhDbContext.dsMembers.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
