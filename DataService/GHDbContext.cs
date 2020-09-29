using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class GHDbContext : DbContext
    {
        public GHDbContext(DbContextOptions<GHDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<SettingServerEntity> dsServerSetting { get; set; }

        public DbSet<UserEntity> dsUsers { get; set; }

        public DbSet<NoteInfoEntity> dsNotes { get; set; }

        public DbSet<PlanEntity> dsPlans { get; set; }
        public DbSet<PlanFileEntity> dsPlanFiles { get; set; }

        public DbSet<MemberInfoEntity> dsMembers { get; set; }
        public DbSet<MemberSalaryEntity> dsMemberSalary { get; set; }
        public DbSet<MemberPayItemEntity> dsMemberPayItem { get; set; }
        public DbSet<MemberFileEntity> dsMemberFiles { get; set; }

        /// <summary>
        /// 个人绩效得分情况
        /// </summary>
        public DbSet<MemberScoreEntity> dsMemberScores { get; set; }

        /// <summary>
        /// 个人简历
        /// </summary>
        public DbSet<MemberResumeEntity> dsMemberResume { get; set; }

        /// <summary>
        /// 奖惩情况
        /// </summary>
        public DbSet<MemberPrizePunishEntity> dsMemberPrizePunish { get; set; }
        /// <summary>
        /// 考核情况
        /// </summary>
        public DbSet<MemberAppraiseEntity> dsMemberAppraise { get; set; }
        /// <summary>
        /// 社会关系
        /// </summary>
        public DbSet<MemberRelationsEntity> dsMemberRelations { get; set; }
        /// <summary>
        /// 休假信息
        /// </summary>
        public DbSet<MemberHolidayEntity> dsMemberHoliday { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                //modelBuilder.Entity<SettingServer>().HasKey(x => x.Id);

                //modelBuilder.Entity<User>().HasKey(x => x.Id);

                //modelBuilder.Entity<Note>().HasKey(x => x.Id);

                //modelBuilder.Entity<Plan>().HasKey(x => x.Id);
                //modelBuilder.Entity<Plan>().Property("Caption").IsRequired().HasMaxLength(100);
                //modelBuilder.Entity<PlanFile>().Property("Name").IsRequired().HasMaxLength(100);

                //modelBuilder.Entity<Member>().HasKey(x => x.Id);
                //modelBuilder.Entity<Member>().Property("Name").IsRequired().HasMaxLength(50);

            }
        }
    }
}
