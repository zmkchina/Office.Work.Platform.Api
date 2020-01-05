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

        public async Task<ModelSettingServer> GetOneByIdAsync(int P_Id)
        {
            return await _ghDbContext.ServerSetting.FindAsync(P_Id);
        }

        public async Task<int> UpdateAsync(ModelSettingServer P_Entity)
        {
            _ghDbContext.ServerSetting.Update(P_Entity);
            return await _ghDbContext.SaveChangesAsync();
        }
    }
}
