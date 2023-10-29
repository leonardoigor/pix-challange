using Api.Core;
using Microsoft.Extensions.Options;
using SharedLibrary.Domain.Entities;
using System.Data.Entity;

namespace Api.Infra.Conntexts
{
    public class DbContext1 : DbContext
    {
        public DbContext1(IOptions<Env> env) : base(env.Value.ConnectionString)
        {
        }

        public DbContext1():base()
        {
        }
 
        public DbSet<User> User { get; set; }




    }
}
