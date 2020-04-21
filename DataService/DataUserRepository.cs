using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class DataUserRepository
    {
        private readonly GHDbContext _ghDbContext;
        public DataUserRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有用户数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelUser>> GetAllAsync()
        {
            return await _ghDbContext.Users.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ModelUser> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.Users.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id、Pwd 查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ModelUser> GetOneByIdPwdAsync(string Id,string Pwd)
        {
            ModelUser user= await _ghDbContext.Users.FindAsync(Id).ConfigureAwait(false);
            if (user.PassWord.Equals(Pwd,System.StringComparison.Ordinal))
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
        public async Task<int> AddNew(ModelUser Entity)
        {
            if (Entity == null) return 0;
            bool IsExist =await _ghDbContext.Users.FirstOrDefaultAsync(e => e.Id == Entity.Id).ConfigureAwait(false)!=null;
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.Users.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
