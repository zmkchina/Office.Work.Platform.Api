using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberFileRepository
    {
        private readonly GHDbContext _GhDbContext;
        public MemberFileRepository(GHDbContext GhDbContext)
        {
            _GhDbContext = GhDbContext;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberFile>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberFiles.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberFile> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMemberFiles.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 按指定的条件查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberFile>> GetEntitiesAsync(MemberFileSearch mSearchFile)
        {
            IQueryable<MemberFile> Items = _GhDbContext.dsMemberFiles.AsNoTracking() as IQueryable<MemberFile>;
            //需要连同该文件的Plan信息一同读取，在操作文件时需使用之。
            if (mSearchFile != null && !string.IsNullOrWhiteSpace(mSearchFile.UserId))
            {
                //判断请求用户是否有权限(必须对该文件所属计划有读取权限)
                Items = Items.Where(e => e.CanReadUserIds == null || e.UserId.Equals(mSearchFile.UserId, System.StringComparison.Ordinal) || e.CanReadUserIds.Contains(mSearchFile.UserId, System.StringComparison.Ordinal));
                if (!string.IsNullOrWhiteSpace(mSearchFile.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(mSearchFile.Id, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.Name))
                {
                    Items = Items.Where(e => e.Name.Equals(mSearchFile.Name, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.MemberId))
                {
                    Items = Items.Where(e => e.MemberId.Equals(mSearchFile.MemberId, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.ContentType))
                {
                    Items = Items.Where(e => e.ContentType.Equals(mSearchFile.ContentType, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.DispatchUnit))
                {
                    Items = Items.Where(e => e.DispatchUnit.Contains(mSearchFile.DispatchUnit, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.FileNumber))
                {
                    Items = Items.Where(e => e.FileNumber.Contains(mSearchFile.FileNumber, System.StringComparison.Ordinal));
                }
                if (!string.IsNullOrWhiteSpace(mSearchFile.SearchNameOrDesc))
                {
                    Items = Items.Where(e => e.Name.Contains(mSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal) || e.Describe.Contains(mSearchFile.SearchNameOrDesc, System.StringComparison.Ordinal));
                }
                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<MemberFile>();
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2。
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberFile PEntity, string FileId)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = FileId;
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberFiles.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberFile PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberFiles.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdAsync(string FileBaseDir, string Id)
        {
            int DelCount = 0;
            if (Id == null) { return 0; }
            MemberFile CurFile = _GhDbContext.dsMemberFiles.Find(Id);
            _GhDbContext.dsMemberFiles.Remove(CurFile);
            DelCount = await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
            if (DelCount > 0)
            {
                //删除硬盘上的文件
                var fileName = Path.Combine(FileBaseDir, $"{CurFile.Id}{CurFile.ExtendName}");
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
            }
            return DelCount;
        }
        /// <summary>
        /// 根据该文件的拥有者Id删除所有文件。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByOwnerIdAsync(string FileBaseDir, string MemberId)
        {
            if (MemberId == null) { return 0; }
            int DelCount = 0;
            List<MemberFile> PlanFiles = await _GhDbContext.dsMemberFiles.Where(x => x.MemberId.Equals(MemberId, StringComparison.Ordinal)).ToListAsync().ConfigureAwait(false);
            for (int i = 0; i < PlanFiles.Count; i++)
            {
                _GhDbContext.dsMemberFiles.Remove(PlanFiles[i]);
                int DelFlag = await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
                if (DelFlag > 0)
                {
                    //删除硬盘上的文件
                    var fileName = Path.Combine(FileBaseDir, $"{PlanFiles[i].Id}{PlanFiles[i].ExtendName}");
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    DelCount++;
                }
            }
            return DelCount;
        }
    }
}
