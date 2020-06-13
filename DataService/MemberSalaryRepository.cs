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
        public async Task<IEnumerable<MemberSalary>> GetAllAsync()
        {
            return await _GhDbContext.dsMemberSalary.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="P_Id"></param>
        /// <returns></returns>
        public async Task<MemberSalary> GetOneByIdAsync(string Id)
        {
            return await _GhDbContext.dsMemberSalary.FindAsync(Id).ConfigureAwait(false);
        }
        /// <summary>
        /// 根据条件查询计划,返回查询的实体列表
        /// </summary>
        /// <param name="mSearchMember">员工查询类对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MemberSalarySearchResult>> GetEntitiesAsync(MemberSalarySearch SearchCondition)
        {
            if (SearchCondition != null && !string.IsNullOrWhiteSpace(SearchCondition.UserId))
            {
                int SearchYear = SearchCondition.PayYear;
                int SearchMonth = SearchCondition.PayMonth;
                List<MemberSalary> SalaryList = await ReadSalarys(SearchCondition).ConfigureAwait(false);


                //如果未查到指定月份的数据，则读取上一个月的数据数据
                if (SalaryList == null || SalaryList.Count == 0)
                {
                    SearchCondition.PayYear = SearchCondition.PayMonth == 1 ? SearchCondition.PayYear - 1 : SearchCondition.PayYear;
                    SearchCondition.PayMonth = SearchCondition.PayMonth > 1 ? SearchCondition.PayMonth - 1 : 12;

                    SalaryList = await ReadSalarys(SearchCondition).ConfigureAwait(false);
                }
                //如果仍未查到，则显示空表
                if (SalaryList == null || SalaryList.Count == 0)
                {
                    List<MemberSalarySearchResult> ResultList = await _GhDbContext.dsMembers.AsNoTracking().OrderBy(x => x.OrderIndex)
                        .Where(x => x.MemberType.Equals(SearchCondition.MemberType, StringComparison.Ordinal) && x.UnitName.Equals(SearchCondition.PayUnitName, StringComparison.Ordinal))
                        .Select(x => new MemberSalarySearchResult
                        {
                            MemberId = x.Id,
                            MemberName = x.Name,
                            PayMonth = SearchMonth ,
                            PayYear = SearchYear,
                            PayUnitName = SearchCondition.PayUnitName,
                            TableType = SearchCondition.TableType,
                            SalaryItems = new List<SalaryItem>()
                        }).ToListAsync().ConfigureAwait(false);

                    List<MemberPayItem> PayItemList = await _GhDbContext.dsMemberPayItem.AsNoTracking().OrderBy(x => x.OrderIndex)
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

                    foreach (MemberSalarySearchResult item in ResultList)
                    {
                        item.SalaryItems.AddRange(salaryItems);
                    }
                    return ResultList;
                }
                else
                {
                    //返回查到的数据
                    List<MemberSalarySearchResult> ResultList = SalaryList.Select(x => new MemberSalarySearchResult
                    {
                        MemberId = x.Member.Id,
                        MemberName = x.Member.Name,
                        PayMonth = SearchMonth,
                        PayYear = SearchYear,
                        PayUnitName = SearchCondition.PayUnitName,
                        TableType = SearchCondition.TableType,
                        SalaryItems = JsonConvert.DeserializeObject<List<SalaryItem>>(x.NameAndAmount)
                    }).ToList();
                    return ResultList;
                }
            }
            return new List<MemberSalarySearchResult>();
        }

        /// <summary>
        /// 读取指定条件的记录。上一方法调用
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private async Task<List<MemberSalary>> ReadSalarys(MemberSalarySearch SearchCondition)
        {
            IQueryable<MemberSalary> Items = _GhDbContext.dsMemberSalary.Include(xx => xx.Member).OrderBy(e => e.PayYear).ThenBy(e => e.PayMonth).AsNoTracking() as IQueryable<MemberSalary>;
            if (!string.IsNullOrWhiteSpace(SearchCondition.PayUnitName))
            {
                Items = Items.Where(e => e.PayUnitName.Equals(SearchCondition.PayUnitName, StringComparison.Ordinal));//查询发放单位。
            }

            if (SearchCondition.PayYear > 0)
            {
                Items = Items.Where(e => e.PayYear == SearchCondition.PayYear);
            }
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

            List<MemberSalary> SalaryList = await Items.OrderBy(x => x.Member.OrderIndex).ToListAsync().ConfigureAwait(false);
            return SalaryList;
        }
        /// <summary>
        /// 向数据库表添加一个新的记录，如果该记录已经存在，返回-2
        /// </summary>
        /// <param name="PEntity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(MemberSalary PEntity)
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
        public async Task<int> UpdateAsync(MemberSalary PEntity)
        {
            if (PEntity == null) { return 0; }
            PEntity.UpDateTime = DateTime.Now;
            _GhDbContext.dsMemberSalary.Update(PEntity);
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

            MemberSalary tempPlan = _GhDbContext.dsMemberSalary.Find(Id);
            _GhDbContext.dsMemberSalary.Remove(tempPlan);
            return await _GhDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
