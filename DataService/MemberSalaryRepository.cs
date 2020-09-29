using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class MemberSalaryRepository
    {
        private readonly GHDbContext _GhDbContext;
        public MemberSalaryRepository(GHDbContext ghDbContext)
        {
            _GhDbContext = ghDbContext;
        }

        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MemberSalaryEntity>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberSalary.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberSalaryEntity> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMemberSalary.FindAsync(Id).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberSalaryDto>> GetEntitiesAsync(MemberSalarySearch SearchCondition)
        {
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                int SearchYear = SearchCondition.PayYear;
                int SearchMonth = SearchCondition.PayMonth;
                List<MemberSalaryEntity> SalaryList = await ReadSalarys(SearchCondition).ConfigureAwait(false);

                if (SalaryList == null || SalaryList.Count == 0)
                {
                    //如果未查到指定月份的数据，且要求填充空数据，则尝试读取上一个月的数据数据
                    if (SearchCondition.FillEmpty)
                    {
                        SearchCondition.PayYear = SearchCondition.PayMonth == 1 ? SearchCondition.PayYear - 1 : SearchCondition.PayYear;
                        SearchCondition.PayMonth = SearchCondition.PayMonth > 1 ? SearchCondition.PayMonth - 1 : 12;

                        SalaryList = await ReadSalarys(SearchCondition).ConfigureAwait(false);

                        if (SalaryList != null && SalaryList.Count > 0)
                        {
                            //查询到上一个月的数据
                            foreach (MemberSalaryEntity item in SalaryList)
                            {
                                item.Id = null;
                            }
                        }

                    }
                    else
                    {
                        //不要求填充空数据
                        return new List<MemberSalaryDto>();

                    }
                }

                //查询到了指定月份或指定月份上一个月的数据。
                if (SalaryList != null && SalaryList.Count > 0)
                {
                    //返回查到的数据
                    List<MemberSalaryDto> SearchResultList = SalaryList.Select(x => new MemberSalaryDto
                    {
                        Id = x.Id,
                        MemberId = x.Member.Id,
                        MemberName = x.Member.Name,
                        PayMonth = SearchMonth > 0 && SearchMonth != SearchCondition.PayMonth ? SearchMonth : x.PayMonth,
                        PayYear = SearchYear > 0 && SearchYear != SearchCondition.PayYear ? SearchYear : x.PayYear,
                        PayUnitName = SearchCondition.PayUnitName,
                        TableType = SearchCondition.TableType,
                        SalaryItems = JsonConvert.DeserializeObject<List<SalaryItem>>(x.NameAndAmount),
                        UserId = x.UserId,
                        Remark = x.Remark
                    }).ToList();
                    return SearchResultList;
                }

                //如果仍未查到(包括上一个月数据也没有），则填充空内容
                List<MemberSalaryDto> FillResultList = await _GhDbContext.dsMembers.AsNoTracking().OrderBy(x => x.OrderIndex)
                    .Where(x => x.MemberType.Equals(SearchCondition.MemberType, StringComparison.Ordinal) && x.UnitName.Equals(SearchCondition.PayUnitName, StringComparison.Ordinal))
                    .Select(x => new MemberSalaryDto
                    {
                        MemberId = x.Id,
                        MemberName = x.Name,
                        PayMonth = SearchMonth,
                        PayYear = SearchYear,
                        PayUnitName = SearchCondition.PayUnitName,
                        TableType = SearchCondition.TableType,
                        SalaryItems = new List<SalaryItem>()
                    }).ToListAsync().ConfigureAwait(false);

                List<MemberPayItemEntity> PayItemList = await _GhDbContext.dsMemberPayItem.AsNoTracking().OrderBy(x => x.OrderIndex)
                    .Where(x => x.MemberTypes.Contains(SearchCondition.MemberType, StringComparison.Ordinal) &&
                    x.InTableType.Equals(SearchCondition.TableType, StringComparison.Ordinal)).ToListAsync().ConfigureAwait(false);

                List<SalaryItem> salaryItems = new List<SalaryItem>();
                List<string> ItemNames = PayItemList.Select(x => x.Name).ToList();
                for (int i = 0; i < ItemNames.Count; i++)
                {
                    salaryItems.Add(new SalaryItem()
                    {
                        Name = ItemNames[i],
                        Amount = 0f
                    });
                }

                foreach (MemberSalaryDto item in FillResultList)
                {
                    item.SalaryItems.AddRange(salaryItems);
                }
                return FillResultList;

            }
            return new List<MemberSalaryDto>();
        }

        /// <summary>
        /// 读取指定条件的记录。上一方法调用
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private async Task<List<MemberSalaryEntity>> ReadSalarys(MemberSalarySearch SearchCondition)
        {
            IQueryable<MemberSalaryEntity> Items = _GhDbContext.dsMemberSalary.Include(xx => xx.Member).OrderBy(e => e.PayYear).ThenBy(e => e.PayMonth).AsNoTracking() as IQueryable<MemberSalaryEntity>;
            if (!string.IsNullOrWhiteSpace(SearchCondition.PayUnitName))
            {
                Items = Items.Where(e => e.PayUnitName.Equals(SearchCondition.PayUnitName, StringComparison.Ordinal));//查询发放单位。
            }
            //年度只能查询指定年度信息。
            Items = Items.Where(e => e.PayYear == SearchCondition.PayYear);

            if (SearchCondition.PayMonth > 0)
            {
                Items = Items.Where(e => e.PayMonth == SearchCondition.PayMonth);
            }
            if (!string.IsNullOrWhiteSpace(SearchCondition.TableType))
            {
                Items = Items.Where(e => e.TableType.Equals(SearchCondition.TableType, StringComparison.Ordinal));
            }

            if (!string.IsNullOrWhiteSpace(SearchCondition.MemberType))
            {
                Items = Items.Where(e => e.Member.MemberType.Equals(SearchCondition.MemberType, StringComparison.Ordinal));
            }

            if (!string.IsNullOrWhiteSpace(SearchCondition.Id))
            {
                Items = Items.Where(e => e.Id.Equals(SearchCondition.Id, StringComparison.Ordinal));//对两个字符串进行byte级别的比较,性能好、速度快。
            }
            if (!string.IsNullOrWhiteSpace(SearchCondition.MemberId))
            {
                Items = Items.Where(e => e.MemberId.Equals(SearchCondition.MemberId, StringComparison.Ordinal));//查询职工身份证号。
            }
            if (!string.IsNullOrWhiteSpace(SearchCondition.Remark))
            {
                Items = Items.Where(e => e.Remark.Contains(SearchCondition.Remark, StringComparison.Ordinal));
            }

            List<MemberSalaryEntity> SalaryList = await Items.OrderBy(x => x.Member.OrderIndex).ToListAsync().ConfigureAwait(false);

            return SalaryList;
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberSalaryEntity PEntity)
        {
            if (PEntity == null || PEntity.Id != null)
            {
                return -2;
            }
            PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberSalary.Add(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);

        }


        /// <summary>
        /// 更新一个实体信息
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MemberSalaryEntity PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberSalary.Update(PEntity);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(MemberSalaryEntity PEntity)
        {
            //此记录的Id为员工的身份证号码，必须输入
            if (PEntity == null || PEntity.Id == null || PEntity.MemberId == null) { return 0; }
            bool IsExist = await _GhDbContext.dsMemberSalary.AnyAsync(e => e.Id == PEntity.Id).ConfigureAwait(false);
            if (IsExist)
            {
                return await UpdateAsync(PEntity).ConfigureAwait(false);
            }
            else
            {
                PEntity.Id = AppCodes.AppStaticClass.GetIdOfDateTime();
                PEntity.UpDateTime = DateTime.Now;
                _GhDbContext.dsMemberSalary.Add(PEntity);
            }
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

            MemberSalaryEntity tempPlan = _GhDbContext.dsMemberSalary.Find(Id);
            _GhDbContext.dsMemberSalary.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
