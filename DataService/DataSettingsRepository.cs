using Office.Work.Platform.Lib;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class DataSettingsRepository
    {
        private readonly GHDbContext _ghDbContext;
        public DataSettingsRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }

        public async Task<ModelSettingServer> GetOneByIdAsync(int Id)
        {
            return await _ghDbContext.ServerSetting.FindAsync(Id).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(ModelSettingServer Entity)
        {
            _ghDbContext.ServerSetting.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
