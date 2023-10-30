using Microsoft.EntityFrameworkCore;
using SharedLibrary.Domain.Entities;

namespace Api.Infra.Conntexts
{
    public class PixContext : DbContext
    {
        public PixContext(DbContextOptions<PixContext> op) : base(op)
        {
        }

        public PixContext() : base()
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Account> Account { get; set; }
    }
}
