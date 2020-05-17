using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Office.Work.Platform.Api.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dsFileDocs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OwnerType = table.Column<string>(type: "varchar(10)", nullable: true),
                    OwnerId = table.Column<string>(type: "varchar(20)", nullable: false),
                    ContentType = table.Column<string>(type: "varchar(200)", nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: true),
                    DispatchUnit = table.Column<string>(type: "varchar(500)", nullable: true),
                    CanReadUserIds = table.Column<string>(type: "varchar(1000)", nullable: true),
                    FileNumber = table.Column<string>(type: "varchar(500)", nullable: true),
                    Pubdate = table.Column<DateTime>(nullable: false),
                    ExtendName = table.Column<string>(type: "varchar(10)", nullable: true),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: true),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    Describe = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsFileDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberPay",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    MemberName = table.Column<string>(type: "varchar(20)", nullable: false),
                    MemberType = table.Column<string>(type: "varchar(20)", nullable: false),
                    PayName = table.Column<string>(type: "varchar(30)", nullable: false),
                    InCardinality = table.Column<string>(type: "varchar(10)", nullable: false),
                    InTableType = table.Column<string>(type: "varchar(20)", nullable: false),
                    AddOrCut = table.Column<string>(type: "varchar(10)", nullable: false),
                    Amount = table.Column<float>(type: "float(10,2)", nullable: false),
                    PayYear = table.Column<int>(nullable: false),
                    PayMonth = table.Column<int>(nullable: false),
                    PayUnitName = table.Column<string>(nullable: true),
                    MemberIndex = table.Column<int>(nullable: false),
                    OrderIndex = table.Column<int>(nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberPay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberPayItem",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(30)", nullable: false),
                    InCardinality = table.Column<string>(type: "varchar(10)", nullable: false),
                    InTableType = table.Column<string>(type: "varchar(20)", nullable: false),
                    AddOrCut = table.Column<string>(type: "varchar(10)", nullable: false),
                    UnitName = table.Column<string>(type: "varchar(50)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(30)", nullable: false),
                    UpDateTime = table.Column<string>(type: "varchar(50)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(300)", nullable: true),
                    OrderIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberPayItem", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberPaySet",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    PayUnitName = table.Column<string>(type: "varchar(50)", nullable: false),
                    PayItemNames = table.Column<string>(type: "varchar(2000)", nullable: true),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberPaySet", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "dsMembers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    Name = table.Column<string>(type: "varchar(10)", nullable: false),
                    Sex = table.Column<string>(type: "varchar(5)", nullable: true),
                    Nation = table.Column<string>(type: "varchar(20)", nullable: true),
                    Birthplace = table.Column<string>(type: "varchar(20)", nullable: true),
                    NativePlace = table.Column<string>(type: "varchar(30)", nullable: true),
                    HealthState = table.Column<string>(type: "varchar(30)", nullable: true),
                    Speciality = table.Column<string>(type: "varchar(30)", nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    BirthdayArchives = table.Column<DateTime>(nullable: false),
                    FixPhoneCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    MobileCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    BeginWork = table.Column<DateTime>(nullable: false),
                    EnterOrganization = table.Column<DateTime>(nullable: false),
                    MemberType = table.Column<string>(type: "varchar(20)", nullable: true),
                    JoinCPC = table.Column<DateTime>(nullable: false),
                    PoliticalStatus = table.Column<string>(type: "varchar(20)", nullable: true),
                    EducationDays = table.Column<string>(type: "varchar(20)", nullable: true),
                    GraduationSchoolDays = table.Column<string>(type: "varchar(50)", nullable: true),
                    MajorDays = table.Column<string>(type: "varchar(50)", nullable: true),
                    GraduationDateDays = table.Column<DateTime>(nullable: false),
                    EducationTop = table.Column<string>(type: "varchar(20)", nullable: true),
                    GraduationSchoolTop = table.Column<string>(type: "varchar(50)", nullable: true),
                    GraduationDateTop = table.Column<DateTime>(nullable: false),
                    MajorTop = table.Column<string>(type: "varchar(50)", nullable: true),
                    DegreeDays = table.Column<string>(type: "varchar(20)", nullable: true),
                    DegreeTop = table.Column<string>(type: "varchar(20)", nullable: true),
                    TechnicalTitle = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployTitle = table.Column<string>(type: "varchar(20)", nullable: true),
                    Department = table.Column<string>(type: "varchar(30)", nullable: true),
                    UnitName = table.Column<string>(type: "varchar(30)", nullable: true),
                    Post = table.Column<string>(type: "varchar(20)", nullable: true),
                    PostInCPC = table.Column<string>(type: "varchar(20)", nullable: true),
                    Job = table.Column<string>(type: "varchar(20)", nullable: true),
                    JobGrade = table.Column<string>(type: "varchar(20)", nullable: true),
                    OrderIndex = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(type: "varchar(2000)", nullable: true),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsNotes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Caption = table.Column<string>(type: "varchar(100)", nullable: false),
                    Content = table.Column<byte[]>(type: "mediumblob", nullable: false),
                    TextContent = table.Column<string>(type: "mediumtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    CanReadUserIds = table.Column<string>(type: "varchar(1000)", nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    Caption = table.Column<string>(type: "varchar(100)", nullable: false),
                    Content = table.Column<string>(type: "varchar(2000)", nullable: false),
                    FinishNote = table.Column<string>(type: "varchar(500)", nullable: true),
                    ContentType = table.Column<string>(type: "varchar(50)", nullable: false),
                    UnitName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Department = table.Column<string>(type: "varchar(50)", nullable: false),
                    ResponsiblePerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Helpers = table.Column<string>(type: "varchar(500)", nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FinishDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    CurrectState = table.Column<string>(type: "varchar(50)", nullable: false),
                    ReadGrant = table.Column<string>(type: "varchar(1000)", nullable: true),
                    UpDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsServerSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkContentType = table.Column<string>(type: "varchar(500)", nullable: false),
                    Deparmentts = table.Column<string>(type: "varchar(500)", nullable: false),
                    IntervalOne = table.Column<int>(nullable: false),
                    IntervalTwo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsServerSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(10)", nullable: false),
                    PassWord = table.Column<string>(type: "varchar(20)", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Post = table.Column<string>(type: "varchar(20)", nullable: false),
                    UnitName = table.Column<string>(type: "varchar(40)", nullable: false),
                    UnitShortName = table.Column<string>(type: "varchar(40)", nullable: false),
                    Department = table.Column<string>(type: "varchar(20)", nullable: false),
                    Grants = table.Column<string>(type: "varchar(1000)", nullable: false),
                    OrderIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberAppraise",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Year = table.Column<string>(type: "varchar(10)", nullable: false),
                    Result = table.Column<string>(type: "varchar(20)", nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberAppraise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberAppraise_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberHoliday",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    HolidayType = table.Column<string>(type: "varchar(20)", nullable: true),
                    HolidayReasion = table.Column<string>(type: "varchar(200)", nullable: true),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberHoliday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberHoliday_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberPrizePunish",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    OccurDate = table.Column<DateTime>(nullable: false),
                    PrizrOrPunishType = table.Column<string>(type: "varchar(20)", nullable: true),
                    PrizrOrPunishName = table.Column<string>(type: "varchar(50)", nullable: true),
                    PrizrOrPunishReasion = table.Column<string>(type: "varchar(500)", nullable: true),
                    PrizrOrPunishUnit = table.Column<string>(type: "varchar(60)", nullable: true),
                    GetScore = table.Column<float>(type: "float(10,2)", nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberPrizePunish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberPrizePunish_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberRelations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Relation = table.Column<string>(type: "varchar(50)", nullable: false),
                    UnitName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Role = table.Column<string>(type: "varchar(50)", nullable: true),
                    OrderIndex = table.Column<int>(nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberRelations_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberResume",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(type: "varchar(500)", nullable: true),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberResume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberResume_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberAppraise_MemberId",
                table: "dsMemberAppraise",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberHoliday_MemberId",
                table: "dsMemberHoliday",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberPrizePunish_MemberId",
                table: "dsMemberPrizePunish",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberRelations_MemberId",
                table: "dsMemberRelations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberResume_MemberId",
                table: "dsMemberResume",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dsFileDocs");

            migrationBuilder.DropTable(
                name: "dsMemberAppraise");

            migrationBuilder.DropTable(
                name: "dsMemberHoliday");

            migrationBuilder.DropTable(
                name: "dsMemberPay");

            migrationBuilder.DropTable(
                name: "dsMemberPayItem");

            migrationBuilder.DropTable(
                name: "dsMemberPaySet");

            migrationBuilder.DropTable(
                name: "dsMemberPrizePunish");

            migrationBuilder.DropTable(
                name: "dsMemberRelations");

            migrationBuilder.DropTable(
                name: "dsMemberResume");

            migrationBuilder.DropTable(
                name: "dsNotes");

            migrationBuilder.DropTable(
                name: "dsPlans");

            migrationBuilder.DropTable(
                name: "dsServerSetting");

            migrationBuilder.DropTable(
                name: "dsUsers");

            migrationBuilder.DropTable(
                name: "dsMembers");
        }
    }
}
