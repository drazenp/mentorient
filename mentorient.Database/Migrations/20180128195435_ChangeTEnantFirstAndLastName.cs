﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace mentorient.Database.Migrations
{
    public partial class ChangeTEnantFirstAndLastName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Tenants",
                newName: "TenantLastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Tenants",
                newName: "TenantFirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenantLastName",
                table: "Tenants",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "TenantFirstName",
                table: "Tenants",
                newName: "FirstName");
        }
    }
}
