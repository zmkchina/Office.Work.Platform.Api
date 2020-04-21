using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class DataFileRepository
    {
        private readonly GHDbContext _ghDbContext;
        public DataFileRepository(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelFile>> GetAllAsync()
        {
            return await _ghDbContext.Files.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<ModelFile> GetOneByIdAsync(string Id)
        {
            return await _ghDbContext.Files.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 获取所有指定类型、指定宿主Id的文件例表。比如：GetOwnerFiles("计划附件","201900001")
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelFile>> GetEntitiesAsync(MSearchFile mSearchFile)
        {
            IQueryable<ModelFile> Items = _ghDbContext.Files as IQueryable<ModelFile>;
            //设定有文件读取权限的用户
            if (mSearchFile != null)
            {
                if (!string.IsNullOrWhiteSpace(mSearchFile.CanReadUserId))
                {
                    Items = Items.Where(e => e.ReadGrant.Contains(mSearchFile.CanReadUserId, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.OwnerId))
                {
                    Items = Items.Where(e => e.OwnerId.Equals(mSearchFile.OwnerId, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.OwnerType))
                {
                    Items = Items.Where(e => e.OwnerType.Equals(mSearchFile.OwnerType, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.KeysInMultiple))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchFile.KeysInMultiple, System.StringComparison.Ordinal) || e.Describe.Contains(mSearchFile.KeysInMultiple, System.StringComparison.Ordinal));
                }
            }
            return await Items.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(ModelFile Entity)
        {
            if (Entity == null) return 0;
            bool IsExist = await _ghDbContext.Files.FirstOrDefaultAsync(e => e.Id == Entity.Id).ConfigureAwait(false) != null;
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.Files.Add(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ModelFile Entity)
        {
            _ghDbContext.Files.Update(Entity);
            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            ModelFile tempPlan = _ghDbContext.Files.Find(Id);
            _ghDbContext.Files.Remove(tempPlan);

            return await _ghDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
