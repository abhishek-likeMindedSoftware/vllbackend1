using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LemonLaw.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEFCoreWeakReferences",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TypeName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DefaultString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEFCoreWeakReferences", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DashboardData",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SynchronizeTitle = table.Column<bool>(type: "bit", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ModelDifferences",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContextId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelDifferences", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyRoleBase",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdministrative = table.Column<bool>(type: "bit", nullable: false),
                    CanEditModel = table.Column<bool>(type: "bit", nullable: false),
                    PermissionPolicy = table.Column<int>(type: "int", nullable: false),
                    IsAllowPermissionPriority = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyRoleBase", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyUser",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ChangePasswordOnFirstLogon = table.Column<bool>(type: "bit", nullable: false),
                    StoredPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: true),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyUser", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReportDataV2",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInplaceReport = table.Column<bool>(type: "bit", nullable: false),
                    PredefinedReportTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParametersObjectTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDataV2", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AuditData",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditedObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NewObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AuditData_AuditEFCoreWeakReferences_AuditedObjectID",
                        column: x => x.AuditedObjectID,
                        principalTable: "AuditEFCoreWeakReferences",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AuditData_AuditEFCoreWeakReferences_NewObjectID",
                        column: x => x.NewObjectID,
                        principalTable: "AuditEFCoreWeakReferences",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AuditData_AuditEFCoreWeakReferences_OldObjectID",
                        column: x => x.OldObjectID,
                        principalTable: "AuditEFCoreWeakReferences",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AuditData_AuditEFCoreWeakReferences_UserObjectID",
                        column: x => x.UserObjectID,
                        principalTable: "AuditEFCoreWeakReferences",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ModelDifferenceAspects",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelDifferenceAspects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModelDifferenceAspects_ModelDifferences_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "ModelDifferences",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyActionPermissionObject",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyActionPermissionObject", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyActionPermissionObject_PermissionPolicyRoleBase_RoleID",
                        column: x => x.RoleID,
                        principalTable: "PermissionPolicyRoleBase",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyNavigationPermissionObject",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetTypeFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NavigateState = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyNavigationPermissionObject", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyNavigationPermissionObject_PermissionPolicyRoleBase_RoleID",
                        column: x => x.RoleID,
                        principalTable: "PermissionPolicyRoleBase",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyTypePermissionObject",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetTypeFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReadState = table.Column<int>(type: "int", nullable: true),
                    WriteState = table.Column<int>(type: "int", nullable: true),
                    CreateState = table.Column<int>(type: "int", nullable: true),
                    DeleteState = table.Column<int>(type: "int", nullable: true),
                    NavigateState = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyTypePermissionObject", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyTypePermissionObject_PermissionPolicyRoleBase_RoleID",
                        column: x => x.RoleID,
                        principalTable: "PermissionPolicyRoleBase",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CaseNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedToStaffId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedByStaffId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    CertificationAccepted = table.Column<bool>(type: "bit", nullable: false),
                    SignatureFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmissionIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionUserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NarrativeStatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorContactDealer = table.Column<bool>(type: "bit", nullable: true),
                    PriorContactMfr = table.Column<bool>(type: "bit", nullable: true),
                    PriorContactNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesiredResolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Applications_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CorrespondenceTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TemplateCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MergeFields = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondenceTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrespondenceTemplates_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CorrespondenceTemplates_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyRolePermissionPolicyUser",
                columns: table => new
                {
                    RolesID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyRolePermissionPolicyUser", x => new { x.RolesID, x.UsersID });
                    table.ForeignKey(
                        name: "FK_PermissionPolicyRolePermissionPolicyUser_PermissionPolicyRoleBase_RolesID",
                        column: x => x.RolesID,
                        principalTable: "PermissionPolicyRoleBase",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyRolePermissionPolicyUser_PermissionPolicyUser_UsersID",
                        column: x => x.UsersID,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyUserLoginInfo",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProviderName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderUserKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserForeignKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyUserLoginInfo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyUserLoginInfo_PermissionPolicyUser_UserForeignKey",
                        column: x => x.UserForeignKey,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyMemberPermissionsObject",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Members = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Criteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadState = table.Column<int>(type: "int", nullable: true),
                    WriteState = table.Column<int>(type: "int", nullable: true),
                    TypePermissionObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyMemberPermissionsObject", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyMemberPermissionsObject_PermissionPolicyTypePermissionObject_TypePermissionObjectID",
                        column: x => x.TypePermissionObjectID,
                        principalTable: "PermissionPolicyTypePermissionObject",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyObjectPermissionsObject",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Criteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadState = table.Column<int>(type: "int", nullable: true),
                    WriteState = table.Column<int>(type: "int", nullable: true),
                    DeleteState = table.Column<int>(type: "int", nullable: true),
                    NavigateState = table.Column<int>(type: "int", nullable: true),
                    TypePermissionObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyObjectPermissionsObject", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyObjectPermissionsObject_PermissionPolicyTypePermissionObject_TypePermissionObjectID",
                        column: x => x.TypePermissionObjectID,
                        principalTable: "PermissionPolicyTypePermissionObject",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GooglePlaceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicants_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Applicants_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Applicants_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedByRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VirusScanResult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible_Consumer = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible_Dealer = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByStaffId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationTokens_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CaseEvents",
                columns: table => new
                {
                    CaseEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActorDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseEvents", x => x.CaseEventId);
                    table.ForeignKey(
                        name: "FK_CaseEvents_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByStaffId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseNotes_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseNotes_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CaseNotes_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Correspondences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyPreview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentByStaffId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendGridMessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Correspondences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Correspondences_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Correspondences_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Correspondences_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DealerOutreaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutreachType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentByStaffId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseDeadline = table.Column<DateOnly>(type: "date", nullable: false),
                    SendGridMessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FollowUp1SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FollowUp2SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalNoticeSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EscalatedToHearing = table.Column<bool>(type: "bit", nullable: false),
                    EscalationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOutreaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerOutreaches_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerOutreaches_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DealerOutreaches_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Decisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DecisionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DecisionDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ComplianceDeadline = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionIssuedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerNotified = table.Column<bool>(type: "bit", nullable: false),
                    DealerNotified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decisions_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Decisions_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Decisions_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Defects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefectDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefectCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstOccurrenceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsOngoing = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defects_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Defects_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Defects_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpenseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptUploaded = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Expenses_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Hearings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HearingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HearingFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HearingLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArbitratorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsumerNoticesSent = table.Column<bool>(type: "bit", nullable: false),
                    DealerNoticesSent = table.Column<bool>(type: "bit", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutcomeNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContinuedTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hearings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hearings_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hearings_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Hearings_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RepairAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepairDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RepairFacilityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepairFacilityAddr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepairFacilityPlaceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MileageAtRepair = table.Column<int>(type: "int", nullable: true),
                    DefectsAddressed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepairSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    DaysOutOfService = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairAttempts_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairAttempts_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RepairAttempts_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VinConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    VinDecoded_Year = table.Column<short>(type: "smallint", nullable: true),
                    VinDecoded_Make = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VinDecoded_Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VinDecoded_Trim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleYear = table.Column<short>(type: "smallint", nullable: false),
                    VehicleMake = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicensePlateState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MileageAtPurchase = table.Column<int>(type: "int", nullable: false),
                    CurrentMileage = table.Column<int>(type: "int", nullable: false),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerAddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerAddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerZip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerGooglePlaceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarrantyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarrantyStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WarrantyExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Vehicles_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DealerResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    OutreachId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponderTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponderEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponderPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseNarrative = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepairHistoryNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SettlementOffer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SettlementDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificationAccepted = table.Column<bool>(type: "bit", nullable: false),
                    CertifierFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerResponses_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerResponses_DealerOutreaches_OutreachId",
                        column: x => x.OutreachId,
                        principalTable: "DealerOutreaches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DealerResponses_PermissionPolicyUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DealerResponses_PermissionPolicyUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_ApplicationId",
                table: "Applicants",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CreatedById",
                table: "Applicants",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_ModifiedById",
                table: "Applicants",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_ApplicationId",
                table: "ApplicationDocuments",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_CreatedById",
                table: "ApplicationDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_ModifiedById",
                table: "ApplicationDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CaseNumber",
                table: "Applications",
                column: "CaseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CreatedById",
                table: "Applications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ModifiedById",
                table: "Applications",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTokens_ApplicationId",
                table: "ApplicationTokens",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTokens_TokenHash",
                table: "ApplicationTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditData_AuditedObjectID",
                table: "AuditData",
                column: "AuditedObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditData_NewObjectID",
                table: "AuditData",
                column: "NewObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditData_OldObjectID",
                table: "AuditData",
                column: "OldObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditData_UserObjectID",
                table: "AuditData",
                column: "UserObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEFCoreWeakReferences_Key_TypeName",
                table: "AuditEFCoreWeakReferences",
                columns: new[] { "Key", "TypeName" });

            migrationBuilder.CreateIndex(
                name: "IX_CaseEvents_ApplicationId",
                table: "CaseEvents",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseNotes_ApplicationId",
                table: "CaseNotes",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseNotes_CreatedById",
                table: "CaseNotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CaseNotes_ModifiedById",
                table: "CaseNotes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Correspondences_ApplicationId",
                table: "Correspondences",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Correspondences_CreatedById",
                table: "Correspondences",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Correspondences_ModifiedById",
                table: "Correspondences",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenceTemplates_CreatedById",
                table: "CorrespondenceTemplates",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenceTemplates_ModifiedById",
                table: "CorrespondenceTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenceTemplates_TemplateCode",
                table: "CorrespondenceTemplates",
                column: "TemplateCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerOutreaches_ApplicationId",
                table: "DealerOutreaches",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOutreaches_CreatedById",
                table: "DealerOutreaches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOutreaches_ModifiedById",
                table: "DealerOutreaches",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DealerResponses_ApplicationId",
                table: "DealerResponses",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerResponses_CreatedById",
                table: "DealerResponses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DealerResponses_ModifiedById",
                table: "DealerResponses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DealerResponses_OutreachId",
                table: "DealerResponses",
                column: "OutreachId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_ApplicationId",
                table: "Decisions",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_CreatedById",
                table: "Decisions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_ModifiedById",
                table: "Decisions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_ApplicationId",
                table: "Defects",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_CreatedById",
                table: "Defects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_ModifiedById",
                table: "Defects",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ApplicationId",
                table: "Expenses",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CreatedById",
                table: "Expenses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ModifiedById",
                table: "Expenses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Hearings_ApplicationId",
                table: "Hearings",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Hearings_CreatedById",
                table: "Hearings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Hearings_ModifiedById",
                table: "Hearings",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ModelDifferenceAspects_OwnerID",
                table: "ModelDifferenceAspects",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyActionPermissionObject_RoleID",
                table: "PermissionPolicyActionPermissionObject",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyMemberPermissionsObject_TypePermissionObjectID",
                table: "PermissionPolicyMemberPermissionsObject",
                column: "TypePermissionObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyNavigationPermissionObject_RoleID",
                table: "PermissionPolicyNavigationPermissionObject",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyObjectPermissionsObject_TypePermissionObjectID",
                table: "PermissionPolicyObjectPermissionsObject",
                column: "TypePermissionObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyRolePermissionPolicyUser_UsersID",
                table: "PermissionPolicyRolePermissionPolicyUser",
                column: "UsersID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyTypePermissionObject_RoleID",
                table: "PermissionPolicyTypePermissionObject",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyUserLoginInfo_LoginProviderName_ProviderUserKey",
                table: "PermissionPolicyUserLoginInfo",
                columns: new[] { "LoginProviderName", "ProviderUserKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyUserLoginInfo_UserForeignKey",
                table: "PermissionPolicyUserLoginInfo",
                column: "UserForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_RepairAttempts_ApplicationId",
                table: "RepairAttempts",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairAttempts_CreatedById",
                table: "RepairAttempts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RepairAttempts_ModifiedById",
                table: "RepairAttempts",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ApplicationId",
                table: "Vehicles",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CreatedById",
                table: "Vehicles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModifiedById",
                table: "Vehicles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VIN",
                table: "Vehicles",
                column: "VIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "ApplicationDocuments");

            migrationBuilder.DropTable(
                name: "ApplicationTokens");

            migrationBuilder.DropTable(
                name: "AuditData");

            migrationBuilder.DropTable(
                name: "CaseEvents");

            migrationBuilder.DropTable(
                name: "CaseNotes");

            migrationBuilder.DropTable(
                name: "Correspondences");

            migrationBuilder.DropTable(
                name: "CorrespondenceTemplates");

            migrationBuilder.DropTable(
                name: "DashboardData");

            migrationBuilder.DropTable(
                name: "DealerResponses");

            migrationBuilder.DropTable(
                name: "Decisions");

            migrationBuilder.DropTable(
                name: "Defects");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Hearings");

            migrationBuilder.DropTable(
                name: "ModelDifferenceAspects");

            migrationBuilder.DropTable(
                name: "PermissionPolicyActionPermissionObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyMemberPermissionsObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyNavigationPermissionObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyObjectPermissionsObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyRolePermissionPolicyUser");

            migrationBuilder.DropTable(
                name: "PermissionPolicyUserLoginInfo");

            migrationBuilder.DropTable(
                name: "RepairAttempts");

            migrationBuilder.DropTable(
                name: "ReportDataV2");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "AuditEFCoreWeakReferences");

            migrationBuilder.DropTable(
                name: "DealerOutreaches");

            migrationBuilder.DropTable(
                name: "ModelDifferences");

            migrationBuilder.DropTable(
                name: "PermissionPolicyTypePermissionObject");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "PermissionPolicyRoleBase");

            migrationBuilder.DropTable(
                name: "PermissionPolicyUser");
        }
    }
}
