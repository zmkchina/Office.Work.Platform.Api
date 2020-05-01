using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System;
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
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<Note>> GetEntitiesAsync(NoteSearch SearchCondition)
        {
            IQueryable<Note> Items = _GhDbContext.dsNotes as IQueryable<Note>;
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(SearchCondition.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.UserId))
                {
                    Items = Items.Where(e => e.UserId.Equals(SearchCondition.UserId, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.Type))
                {
                    Items = Items.Where(e => e.Type.Equals(SearchCondition.Type, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.KeysInMultiple))
                {
                    Items = Items.Where(e => e.Caption.Contains(SearchCondition.KeysInMultiple, StringComparison.Ordinal)|| e.Content.Contains(SearchCondition.KeysInMultiple, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }

                return await Items.ToListAsync().ConfigureAwait(false);
            }
            return new List<Note>();
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(Note PEntity)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsNotes.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }


        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Note PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsNotes.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // <summary>
        /// 根据Id删除一个实体信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string Id)
        {
            if (Id == null) { return 0; }
            Note tempPlan = _GhDbContext.dsNotes.Find(Id);
            _GhDbContext.dsNotes.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
