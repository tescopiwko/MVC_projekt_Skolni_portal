using Microsoft.EntityFrameworkCore;
using MVC_projekt_Skolni_portal.Models;

namespace MVC_projekt_Skolni_portal.Data
{
    public class KontextDatabaze : DbContext
    {
        public KontextDatabaze(DbContextOptions<KontextDatabaze> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Grade> Grades { get; set; }
    }
    
    
}

