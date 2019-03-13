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
    public partial class AddLabNotesFieldsToTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabNotes",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabProcessNotes",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabStorageNotes",
                table: "Tests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabNotes",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LabProcessNotes",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LabStorageNotes",
                table: "Tests");
        }
    }
}
