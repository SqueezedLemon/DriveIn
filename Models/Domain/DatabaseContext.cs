using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveIn.Models.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opts) : base(opts)
        {}

         public DbSet<User>? User { get; set; }
         public DbSet<Profile>? Profile { get; set; }
    }
}