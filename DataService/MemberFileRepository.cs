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
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberFile Entity)
        {
            if (Entity == null) return 0;
            bool IsExist = await _ghDbContext.dsMemberFiles.FirstOrDefaultAsync(e => e.Id == Entity.Id).ConfigureAwait(false) != null;
            if (IsExist)
            {
                return -2;
            }
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
