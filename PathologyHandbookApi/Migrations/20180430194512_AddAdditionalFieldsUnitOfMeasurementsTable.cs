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
    public partial class AddAdditionalFieldsUnitOfMeasurementsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "UnitOfMeasurements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UnitOfMeasurements",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "UnitOfMeasurements",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "UnitOfMeasurements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "UnitOfMeasurements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UnitOfMeasurements");
        }
    }
}
