using Microsoft.EntityFrameworkCore;
using FinancialTracker.Entities;
using FinancialTracker.Entities.Accounts;
using FinancialTracker.Services;

namespace FinancialTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<PersonalAccount> PersonalAccounts { get; set; }
        public DbSet<SharedAccount> SharedAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<RecoveryCode> RecoveryCodes { get; set; }

        public DbSet<AccountHistoryEntry> AccountHistoryEntries { get; set; }

        public DbSet<TransactionEditHistory> TransactionEditHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=financial.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка наследования
            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("AccountType")
                .HasValue<PersonalAccount>("Personal")
                .HasValue<SharedAccount>("Shared");

            modelBuilder.Entity<Account>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<PersonalAccount>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId);

            // Настройка SharedAccount
            modelBuilder.Entity<SharedAccount>()
                .Property(s => s.CreatorUserId)
                .IsRequired();

            // Настройка истории
            modelBuilder.Entity<AccountHistoryEntry>()
                .HasOne<SharedAccount>()
                .WithMany(a => a.History)
                .HasForeignKey("SharedAccountId");

            modelBuilder.Entity<TransactionEditHistory>()
                .HasOne<Transaction>()
                .WithMany(t => t.EditHistory)
                .HasForeignKey(e => e.Id);

            modelBuilder.Entity<Budget>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Настройка Transaction
            modelBuilder.Entity<Transaction>()
                .HasMany(t => t.EditHistory)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка AccountHistoryEntry
            modelBuilder.Entity<AccountHistoryEntry>()
                .Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}