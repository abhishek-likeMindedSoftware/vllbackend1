using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LemonLaw.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameApplicationToVllApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop all FKs that reference the Applications table
            migrationBuilder.DropForeignKey(name: "FK_Applicants_Applications_ApplicationId",          table: "Applicants");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationDocuments_Applications_ApplicationId", table: "ApplicationDocuments");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationTokens_Applications_ApplicationId",   table: "ApplicationTokens");
            migrationBuilder.DropForeignKey(name: "FK_CaseEvents_Applications_ApplicationId",           table: "CaseEvents");
            migrationBuilder.DropForeignKey(name: "FK_CaseNotes_Applications_ApplicationId",            table: "CaseNotes");
            migrationBuilder.DropForeignKey(name: "FK_Correspondences_Applications_ApplicationId",      table: "Correspondences");
            migrationBuilder.DropForeignKey(name: "FK_DealerOutreaches_Applications_ApplicationId",     table: "DealerOutreaches");
            migrationBuilder.DropForeignKey(name: "FK_DealerResponses_Applications_ApplicationId",      table: "DealerResponses");
            migrationBuilder.DropForeignKey(name: "FK_Decisions_Applications_ApplicationId",            table: "Decisions");
            migrationBuilder.DropForeignKey(name: "FK_Defects_Applications_ApplicationId",              table: "Defects");
            migrationBuilder.DropForeignKey(name: "FK_Expenses_Applications_ApplicationId",             table: "Expenses");
            migrationBuilder.DropForeignKey(name: "FK_Hearings_Applications_ApplicationId",             table: "Hearings");
            migrationBuilder.DropForeignKey(name: "FK_RepairAttempts_Applications_ApplicationId",       table: "RepairAttempts");
            migrationBuilder.DropForeignKey(name: "FK_Vehicles_Applications_ApplicationId",             table: "Vehicles");
            migrationBuilder.DropForeignKey(name: "FK_Applications_PermissionPolicyUser_CreatedById",   table: "Applications");
            migrationBuilder.DropForeignKey(name: "FK_Applications_PermissionPolicyUser_ModifiedById",  table: "Applications");

            // Drop PK and indexes on Applications before rename
            migrationBuilder.DropPrimaryKey(name: "PK_Applications", table: "Applications");

            migrationBuilder.RenameIndex(name: "IX_Applications_ModifiedById", table: "Applications", newName: "IX_VllApplications_ModifiedById");
            migrationBuilder.RenameIndex(name: "IX_Applications_CreatedById",  table: "Applications", newName: "IX_VllApplications_CreatedById");
            migrationBuilder.RenameIndex(name: "IX_Applications_CaseNumber",   table: "Applications", newName: "IX_VllApplications_CaseNumber");

            // Rename the table
            migrationBuilder.RenameTable(name: "Applications", newName: "VllApplications");

            // Restore PK on new table name
            migrationBuilder.AddPrimaryKey(name: "PK_VllApplications", table: "VllApplications", column: "Id");

            // Re-add all FKs pointing to VllApplications
            migrationBuilder.AddForeignKey(name: "FK_Applicants_VllApplications_ApplicationId",          table: "Applicants",          column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_ApplicationDocuments_VllApplications_ApplicationId", table: "ApplicationDocuments", column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_ApplicationTokens_VllApplications_ApplicationId",   table: "ApplicationTokens",   column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_CaseEvents_VllApplications_ApplicationId",           table: "CaseEvents",           column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_CaseNotes_VllApplications_ApplicationId",            table: "CaseNotes",            column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Correspondences_VllApplications_ApplicationId",      table: "Correspondences",      column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_DealerOutreaches_VllApplications_ApplicationId",     table: "DealerOutreaches",     column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_DealerResponses_VllApplications_ApplicationId",      table: "DealerResponses",      column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Decisions_VllApplications_ApplicationId",            table: "Decisions",            column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Defects_VllApplications_ApplicationId",              table: "Defects",              column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Expenses_VllApplications_ApplicationId",             table: "Expenses",             column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Hearings_VllApplications_ApplicationId",             table: "Hearings",             column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_RepairAttempts_VllApplications_ApplicationId",       table: "RepairAttempts",       column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Vehicles_VllApplications_ApplicationId",             table: "Vehicles",             column: "ApplicationId", principalTable: "VllApplications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_VllApplications_PermissionPolicyUser_CreatedById",   table: "VllApplications",      column: "CreatedById",   principalTable: "PermissionPolicyUser", principalColumn: "ID");
            migrationBuilder.AddForeignKey(name: "FK_VllApplications_PermissionPolicyUser_ModifiedById",  table: "VllApplications",      column: "ModifiedById",  principalTable: "PermissionPolicyUser", principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Applicants_VllApplications_ApplicationId",          table: "Applicants");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationDocuments_VllApplications_ApplicationId", table: "ApplicationDocuments");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationTokens_VllApplications_ApplicationId",   table: "ApplicationTokens");
            migrationBuilder.DropForeignKey(name: "FK_CaseEvents_VllApplications_ApplicationId",           table: "CaseEvents");
            migrationBuilder.DropForeignKey(name: "FK_CaseNotes_VllApplications_ApplicationId",            table: "CaseNotes");
            migrationBuilder.DropForeignKey(name: "FK_Correspondences_VllApplications_ApplicationId",      table: "Correspondences");
            migrationBuilder.DropForeignKey(name: "FK_DealerOutreaches_VllApplications_ApplicationId",     table: "DealerOutreaches");
            migrationBuilder.DropForeignKey(name: "FK_DealerResponses_VllApplications_ApplicationId",      table: "DealerResponses");
            migrationBuilder.DropForeignKey(name: "FK_Decisions_VllApplications_ApplicationId",            table: "Decisions");
            migrationBuilder.DropForeignKey(name: "FK_Defects_VllApplications_ApplicationId",              table: "Defects");
            migrationBuilder.DropForeignKey(name: "FK_Expenses_VllApplications_ApplicationId",             table: "Expenses");
            migrationBuilder.DropForeignKey(name: "FK_Hearings_VllApplications_ApplicationId",             table: "Hearings");
            migrationBuilder.DropForeignKey(name: "FK_RepairAttempts_VllApplications_ApplicationId",       table: "RepairAttempts");
            migrationBuilder.DropForeignKey(name: "FK_Vehicles_VllApplications_ApplicationId",             table: "Vehicles");
            migrationBuilder.DropForeignKey(name: "FK_VllApplications_PermissionPolicyUser_CreatedById",   table: "VllApplications");
            migrationBuilder.DropForeignKey(name: "FK_VllApplications_PermissionPolicyUser_ModifiedById",  table: "VllApplications");

            migrationBuilder.DropPrimaryKey(name: "PK_VllApplications", table: "VllApplications");

            migrationBuilder.RenameIndex(name: "IX_VllApplications_ModifiedById", table: "VllApplications", newName: "IX_Applications_ModifiedById");
            migrationBuilder.RenameIndex(name: "IX_VllApplications_CreatedById",  table: "VllApplications", newName: "IX_Applications_CreatedById");
            migrationBuilder.RenameIndex(name: "IX_VllApplications_CaseNumber",   table: "VllApplications", newName: "IX_Applications_CaseNumber");

            migrationBuilder.RenameTable(name: "VllApplications", newName: "Applications");

            migrationBuilder.AddPrimaryKey(name: "PK_Applications", table: "Applications", column: "Id");

            migrationBuilder.AddForeignKey(name: "FK_Applicants_Applications_ApplicationId",          table: "Applicants",          column: "ApplicationId", principalTable: "Applications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_ApplicationDocuments_Applications_ApplicationId", table: "ApplicationDocuments", column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_ApplicationTokens_Applications_ApplicationId",   table: "ApplicationTokens",   column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_CaseEvents_Applications_ApplicationId",           table: "CaseEvents",           column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_CaseNotes_Applications_ApplicationId",            table: "CaseNotes",            column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Correspondences_Applications_ApplicationId",      table: "Correspondences",      column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_DealerOutreaches_Applications_ApplicationId",     table: "DealerOutreaches",     column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_DealerResponses_Applications_ApplicationId",      table: "DealerResponses",      column: "ApplicationId", principalTable: "Applications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Decisions_Applications_ApplicationId",            table: "Decisions",            column: "ApplicationId", principalTable: "Applications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Defects_Applications_ApplicationId",              table: "Defects",              column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Expenses_Applications_ApplicationId",             table: "Expenses",             column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Hearings_Applications_ApplicationId",             table: "Hearings",             column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_RepairAttempts_Applications_ApplicationId",       table: "RepairAttempts",       column: "ApplicationId", principalTable: "Applications", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(name: "FK_Vehicles_Applications_ApplicationId",             table: "Vehicles",             column: "ApplicationId", principalTable: "Applications", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Applications_PermissionPolicyUser_CreatedById",   table: "Applications",         column: "CreatedById",   principalTable: "PermissionPolicyUser", principalColumn: "ID");
            migrationBuilder.AddForeignKey(name: "FK_Applications_PermissionPolicyUser_ModifiedById",  table: "Applications",         column: "ModifiedById",  principalTable: "PermissionPolicyUser", principalColumn: "ID");
        }
    }
}
