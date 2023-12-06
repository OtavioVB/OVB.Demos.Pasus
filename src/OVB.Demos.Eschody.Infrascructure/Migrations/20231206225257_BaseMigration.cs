using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OVB.Demos.Eschody.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class BaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    idstudent = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    correlation_id = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    source_platform = table.Column<string>(type: "VARCHAR", maxLength: 32, nullable: false),
                    execution_user = table.Column<string>(type: "VARCHAR", maxLength: 32, nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMPTZ", fixedLength: true, nullable: false),
                    first_name = table.Column<string>(type: "VARCHAR", maxLength: 32, nullable: false),
                    last_name = table.Column<string>(type: "VARCHAR", maxLength: 32, nullable: false),
                    email = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "CHAR(11)", fixedLength: true, maxLength: 11, nullable: false),
                    password = table.Column<string>(type: "CHAR(64)", fixedLength: true, maxLength: 64, nullable: false),
                    last_correlation_id = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    last_source_platform = table.Column<string>(type: "VARCHAR", maxLength: 32, nullable: false),
                    last_execution_user = table.Column<string>(type: "VARCHAR", maxLength: 32, nullable: false),
                    last_modified_at = table.Column<DateTime>(type: "TIMESTAMPTZ", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_id", x => x.idstudent);
                });

            migrationBuilder.CreateIndex(
                name: "uk_student_email",
                table: "students",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students");
        }
    }
}
