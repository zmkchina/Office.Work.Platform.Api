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
            return await _ghDbContext.Users.ToListAsync();
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<ModelUser> GetOneByIdAsync(string P_Id)
        {
            return await _ghDbContext.Users.FindAsync(P_Id);
        }
        /// <summary>
        /// 根据Id、Pwd 查询用户信息
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<ModelUser> GetOneByIdPwdAsync(string P_Id,string P_Pwd)
        {
            ModelUser user= await _ghDbContext.Users.FindAsync(P_Id);
            if (user.PassWord.Equals(P_Pwd))
            {
                return user;
            }
            return null;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNew(ModelUser P_Entity)
        {
            if (P_Entity == null) return 0;
            bool IsExist = _ghDbContext.Users.Count(e => e.Id == P_Entity.Id) > 0;
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.Users.Add(P_Entity);
            return await _ghDbContext.SaveChangesAsync();
        }
    }
}
