//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PathologyHandbookApi.Migrations
{
    public partial class AddRefRangesToTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RefRanges_TestId",
                table: "RefRanges",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefRanges_Tests_TestId",
                table: "RefRanges",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefRanges_Tests_TestId",
                table: "RefRanges");

            migrationBuilder.DropIndex(
                name: "IX_RefRanges_TestId",
                table: "RefRanges");
        }
    }
}
