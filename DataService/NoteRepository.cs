using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Api.AutoMapperProfiles;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    /// <summary>
    /// 用户备忘信息查询类
    /// </summary>
    public class NoteRepository
    {
        private readonly GHDbContext _GhDbContext;
        private readonly IMapper _Mapper;
        public NoteRepository(GHDbContext ghDbContext, IMapper mapper)
        {
            _GhDbContext = ghDbContext;
            _Mapper = mapper;
        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NoteInfoDto>> GetAllAsync()
        {
            var NoteEntities = await _GhDbContext.dsNotes.ToListAsync().ConfigureAwait(false);
            var NoteDtos = _Mapper.Map<IEnumerable<NoteInfoDto>>(NoteEntities);
            return NoteDtos;
        }
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<NoteInfoDto> GetOneByIdAsync(string Id)
        {
            var NoteEntities = await _GhDbContext.dsNotes.FindAsync(Id).ConfigureAwait(false);
            var NoteDtos = _Mapper.Map<NoteInfoDto>(NoteEntities);
            return NoteDtos;
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<NoteInfoDtoPages> GetEntitiesAsync(NoteInfoSearch SearchCondition)
        {
            NoteInfoDtoPages SearchResult = new NoteInfoDtoPages();
            IQueryable<NoteInfoEntity> Items = _GhDbContext.dsNotes.AsNoTracking() as IQueryable<NoteInfoEntity>;
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                if (!string.IsNullOrWhiteSpace(SearchCondition.Id))
                {
                    Items = Items.Where(e => e.Id.Equals(SearchCondition.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (SearchCondition.IsMySelft.Equals("Yes", StringComparison.Ordinal))
                {
                    Items = Items.Where(e => e.UserId.Equals(SearchCondition.UserId, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                else
                {
                    Items = Items.Where(e => e.CanReadUserIds.Contains(SearchCondition.UserId, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                if (!string.IsNullOrWhiteSpace(SearchCondition.KeysInMultiple))
                {
                    Items = Items.Where(e => e.Caption.Contains(SearchCondition.KeysInMultiple, StringComparison.Ordinal) || e.TextContent.Contains(SearchCondition.KeysInMultiple, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
                }
                SearchResult.SearchCondition.RecordCount = await Items.CountAsync().ConfigureAwait(false);
                List<NoteInfoEntity> RecordEntities = await Items.OrderByDescending(x => x.UpDateTime).Skip((SearchCondition.PageIndex - 1) * SearchCondition.PageSize).Take(SearchCondition.PageSize).ToListAsync().ConfigureAwait(false);
                SearchResult.RecordList = _Mapper.Map<List<NoteInfoDto>>(RecordEntities);
            }
            return SearchResult;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(NoteInfoEntity PEntity)
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
        public async Task<int> UpdateAsync(NoteInfoEntity PEntity)
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
            NoteInfoEntity tempPlan = _GhDbContext.dsNotes.Find(Id);
            _GhDbContext.dsNotes.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
