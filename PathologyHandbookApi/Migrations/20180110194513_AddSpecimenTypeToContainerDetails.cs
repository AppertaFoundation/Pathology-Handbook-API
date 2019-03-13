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
    public partial class AddSpecimenTypeToContainerDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecimenType",
                table: "ContainerDetails");

            migrationBuilder.AddColumn<int>(
                name: "SpecimenTypeId",
                table: "ContainerDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContainerDetails_SpecimenTypeId",
                table: "ContainerDetails",
                column: "SpecimenTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContainerDetails_SpecimenTypes_SpecimenTypeId",
                table: "ContainerDetails",
                column: "SpecimenTypeId",
                principalTable: "SpecimenTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContainerDetails_SpecimenTypes_SpecimenTypeId",
                table: "ContainerDetails");

            migrationBuilder.DropIndex(
                name: "IX_ContainerDetails_SpecimenTypeId",
                table: "ContainerDetails");

            migrationBuilder.DropColumn(
                name: "SpecimenTypeId",
                table: "ContainerDetails");

            migrationBuilder.AddColumn<string>(
                name: "SpecimenType",
                table: "ContainerDetails",
                nullable: true);
        }
    }
}
