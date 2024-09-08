using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cabinets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Number",
                value: "101");

            migrationBuilder.UpdateData(
                table: "Cabinets",
                keyColumn: "Id",
                keyValue: 2,
                column: "Number",
                value: "204");

            migrationBuilder.InsertData(
                table: "Cabinets",
                columns: new[] { "Id", "Number" },
                values: new object[,]
                {
                    { 3, "212" },
                    { 4, "302" }
                });

            migrationBuilder.UpdateData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Терапевт");

            migrationBuilder.UpdateData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Хирург");

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Окулист" },
                    { 4, "Педиатр" }
                });

            migrationBuilder.UpdateData(
                table: "Uchastoks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Number",
                value: "1А");

            migrationBuilder.UpdateData(
                table: "Uchastoks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Number",
                value: "1Б");

            migrationBuilder.InsertData(
                table: "Uchastoks",
                columns: new[] { "Id", "Number" },
                values: new object[,]
                {
                    { 3, "2А" },
                    { 4, "2Б" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cabinets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cabinets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Uchastoks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Uchastoks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Cabinets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Number",
                value: "Тестовый кабинет №1");

            migrationBuilder.UpdateData(
                table: "Cabinets",
                keyColumn: "Id",
                keyValue: 2,
                column: "Number",
                value: "Тестовый кабинет №2");

            migrationBuilder.UpdateData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Тестовый специализация №1");

            migrationBuilder.UpdateData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Тестовая специализация №2");

            migrationBuilder.UpdateData(
                table: "Uchastoks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Number",
                value: "Тестовый участок №1");

            migrationBuilder.UpdateData(
                table: "Uchastoks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Number",
                value: "Тестовый участок №2");
        }
    }
}
