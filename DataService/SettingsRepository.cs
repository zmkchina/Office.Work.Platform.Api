using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class SettingsRepository
    {
        private readonly GHDbContext _ghDbContext;
        public SettingsRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNew(SettingServer Entity)
        {
            if (Entity == null) return 0;
            bool IsExist = await _ghDbContext.dsServerSetting.AnyAsync().ConfigureAwait(false);
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.dsServerSetting.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<SettingServer> ReadAsync()
        {
            return await _ghDbContext.dsServerSetting.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(SettingServer Entity)
        {
            _ghDbContext.dsServerSetting.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
