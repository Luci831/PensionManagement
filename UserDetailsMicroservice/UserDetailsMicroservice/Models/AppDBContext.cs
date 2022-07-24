using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserDetailsMicroservice.Models;

namespace UserDetailsMicroservice.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public DbSet<PensionerDetails> PensionerDetails { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
