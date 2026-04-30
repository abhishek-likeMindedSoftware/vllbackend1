using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EFCore.AuditTrail;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Entities.Faq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Infrastructure.Data;

/// <summary>
/// API DbContext — second window into the same database as LemonLawDbContext (XAF).
/// Uses Snapshot change tracking (no proxies) and JWT-based user resolution.
/// Does NOT own migrations — schema is managed by LemonLawDbContext via XAF.
/// </summary>
public class LemonLawAPIDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public LemonLawAPIDbContext(
        DbContextOptions<LemonLawAPIDbContext> options,
        IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // ── XAF framework tables ──────────────────────────────────────────────────
    public DbSet<ModelDifference> ModelDifferences { get; set; }
    public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
    public DbSet<PermissionPolicyRole> Roles { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationUserLoginInfo> UserLoginsInfo { get; set; }
    public DbSet<ReportDataV2> ReportDataV2 { get; set; }
    public DbSet<DashboardData> DashboardData { get; set; }
    public DbSet<AuditDataItemPersistent> AuditData { get; set; }
    public DbSet<AuditEFCoreWeakReference> AuditEFCoreWeakReferences { get; set; }

    // ── Business tables ───────────────────────────────────────────────────────
    public DbSet<AppEntity> Applications => Set<AppEntity>();
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Defect> Defects => Set<Defect>();
    public DbSet<RepairAttempt> RepairAttempts => Set<RepairAttempt>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ApplicationDocument> ApplicationDocuments => Set<ApplicationDocument>();
    public DbSet<ApplicationToken> ApplicationTokens => Set<ApplicationToken>();
    public DbSet<CaseEvent> CaseEvents => Set<CaseEvent>();
    public DbSet<CaseNote> CaseNotes => Set<CaseNote>();
    public DbSet<Correspondence> Correspondences => Set<Correspondence>();
    public DbSet<DealerOutreach> DealerOutreaches => Set<DealerOutreach>();
    public DbSet<DealerResponse> DealerResponses => Set<DealerResponse>();
    public DbSet<Hearing> Hearings => Set<Hearing>();
    public DbSet<Decision> Decisions => Set<Decision>();
    public DbSet<CorrespondenceTemplate> CorrespondenceTemplates => Set<CorrespondenceTemplate>();
    public DbSet<FaqQuestion> FaqQuestions => Set<FaqQuestion>();
    public DbSet<FaqAnswer> FaqAnswers => Set<FaqAnswer>();

    // ── SaveChanges overrides ─────────────────────────────────────────────────

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        HandleSoftDeletes();
        UpdateAuditFields();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        HandleSoftDeletes();
        UpdateAuditFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    // ── Soft delete — uses JWT ClaimsPrincipal ────────────────────────────────

    private void HandleSoftDeletes()
    {
        var currentUserId = GetCurrentUserId();

        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Deleted && e.Entity is AuditDetails))
        {
            entry.State = EntityState.Modified;
            var entity = (AuditDetails)entry.Entity;
            entity.IsDeleted = true;
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedById = currentUserId;
        }
    }

    // ── Audit fields — uses JWT ClaimsPrincipal ───────────────────────────────

    private void UpdateAuditFields()
    {
        var currentUserId = GetCurrentUserId();
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.Entity is AuditDetails &&
                                 (e.State == EntityState.Added || e.State == EntityState.Modified)))
        {
            var entity = (AuditDetails)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedDate = now;
                entity.CreatedById = currentUserId;
            }

            entity.ModifiedDate = now;
            entity.ModifiedById = currentUserId;
        }
    }

    private Guid? GetCurrentUserId()
    {
        var claim = _httpContextAccessor?.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Use Snapshot tracking — no proxy requirement, compatible with plain auto-properties
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        // ── XAF framework entity configurations ───────────────────────────────
        modelBuilder.Entity<ApplicationUserLoginInfo>(b =>
        {
            b.HasIndex(
                nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.LoginProviderName),
                nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.ProviderUserKey))
             .IsUnique();
        });
        modelBuilder.Entity<ModelDifference>()
            .HasMany(t => t.Aspects)
            .WithOne(t => t.Owner)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<AuditEFCoreWeakReference>()
            .HasMany(p => p.AuditItems).WithOne(p => p.AuditedObject);
        modelBuilder.Entity<AuditEFCoreWeakReference>()
            .HasMany(p => p.OldItems).WithOne(p => p.OldObject);
        modelBuilder.Entity<AuditEFCoreWeakReference>()
            .HasMany(p => p.NewItems).WithOne(p => p.NewObject);
        modelBuilder.Entity<AuditEFCoreWeakReference>()
            .HasMany(p => p.UserItems).WithOne(p => p.UserObject);

        // ── Business entity configurations ────────────────────────────────────

        // Application
        modelBuilder.Entity<AppEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.HasIndex(x => x.CaseNumber).IsUnique();
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.ApplicationType).HasConversion<string>();
            e.Property(x => x.DesiredResolution).HasConversion<string>();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Applicant
        modelBuilder.Entity<Applicant>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.HasIndex(x => x.ApplicationId).IsUnique();
            e.Property(x => x.PhoneType).HasConversion<string>();
            e.Property(x => x.PreferredContact).HasConversion<string>();
            e.HasOne(x => x.Application).WithOne(x => x.Applicant)
                .HasForeignKey<Applicant>(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Vehicle
        modelBuilder.Entity<Vehicle>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.HasIndex(x => x.ApplicationId).IsUnique();
            e.HasIndex(x => x.VIN);
            e.Property(x => x.WarrantyType).HasConversion<string>();
            e.HasOne(x => x.Application).WithOne(x => x.Vehicle)
                .HasForeignKey<Vehicle>(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Defect
        modelBuilder.Entity<Defect>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.DefectCategory).HasConversion<string>();
            e.HasOne(x => x.Application).WithMany(x => x.Defects)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // RepairAttempt
        modelBuilder.Entity<RepairAttempt>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.HasOne(x => x.Application).WithMany(x => x.RepairAttempts)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Expense
        modelBuilder.Entity<Expense>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.ExpenseType).HasConversion<string>();
            e.HasOne(x => x.Application).WithMany(x => x.Expenses)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // ApplicationDocument
        modelBuilder.Entity<ApplicationDocument>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.DocumentType).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.VirusScanResult).HasConversion<string>();
            e.Property(x => x.UploadedByRole).HasConversion<string>();
            e.HasOne(x => x.Application).WithMany(x => x.Documents)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // ApplicationToken
        modelBuilder.Entity<ApplicationToken>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.HasIndex(x => x.TokenHash).IsUnique();
            e.Property(x => x.TokenType).HasConversion<string>();
            e.HasOne(x => x.Application).WithOne(x => x.Token)
                .HasForeignKey<ApplicationToken>(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CaseEvent — append-only, no soft delete
        modelBuilder.Entity<CaseEvent>(e =>
        {
            e.HasKey(x => x.CaseEventId);
            e.Property(x => x.CaseEventId).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.EventType).HasConversion<string>();
            e.Property(x => x.ActorType).HasConversion<string>();
            e.HasOne(x => x.Application).WithMany(x => x.Events)
                .HasForeignKey(x => x.ApplicationId);
        });

        // CaseNote
        modelBuilder.Entity<CaseNote>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.HasOne(x => x.Application).WithMany(x => x.Notes)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Correspondence
        modelBuilder.Entity<Correspondence>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.Direction).HasConversion<string>();
            e.Property(x => x.RecipientType).HasConversion<string>();
            e.Property(x => x.DeliveryStatus).HasConversion<string>();
            e.HasOne(x => x.Application).WithMany(x => x.Correspondences)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // DealerOutreach
        modelBuilder.Entity<DealerOutreach>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.OutreachType).HasConversion<string>();
            e.Property(x => x.DeliveryStatus).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.HasOne(x => x.Application).WithMany(x => x.DealerOutreaches)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // DealerResponse
        modelBuilder.Entity<DealerResponse>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.DealerPosition).HasConversion<string>();
            e.HasOne(x => x.Outreach).WithOne(x => x.Response)
                .HasForeignKey<DealerResponse>(x => x.OutreachId)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Hearing
        modelBuilder.Entity<Hearing>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.HearingFormat).HasConversion<string>();
            e.Property(x => x.Outcome).HasConversion<string>();
            e.HasOne(x => x.Application).WithMany(x => x.Hearings)
                .HasForeignKey(x => x.ApplicationId);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Decision
        modelBuilder.Entity<Decision>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(x => x.DecisionType).HasConversion<string>();
            e.HasOne(x => x.Application).WithOne(x => x.Decision)
                .HasForeignKey<Decision>(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // CorrespondenceTemplate
        modelBuilder.Entity<CorrespondenceTemplate>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.HasIndex(x => x.TemplateCode).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<FaqQuestion>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.FaqQuestion)
                .HasForeignKey(a => a.FaqQuestionId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
