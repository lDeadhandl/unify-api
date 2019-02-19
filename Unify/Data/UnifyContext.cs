using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unify.Data
{
    public class UnifyContext : DbContext
    {
        public UnifyContext(DbContextOptions<UnifyContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Party> Party { get; set; }
        public DbSet<Guests> Guests { get; set; }
    }
}
