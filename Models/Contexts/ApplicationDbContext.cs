using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Models.Accounting;
using Models.Models.HiAccounting;
using Models.Models.HiAccounting.Debs;
using Models.Models.HiLoans;
using Models.Models.Shared;

namespace Models.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ClientUser>().HasQueryFilter(x => x.State != Enums.State.Removed);
            builder.Entity<Movement>().HasQueryFilter(x => x.State != Enums.State.Removed && x.State != Enums.State.Payment);
            builder.Entity<Loan>().HasQueryFilter(x => x.State != Enums.State.Removed);
            builder.Entity<Deb>().HasQueryFilter(x => x.State != Enums.State.Removed);
            builder.Entity<Bank>().HasQueryFilter(x => x.State != Enums.State.Removed);

        }

        #region Shared
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Company> Companies { get; set; }
        #endregion


        #region HiAccounting
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        #endregion

        #region HiLoans
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Deb> Debs { get; set; }
        public DbSet<ReclosingHistory> ReclosingHistories { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<HistoryPaymentsLoan> HistoryPaymentsLoan { get; set; }
        public DbSet<HistoryOnlyInterest> HistoryOnlyInterests { get; set; }

        #endregion


        #region HIInventaty
        public DbSet<Article> Articles { get; set; }
        #endregion

        #region Users
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }
        #endregion
    }
}
