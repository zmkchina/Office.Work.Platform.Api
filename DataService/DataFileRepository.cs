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
            return await _ghDbContext.Files.ToListAsync();
        }
        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<ModelFile> GetOneByIdAsync(string P_Id)
        {
            return await _ghDbContext.Files.FindAsync(P_Id);
        }
        /// <summary>
        /// 获取所有指定类型、指定宿主Id的文件例表。比如：GetOwnerFiles("计划附件","201900001")
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelFile>> GetEntitiesAsync(MSearchFile mSearchFile)
        {
            IQueryable<ModelFile> Items = _ghDbContext.Files as IQueryable<ModelFile>;
            //设定有文件读取权限的用户
            if (!string.IsNullOrWhiteSpace(mSearchFile.CanReadUserId))
            {
                Items = Items.Where(e => e.ReadGrant.Contains(mSearchFile.CanReadUserId));
            }
            if (!string.IsNullOrWhiteSpace(mSearchFile.OwnerId))
            {
                Items = Items.Where(e => e.OwnerId.Equals(mSearchFile.OwnerId));
            }
            if (!string.IsNullOrWhiteSpace(mSearchFile.OwnerType))
            {
                Items = Items.Where(e => e.OwnerType.Equals(mSearchFile.OwnerType));
            }
            if (!string.IsNullOrWhiteSpace(mSearchFile.KeysInMultiple))
            {
                Items = Items.Where(e => e.Name.Contains(mSearchFile.KeysInMultiple) || e.Describe.Contains(mSearchFile.KeysInMultiple));
            }
            return await Items.ToListAsync();
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(ModelFile P_Entity)
        {
            if (P_Entity == null) return 0;
            bool IsExist = _ghDbContext.Files.Count(e => e.Id == P_Entity.Id) > 0;
            if (IsExist)
            {
                return -2;
            }
            _ghDbContext.Files.Add(P_Entity);
            return await _ghDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ModelFile P_Entity)
        {
            _ghDbContext.Files.Update(P_Entity);
            return await _ghDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string P_Id)
        {
            ModelFile tempPlan = _ghDbContext.Files.Find(P_Id);
            _ghDbContext.Files.Remove(tempPlan);

            return await _ghDbContext.SaveChangesAsync();
        }
    }
}
