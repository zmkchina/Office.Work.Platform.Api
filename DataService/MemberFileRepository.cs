using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberFileRepository
    {
        private readonly GHDbContext _ghDbContext;
        public MemberFileRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberFile>> GetAllAsync()
        {
            return await _ghDbContext.dsMemberFiles.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberFile> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.dsMemberFiles.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 按指定的条件查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberFile>> GetEntitiesAsync(MemberFileSearch mfSearchFile)
        {
            IQueryable<MemberFile> Items = _ghDbContext.dsMemberFiles.Include(x => x.Member) as IQueryable<MemberFile>;
            if (mfSearchFile != null && !string.IsNullOrWhiteSpace(mfSearchFile.UserId))
            {
                //TODO：判断请求用户是否有权限(必须对该文件所属计划有读取权限)

                if (!string.IsNullOrWhiteSpace(mfSearchFile.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(mfSearchFile.Id, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mfSearchFile.MemberId))
                {
                    Items = Items.Where(e => e.Member.Id.Equals(mfSearchFile.MemberId, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mfSearchFile.OtherRecordId))
                {
                    Items = Items.Where(e => e.OtherRecordId.Equals(mfSearchFile.OtherRecordId, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mfSearchFile.FileType))
                {
                    Items = Items.Where(e => e.FileType.Equals(mfSearchFile.FileType, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mfSearchFile.SearchNameOrDesc))
                {
                    Items = Items.Where(e => e.Name.Contains(mfSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal) || e.Describe.Contains(mfSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal));
                }
                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<MemberFile>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberFile Entity,string FileId)
        {
            if(Entity == null || Entity.Id != null)
            {
                return -2;
            }
            Entity.Id = FileId;
            _ghDbContext.dsMemberFiles.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberFile Entity)
        {
            _ghDbContext.dsMemberFiles.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            MemberFile tempPlan = _ghDbContext.dsMemberFiles.Find(Id);
            _ghDbContext.dsMemberFiles.Remove(tempPlan);

            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
