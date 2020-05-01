﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Office.Work.Platform.Api.DataService;

namespace Office.Work.Platform.Api.Migrations
{
    [DbContext(typeof(GHDbContext))]
    partial class GHDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Office.Work.Platform.Lib.Member", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("BeginWork")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DegreeTop")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Department")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("EducationDays")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("EducationTop")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("EmployTitle")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("EmploymentType")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("EnterOrganization")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FixPhoneCode")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("GraduationDateDays")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("GraduationDateTop")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GraduationSchoolDays")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("GraduationSchoolTop")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Job")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("JobGrade")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("JoinCPC")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MajorDays")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MajorTop")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MobileCode")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("PoliticalStatus")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Post")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PostInCPC")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Remarks")
                        .HasColumnType("varchar(2000)");

                    b.Property<string>("Sex")
                        .HasColumnType("varchar(5)");

                    b.Property<string>("TechnicalTitle")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("UnitName")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("dsMembers");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Describe")
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("ExtendName")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("OtherRecordId")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberFiles");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberHoliday", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HolidayReasion")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("HolidayType")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberHoliday");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayMonthInsurance", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<float>("HousingFund")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("MedicalInsurance")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<float>("OccupationalPension")
                        .HasColumnType("float(10,2)");

                    b.Property<int>("PayMonth")
                        .HasColumnType("int");

                    b.Property<int>("PayYear")
                        .HasColumnType("int");

                    b.Property<float>("PensionInsurance")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(500)");

                    b.Property<float>("Tax")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("UnemploymentInsurance")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("UnionFees")
                        .HasColumnType("float(10,2)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberPayMonthInsurance");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayMonthOfficial", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<float>("FoodAllowance")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("IncentivePerformancePay")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("LivingAllowance")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("PayMonth")
                        .HasColumnType("int");

                    b.Property<int>("PayYear")
                        .HasColumnType("int");

                    b.Property<float>("PostAllowance")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("PostPay")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(500)");

                    b.Property<float>("ScalePay")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("TrafficAllowance")
                        .HasColumnType("float(10,2)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberPayMonthOfficial");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayMonthUnofficial", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<float>("BasicPay")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("FoodAllowance")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("PayMonth")
                        .HasColumnType("int");

                    b.Property<int>("PayYear")
                        .HasColumnType("int");

                    b.Property<float>("PerformancePay")
                        .HasColumnType("float(10,2)");

                    b.Property<float>("PostPay")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(500)");

                    b.Property<float>("TrafficAllowance")
                        .HasColumnType("float(10,2)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberPayMonthUnofficial");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayTemp", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<float>("Amount")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberPayTemp");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPrizePunish", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<float>("GetScore")
                        .HasColumnType("float(10,2)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("OccurDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PrizrOrPunishName")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PrizrOrPunishReasion")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("PrizrOrPunishType")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PrizrOrPunishUnit")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberPrizePunish");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberRelations", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Relation")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Role")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UnitName")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberRelations");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberResume", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Content")
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("dsMemberResume");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.Note", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("CanReadUserIds")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("blob");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("dsNotes");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.Plan", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("varchar(2000)");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CurrectState")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FinishDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FinishNote")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Helpers")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("PlanType")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ReadGrant")
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("ResponsiblePerson")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("dsPlans");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.PlanFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Describe")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("ExtendName")
                        .HasColumnType("varchar(10)");

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("PlanId")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("UpDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("dsPlanFiles");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.SettingServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Deparmentts")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.Property<int>("IntervalOne")
                        .HasColumnType("int");

                    b.Property<int>("IntervalTwo")
                        .HasColumnType("int");

                    b.Property<string>("WorkContentType")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.ToTable("dsServerSetting");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Grants")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Post")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasColumnType("varchar(40)");

                    b.HasKey("Id");

                    b.ToTable("dsUsers");
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberFile", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberHoliday", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayMonthInsurance", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayMonthOfficial", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayMonthUnofficial", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPayTemp", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberPrizePunish", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberRelations", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.MemberResume", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Office.Work.Platform.Lib.PlanFile", b =>
                {
                    b.HasOne("Office.Work.Platform.Lib.Plan", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
