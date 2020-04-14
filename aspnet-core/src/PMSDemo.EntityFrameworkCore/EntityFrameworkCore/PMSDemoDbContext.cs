using PMSDemo.PriorityAreas;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMSDemo.Authorization.Delegation;
using PMSDemo.Authorization.Roles;
using PMSDemo.Authorization.Users;
using PMSDemo.Chat;
using PMSDemo.Editions;
using PMSDemo.Friendships;
using PMSDemo.MultiTenancy;
using PMSDemo.MultiTenancy.Accounting;
using PMSDemo.MultiTenancy.Payments;
using PMSDemo.Storage;
using PMSDemo.Agencies;
using PMSDemo.Deliverables;
using PMSDemo.PerformanceIndicators;
using PMSDemo.PerformanceActivities;

namespace PMSDemo.EntityFrameworkCore
{
    public class PMSDemoDbContext : AbpZeroDbContext<Tenant, Role, User, PMSDemoDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<ActivityAttachment> ActivityAttachments { get; set; }
        public virtual DbSet<IndicatorAttachment> IndicatorAttachments { get; set; }
        public virtual DbSet<IndicatorYearlyTarget> IndicatorYearlyTargets { get; set; }
        public virtual DbSet<ActivityUpdateLog> ActivityUpdateLogs { get; set; }
        public virtual DbSet<IndicatorUpdateLog> IndicatorUpdateLogs { get; set; }
        public virtual DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public virtual DbSet<PerformanceActivity> PerformanceActivities { get; set; }
        public virtual DbSet<PerformanceIndicator> PerformanceIndicators { get; set; }
        public virtual DbSet<Deliverable> Deliverables { get; set; }
        public virtual DbSet<Mda> Agencies { get; set; }
        public virtual DbSet<PriorityArea> PriorityAreas { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public PMSDemoDbContext(DbContextOptions<PMSDemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
