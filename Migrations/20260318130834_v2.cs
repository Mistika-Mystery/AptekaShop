using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apteka.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Akqias",
                columns: table => new
                {
                    IDAk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naz = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Akqias", x => x.IDAk);
                });

            migrationBuilder.CreateTable(
                name: "Proizs",
                columns: table => new
                {
                    IDPr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naz = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizs", x => x.IDPr);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    idRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.idRole);
                });

            migrationBuilder.CreateTable(
                name: "Vids",
                columns: table => new
                {
                    IDV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naz = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vids", x => x.IDV);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "idRole",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leks",
                columns: table => new
                {
                    IDL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naz = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Qena = table.Column<int>(type: "int", nullable: false),
                    ProizIDPr = table.Column<int>(type: "int", nullable: true),
                    AkqiaIDAk = table.Column<int>(type: "int", nullable: true),
                    VidZIDV = table.Column<int>(type: "int", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leks", x => x.IDL);
                    table.ForeignKey(
                        name: "FK_Leks_Akqias_AkqiaIDAk",
                        column: x => x.AkqiaIDAk,
                        principalTable: "Akqias",
                        principalColumn: "IDAk");
                    table.ForeignKey(
                        name: "FK_Leks_Proizs_ProizIDPr",
                        column: x => x.ProizIDPr,
                        principalTable: "Proizs",
                        principalColumn: "IDPr");
                    table.ForeignKey(
                        name: "FK_Leks_Vids_VidZIDV",
                        column: x => x.VidZIDV,
                        principalTable: "Vids",
                        principalColumn: "IDV");
                });

            migrationBuilder.CreateTable(
                name: "Zakazs",
                columns: table => new
                {
                    IDZ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    UsersIdUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zakazs", x => x.IDZ);
                    table.ForeignKey(
                        name: "FK_Zakazs_Users_UsersIdUser",
                        column: x => x.UsersIdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "ZakLeks",
                columns: table => new
                {
                    IDZL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZakazIDZ = table.Column<int>(type: "int", nullable: false),
                    LekIDL = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZakLeks", x => x.IDZL);
                    table.ForeignKey(
                        name: "FK_ZakLeks_Leks_LekIDL",
                        column: x => x.LekIDL,
                        principalTable: "Leks",
                        principalColumn: "IDL");
                    table.ForeignKey(
                        name: "FK_ZakLeks_Zakazs_ZakazIDZ",
                        column: x => x.ZakazIDZ,
                        principalTable: "Zakazs",
                        principalColumn: "IDZ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leks_AkqiaIDAk",
                table: "Leks",
                column: "AkqiaIDAk");

            migrationBuilder.CreateIndex(
                name: "IX_Leks_ProizIDPr",
                table: "Leks",
                column: "ProizIDPr");

            migrationBuilder.CreateIndex(
                name: "IX_Leks_VidZIDV",
                table: "Leks",
                column: "VidZIDV");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdRole",
                table: "Users",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_Zakazs_UsersIdUser",
                table: "Zakazs",
                column: "UsersIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_ZakLeks_LekIDL",
                table: "ZakLeks",
                column: "LekIDL");

            migrationBuilder.CreateIndex(
                name: "IX_ZakLeks_ZakazIDZ",
                table: "ZakLeks",
                column: "ZakazIDZ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZakLeks");

            migrationBuilder.DropTable(
                name: "Leks");

            migrationBuilder.DropTable(
                name: "Zakazs");

            migrationBuilder.DropTable(
                name: "Akqias");

            migrationBuilder.DropTable(
                name: "Proizs");

            migrationBuilder.DropTable(
                name: "Vids");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
