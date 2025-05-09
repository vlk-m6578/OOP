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
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        public AppDbContext()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database creation error: {ex.Message}");
            }
        }
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
                    .Property(s => s.MemberUserIds)
                    .IsRequired()
                    .HasDefaultValue("");

            // Настройка истории
            modelBuilder.Entity<AccountHistoryEntry>()
                .HasOne<SharedAccount>()
                .WithMany(a => a.History)
                .HasForeignKey("SharedAccountId");

            modelBuilder.Entity<TransactionEditHistory>()
                .HasOne(h => h.Transaction)
                .WithMany(t => t.EditHistory)
                .HasForeignKey(h => h.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Настройка Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка AccountHistoryEntry
            modelBuilder.Entity<AccountHistoryEntry>()
                .Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.SharedAccount)
                .WithMany()
                .HasForeignKey(i => i.SharedAccountId);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.InvitedUser)
                .WithMany()
                .HasForeignKey(i => i.InvitedUserId);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.InviterUser)
                .WithMany()
                .HasForeignKey(i => i.InviterUserId);

            modelBuilder.Entity<SharedAccount>()
                .HasMany(s => s.History)
                .WithOne(e => e.SharedAccount)
                .HasForeignKey(e => e.SharedAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}