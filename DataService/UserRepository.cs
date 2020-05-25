using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class UserRepository
    {
        private readonly GHDbContext _GhDbContext;
        public UserRepository(GHDbContext ghDbContext)
        {
            _GhDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有用户数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _GhDbContext.dsUsers.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsUsers.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id、Pwd 查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User> GetOneByIdPwdAsync(string Id, string Pwd)
        {
            User user = await _GhDbContext.dsUsers.FindAsync(Id).ConfigureAwait(false);
            if (user != null && user.PassWord.Equals(Pwd, System.StringComparison.Ordinal))
            {
                return user;
            }
            return null;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(User PEntity)
        {
            if (PEntity == null ) return 0;
            bool IsExist = await _GhDbContext.dsUsers.AnyAsync(e => e.Id == PEntity.Id).ConfigureAwait(false);
            if (IsExist)
            {
                return -2;
            }
            _GhDbContext.dsUsers.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(User PEntity)
        {
            if (PEntity == null) { return 0; }
            _GhDbContext.dsUsers.Update(PEntity);
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
            User tempPlan = _GhDbContext.dsUsers.Find(Id);
            _GhDbContext.dsUsers.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
