//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PathologyHandbookApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectionContainerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    ColourHex = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GeneralDetails = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CollectionContainerTypes", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactTypeDescription = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ContactTypes", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OpeningHours = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Departments", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "TagTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_TagTypes", x => x.Id); });

            migrationBuilder.InsertData(
                table: "TagTypes",
                columns: new[] { "Id", "Code", "CreatedBy", "DateCreated", "Description" },
                values: new object[,]
                {
                    { 1,  "Code", "Initial", DateTime.Now, "Code", true },
                    { 2,  "Synonym", "Initial",  DateTime.Now, "Synonym", true },
                    { 3,  "MainTestName", "Initial",  DateTime.Now, "Main Test Name", true },
                    { 4,  "ConcurrentTest", "Initial",  DateTime.Now, "Concurrent Test", true },
                    { 5,  "Department", "Initial",  DateTime.Now, "Department", true }
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CostPerTest = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    GeneralDetails = table.Column<string>(nullable: true),
                    GpAcuteTaT = table.Column<string>(nullable: true),
                    GpNormalTaT = table.Column<string>(nullable: true),
                    Guidelines = table.Column<string>(nullable: true),
                    Interpretation = table.Column<string>(nullable: true),
                    IpAcuteTaT = table.Column<string>(nullable: true),
                    IpNormalTaT = table.Column<string>(nullable: true),
                    MainCode = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    NameDescription = table.Column<string>(nullable: true),
                    ReferenceIntervalFemale = table.Column<string>(nullable: true),
                    ReferenceIntervalMale = table.Column<string>(nullable: true),
                    ReferenceIntervalNotes = table.Column<string>(nullable: true),
                    ReferenceIntervalPaediatric = table.Column<string>(nullable: true),
                    TestId = table.Column<int>(nullable: true),
                    UnitsOfMeasurement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tests_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactId = table.Column<int>(nullable: true),
                    ContactTypeId = table.Column<int>(nullable: false),
                    ContactValue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactDetails_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactDetails_ContactTypes_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContainerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CollectionConditions = table.Column<string>(nullable: true),
                    CollectionContainerTypeId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DietaryRequirements = table.Column<string>(nullable: true),
                    DrawOrder = table.Column<int>(nullable: true),
                    GeneralDetails = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NumberOfCollectionContainersRequired = table.Column<int>(nullable: false),
                    SpecimenType = table.Column<string>(nullable: true),
                    StorageConditions = table.Column<string>(nullable: true),
                    TestId = table.Column<int>(nullable: true),
                    TransportConditions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContainerDetails_CollectionContainerTypes_CollectionContainerTypeId",
                        column: x => x.CollectionContainerTypeId,
                        principalTable: "CollectionContainerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContainerDetails_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    TagTypeId = table.Column<int>(nullable: false),
                    TestId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_TagTypes_TagTypeId",
                        column: x => x.TagTypeId,
                        principalTable: "TagTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tags_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_ContactId",
                table: "ContactDetails",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_ContactTypeId",
                table: "ContactDetails",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_DepartmentId",
                table: "Contacts",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerDetails_CollectionContainerTypeId",
                table: "ContainerDetails",
                column: "CollectionContainerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerDetails_TestId",
                table: "ContainerDetails",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagTypeId",
                table: "Tags",
                column: "TagTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TestId",
                table: "Tags",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_DepartmentId",
                table: "Tests",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestId",
                table: "Tests",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactDetails");

            migrationBuilder.DropTable(
                name: "ContainerDetails");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropTable(
                name: "CollectionContainerTypes");

            migrationBuilder.DropTable(
                name: "TagTypes");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
