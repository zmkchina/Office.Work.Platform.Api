using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class FileDocRepository
    {
        private readonly GHDbContext _GhDbContext;
        public FileDocRepository(GHDbContext GhDbContext)
        {
            _GhDbContext = GhDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FileDoc>> GetAllAsync()
        {
            return await _GhDbContext.dsFileDocs.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<FileDoc> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsFileDocs.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 按指定的条件查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FileDoc>> GetEntitiesAsync(FileDocSearch mSearchFile)
        {
            IQueryable<FileDoc> Items = _GhDbContext.dsFileDocs as IQueryable<FileDoc>;
            //需要连同该文件的Plan信息一同读取，在操作文件时需使用之。
            if (mSearchFile != null && !string.IsNullOrWhiteSpace(mSearchFile.UserId))
            {
                //判断请求用户是否有权限(必须对该文件所属计划有读取权限)
                Items = Items.Where(e => e.CanReadUserIds == null || e.UserId.Equals(mSearchFile.UserId, System.StringComparison.Ordinal) || e.CanReadUserIds.Contains(mSearchFile.UserId, System.StringComparison.Ordinal));
                if (!string.IsNullOrWhiteSpace(mSearchFile.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(mSearchFile.Id, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.OwnerId))
                {
                    Items = Items.Where(e => e.OwnerId.Equals(mSearchFile.OwnerId, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.OwnerType))
                {
                    Items = Items.Where(e => e.OwnerType.Equals(mSearchFile.OwnerType, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.ContentType))
                {
                    Items = Items.Where(e => e.ContentType.Equals(mSearchFile.ContentType, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.DispatchUnit))
                {
                    Items = Items.Where(e => e.DispatchUnit.Contains(mSearchFile.DispatchUnit, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.SearchNameOrDesc))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal) || e.Describe.Contains(mSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal));
                }
                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<FileDoc>();
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(FileDoc PEntity, string FileId)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = FileId;
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsFileDocs.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(FileDoc PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsFileDocs.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            if (Id == null) { return 0; }
            FileDoc tempPlan = _GhDbContext.dsFileDocs.Find(Id);
            _GhDbContext.dsFileDocs.Remove(tempPlan);

            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
