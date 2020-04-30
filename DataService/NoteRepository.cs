using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class NoteRepository
    {
        private readonly GHDbContext _GhDbContext;
        public NoteRepository(GHDbContext ghDbContext)
        {
            _GhDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _GhDbContext.dsNotes.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Note> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsNotes.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNew(Note Entity)
        {
            if (Entity == null || Entity.Id!=null)
            {
                return -2;
            }
            Entity.Id= AppCodes.AppStaticClass.GetIdOfDateTime();
            _GhDbContext.dsNotes.Add(Entity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
