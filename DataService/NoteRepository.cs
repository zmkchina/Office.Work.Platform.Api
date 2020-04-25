using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class NoteRepository
    {
        private readonly GHDbContext _ghDbContext;
        public NoteRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _ghDbContext.dsNotes.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Note> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.dsNotes.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNew(Note Entity)
        {
            if (Entity == null) return 0;
            bool IsExist =await _ghDbContext.dsNotes.FirstOrDefaultAsync(e => e.Id == Entity.Id).ConfigureAwait(false)!=null;
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.dsNotes.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
