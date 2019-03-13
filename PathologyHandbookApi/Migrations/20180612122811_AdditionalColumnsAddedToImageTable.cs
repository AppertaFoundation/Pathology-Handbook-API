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
    public partial class AdditionalColumnsAddedToImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CollectionContainerTypes_CollectionContainerTypeId",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "CollectionContainerTypeId",
                table: "Images",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullsizeFileName",
                table: "Images",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailFileName",
                table: "Images",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_CollectionContainerTypes_CollectionContainerTypeId",
                table: "Images",
                column: "CollectionContainerTypeId",
                principalTable: "CollectionContainerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CollectionContainerTypes_CollectionContainerTypeId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FullsizeFileName",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ThumbnailFileName",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "CollectionContainerTypeId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Images_CollectionContainerTypes_CollectionContainerTypeId",
                table: "Images",
                column: "CollectionContainerTypeId",
                principalTable: "CollectionContainerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
