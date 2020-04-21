using Microsoft.EntityFrameworkCore;
using Office.Work.Platform.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class GHDbContext : DbContext
    {
        public GHDbContext(DbContextOptions<GHDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<ModelUser> Users { get; set; }
        public DbSet<ModelFile> Files { get; set; }
        public DbSet<ModelMember> Members { get; set; }
        public DbSet<ModelMemberFamily> MemberFamily { get; set; }
        public DbSet<ModelPlan> Plans { get; set; }
        public DbSet<ModelSettingServer> ServerSetting { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Entity<ModelUser>().HasKey(x => x.Id);
                modelBuilder.Entity<ModelFile>().HasKey(x => x.Id);
                modelBuilder.Entity<ModelFile>().Property("Name").IsRequired().HasMaxLength(100);
                modelBuilder.Entity<ModelMember>().HasKey(x => x.Id);
                modelBuilder.Entity<ModelMember>().Property("Name").IsRequired().HasMaxLength(50);
                modelBuilder.Entity<ModelMemberFamily>().HasKey(x => x.Name);
                modelBuilder.Entity<ModelMemberFamily>().Property("Name").IsRequired().HasMaxLength(50);
                modelBuilder.Entity<ModelPlan>().HasKey(x => x.Id);
                modelBuilder.Entity<ModelPlan>().Property("Caption").IsRequired().HasMaxLength(100);
                modelBuilder.Entity<ModelSettingServer>().HasKey(x => x.Id);
            }
        }
    }
}
