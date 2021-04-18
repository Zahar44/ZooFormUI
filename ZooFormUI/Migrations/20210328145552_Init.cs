using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZooFormUI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aviaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Length = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    MaxAnimalsCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aviaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Freeze = table.Column<bool>(nullable: false),
                    RotAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kinds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsWormBlooded = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Сonditions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZooKeepers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(nullable: true),
                    SName = table.Column<string>(nullable: true),
                    MName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Family = table.Column<int>(nullable: false),
                    Salary = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZooKeepers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AviaryKinds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AviaryId = table.Column<int>(nullable: false),
                    KindId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AviaryKinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AviaryKinds_Aviaries_AviaryId",
                        column: x => x.AviaryId,
                        principalTable: "Aviaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AviaryKinds_Kinds_KindId",
                        column: x => x.KindId,
                        principalTable: "Kinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    IsPredator = table.Column<bool>(nullable: false),
                    KindId = table.Column<int>(nullable: false),
                    ZooKeeperId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Kinds_KindId",
                        column: x => x.KindId,
                        principalTable: "Kinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animals_ZooKeepers_ZooKeeperId",
                        column: x => x.ZooKeeperId,
                        principalTable: "ZooKeepers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalFoods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(nullable: false),
                    FoodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalFoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalFoods_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalFoods_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalFoods_AnimalId",
                table: "AnimalFoods",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalFoods_FoodId",
                table: "AnimalFoods",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_KindId",
                table: "Animals",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ZooKeeperId",
                table: "Animals",
                column: "ZooKeeperId");

            migrationBuilder.CreateIndex(
                name: "IX_AviaryKinds_AviaryId",
                table: "AviaryKinds",
                column: "AviaryId");

            migrationBuilder.CreateIndex(
                name: "IX_AviaryKinds_KindId",
                table: "AviaryKinds",
                column: "KindId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalFoods");

            migrationBuilder.DropTable(
                name: "AviaryKinds");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Aviaries");

            migrationBuilder.DropTable(
                name: "Kinds");

            migrationBuilder.DropTable(
                name: "ZooKeepers");
        }
    }
}
