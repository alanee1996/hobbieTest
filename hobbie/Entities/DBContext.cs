using System;
using Microsoft.EntityFrameworkCore;

namespace hobbie.Entities
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hobbie> hobbies { get; set; }
        public DbSet<User> users { get; set; }
    }
}
