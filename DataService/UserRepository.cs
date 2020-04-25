using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class UserRepository
    {
        private readonly GHDbContext _ghDbContext;
        public UserRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有用户数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _ghDbContext.dsUsers.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.dsUsers.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id、Pwd 查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User> GetOneByIdPwdAsync(string Id, string Pwd)
        {
            User user = await _ghDbContext.dsUsers.FindAsync(Id).ConfigureAwait(false);
            if (user != null && user.PassWord.Equals(Pwd, System.StringComparison.Ordinal))
            {
                return user;
            }
            return null;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNew(User Entity)
        {
            if (Entity == null) return 0;
            bool IsExist = await _ghDbContext.dsUsers.CountAsync(e => e.Id == Entity.Id).ConfigureAwait(false) > 0;
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.dsUsers.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
