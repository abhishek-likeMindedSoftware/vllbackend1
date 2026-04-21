using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LemonLaw.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FAQEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaqQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqQuestions_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_FaqQuestions_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FaqAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FaqQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqAnswers_FaqQuestions_FaqQuestionId",
                        column: x => x.FaqQuestionId,
                        principalTable: "FaqQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FaqAnswers_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_FaqAnswers_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaqAnswers_CreatedById",
                table: "FaqAnswers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqAnswers_FaqQuestionId",
                table: "FaqAnswers",
                column: "FaqQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqAnswers_ModifiedById",
                table: "FaqAnswers",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestions_CreatedById",
                table: "FaqQuestions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestions_ModifiedById",
                table: "FaqQuestions",
                column: "ModifiedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaqAnswers");

            migrationBuilder.DropTable(
                name: "FaqQuestions");
        }
    }
}
