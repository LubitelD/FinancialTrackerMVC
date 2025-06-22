using Microsoft.EntityFrameworkCore;
using FinancialTrackerDomain.Model;

namespace FinancialTrackerDomain;

public partial class FinancialTrackerContext : DbContext
{
    public FinancialTrackerContext()
    {
    }

    public FinancialTrackerContext(DbContextOptions<FinancialTrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FinancialGoal> FinancialGoals { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2UQOBQK\\SQLEXPRESS; Database=financialTracker2; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id);
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id);

            entity
                .Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id);
            entity.Property(e => e.Description);
            entity.Property(e => e.CategoryType);
            entity
                .Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

        });

        modelBuilder.Entity<FinancialGoal>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id);
            entity.Property(e => e.SavedAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TargetAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.FinancialGoals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity
                .HasOne(d => d.Budget)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BudgetId);

            entity
                .HasOne(d => d.FinancialGoal)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FinancialGoalId);

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
