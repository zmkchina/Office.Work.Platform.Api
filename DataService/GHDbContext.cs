using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;

namespace Office.Work.Platform.Api.DataService
{
    public class GHDbContext : DbContext
    {
        public GHDbContext(DbContextOptions<GHDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<SettingServer> dsServerSetting { get; set; }

        public DbSet<User> dsUsers { get; set; }

        public DbSet<Note> dsNotes { get; set; }

        public DbSet<Plan> dsPlans { get; set; }
        public DbSet<PlanFile> dsPlanFiles { get; set; }
        public DbSet<Member> dsMembers { get; set; }
        public DbSet<MemberPayMonth> dsMemberPayMonth { get; set; }
        public DbSet<MemberPayMonthInsurance> dsMemberPayMonthInsurance { get; set; }
        public DbSet<MemberPayMonthUnofficial> dsMemberPayMonthUnofficial { get; set; }
        public DbSet<MemberPayTemp> dsMemberPayTemp { get; set; }
        public DbSet<MemberFile> dsMemberFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                //modelBuilder.Entity<SettingServer>().HasKey(x => x.Id);

                //modelBuilder.Entity<User>().HasKey(x => x.Id);

                //modelBuilder.Entity<Note>().HasKey(x => x.Id);

                //modelBuilder.Entity<Plan>().HasKey(x => x.Id);
                //modelBuilder.Entity<Plan>().Property("Caption").IsRequired().HasMaxLength(100);

                //modelBuilder.Entity<PlanFile>().HasKey(x => x.Id);
                //modelBuilder.Entity<PlanFile>().Property("Name").IsRequired().HasMaxLength(100);

                //modelBuilder.Entity<Member>().HasKey(x => x.Id);
                //modelBuilder.Entity<Member>().Property("Name").IsRequired().HasMaxLength(50);

                //modelBuilder.Entity<MemberPayMonth>().HasKey(x => x.Id);

                //modelBuilder.Entity<MemberPayMonthUnofficial>().HasKey(x => x.Id);

                //modelBuilder.Entity<MemberPayTemp>().HasKey(x => x.Id);
                //modelBuilder.Entity<MemberPayTemp>().Property("PayName").IsRequired().HasMaxLength(50);

                //modelBuilder.Entity<MemberFile>().HasKey(x => x.Id);
                //modelBuilder.Entity<MemberFile>().Property("Name").IsRequired().HasMaxLength(50);
            }
        }
    }
}
