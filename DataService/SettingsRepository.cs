using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class SettingsRepository
    {
        private readonly GHDbContext _ghDbContext;
        private readonly IMapper _Mapper;
        public SettingsRepository(GHDbContext ghDbContext, IMapper mapper)
        {
            _ghDbContext = ghDbContext;
            _Mapper = mapper;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> AddNew(SettingServerEntity Entity)
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
        public async Task<SettingServerDto> ReadAsync()
        {

            var SettingServerOneEntity= await _ghDbContext.dsServerSetting.FirstOrDefaultAsync().ConfigureAwait(false);
            return _Mapper.Map<SettingServerDto>(SettingServerOneEntity);
        }

        public async Task<int> UpdateAsync(SettingServerEntity Entity)
        {
            _ghDbContext.dsServerSetting.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
