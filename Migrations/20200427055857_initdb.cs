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
                name: "dsMembers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    Name = table.Column<string>(type: "varchar(10)", nullable: false),
                    Sex = table.Column<string>(type: "varchar(5)", nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    FixPhoneCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    MobileCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    BeginWork = table.Column<DateTime>(nullable: false),
                    EnterOrganization = table.Column<DateTime>(nullable: false),
                    EmploymentType = table.Column<string>(type: "varchar(20)", nullable: true),
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
                    DegreeTop = table.Column<string>(type: "varchar(20)", nullable: true),
                    TechnicalTitle = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployTitle = table.Column<string>(type: "varchar(20)", nullable: true),
                    Department = table.Column<string>(type: "varchar(30)", nullable: true),
                    UnitName = table.Column<string>(type: "varchar(30)", nullable: true),
                    Post = table.Column<string>(type: "varchar(20)", nullable: true),
                    PostInCPC = table.Column<string>(type: "varchar(20)", nullable: true),
                    Job = table.Column<string>(type: "varchar(20)", nullable: true),
                    JobGrade = table.Column<string>(type: "varchar(20)", nullable: true),
                    Resume = table.Column<string>(type: "varchar(2000)", nullable: true),
                    Prize = table.Column<string>(type: "varchar(2000)", nullable: true),
                    Punish = table.Column<string>(type: "varchar(2000)", nullable: true),
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
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    Caption = table.Column<string>(type: "varchar(100)", nullable: false),
                    Content = table.Column<byte[]>(type: "blob", nullable: false),
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
                    PlanType = table.Column<string>(type: "varchar(50)", nullable: false),
                    Department = table.Column<string>(type: "varchar(50)", nullable: false),
                    ResponsiblePerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Helpers = table.Column<string>(type: "varchar(500)", nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FinishDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<string>(type: "varchar(20)", nullable: true),
                    CurrectState = table.Column<string>(type: "varchar(50)", nullable: true),
                    ReadGrant = table.Column<string>(type: "varchar(1000)", nullable: true)
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
                    Department = table.Column<string>(type: "varchar(20)", nullable: false),
                    Grants = table.Column<string>(type: "varchar(1000)", nullable: false),
                    OrderIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberFiles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    FType = table.Column<string>(type: "varchar(20)", nullable: false),
                    PayId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    ExtendName = table.Column<string>(type: "varchar(10)", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    Describe = table.Column<string>(type: "varchar(1000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberFiles_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberPayMonth",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    PostPay = table.Column<float>(nullable: false),
                    ScalePay = table.Column<float>(nullable: false),
                    PostAllowance = table.Column<float>(nullable: false),
                    LivingAllowance = table.Column<float>(nullable: false),
                    IncentivePerformancePay = table.Column<float>(nullable: false),
                    HousingFund = table.Column<float>(nullable: false),
                    OccupationalPension = table.Column<float>(nullable: false),
                    PensionInsurance = table.Column<float>(nullable: false),
                    UnemploymentInsurance = table.Column<float>(nullable: false),
                    MedicalInsurance = table.Column<float>(nullable: false),
                    UnionFees = table.Column<float>(nullable: false),
                    Tax = table.Column<float>(nullable: false),
                    PayYear = table.Column<int>(nullable: false),
                    PayMonth = table.Column<int>(nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberPayMonth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberPayMonth_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberPayMonthUnofficial",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    BasicPay = table.Column<float>(nullable: false),
                    PostPay = table.Column<float>(nullable: false),
                    PerformancePay = table.Column<float>(nullable: false),
                    HousingFund = table.Column<float>(nullable: false),
                    OccupationalPension = table.Column<float>(nullable: false),
                    PensionInsurance = table.Column<float>(nullable: false),
                    UnemploymentInsurance = table.Column<float>(nullable: false),
                    MedicalInsurance = table.Column<float>(nullable: false),
                    UnionFees = table.Column<float>(nullable: false),
                    Tax = table.Column<float>(nullable: false),
                    PayYear = table.Column<int>(nullable: false),
                    PayMonth = table.Column<int>(nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberPayMonthUnofficial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberPayMonthUnofficial_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsMemberPayTemp",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PayName = table.Column<string>(nullable: true),
                    MemberId = table.Column<string>(type: "varchar(20)", nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsMemberPayTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsMemberPayTemp_dsMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "dsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dsPlanFiles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PlanId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: true),
                    ExtendName = table.Column<string>(type: "varchar(10)", nullable: true),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: true),
                    UpDateTime = table.Column<DateTime>(nullable: false),
                    Describe = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dsPlanFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dsPlanFiles_dsPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "dsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberFiles_MemberId",
                table: "dsMemberFiles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberPayMonth_MemberId",
                table: "dsMemberPayMonth",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberPayMonthUnofficial_MemberId",
                table: "dsMemberPayMonthUnofficial",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsMemberPayTemp_MemberId",
                table: "dsMemberPayTemp",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_dsPlanFiles_PlanId",
                table: "dsPlanFiles",
                column: "PlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dsMemberFiles");

            migrationBuilder.DropTable(
                name: "dsMemberPayMonth");

            migrationBuilder.DropTable(
                name: "dsMemberPayMonthUnofficial");

            migrationBuilder.DropTable(
                name: "dsMemberPayTemp");

            migrationBuilder.DropTable(
                name: "dsNotes");

            migrationBuilder.DropTable(
                name: "dsPlanFiles");

            migrationBuilder.DropTable(
                name: "dsServerSetting");

            migrationBuilder.DropTable(
                name: "dsUsers");

            migrationBuilder.DropTable(
                name: "dsMembers");

            migrationBuilder.DropTable(
                name: "dsPlans");
        }
    }
}
