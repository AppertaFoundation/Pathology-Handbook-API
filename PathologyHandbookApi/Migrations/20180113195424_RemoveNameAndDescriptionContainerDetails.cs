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
    public partial class RemoveNameAndDescriptionContainerDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContainerDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ContainerDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContainerDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ContainerDetails",
                nullable: true);
        }
    }
}
