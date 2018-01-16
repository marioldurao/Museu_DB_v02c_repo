using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Museu_DB_v02c.Models
{
    public class Museu_DB_v02cContext : DbContext
    {
        public Museu_DB_v02cContext (DbContextOptions<Museu_DB_v02cContext> options)
            : base(options)
        {
        }

        public DbSet<Museu_DB_v02c.Models.Visitor> Visitor { get; set; }
    }
}
