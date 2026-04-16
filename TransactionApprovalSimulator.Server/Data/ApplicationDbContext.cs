using Microsoft.EntityFrameworkCore;
using TransactionApprovalSimulator.Server.Models;

namespace TransactionApprovalSimulator.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; } = null!;
    }
}