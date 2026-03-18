using apteka.Models;
using Microsoft.EntityFrameworkCore;

namespace apteka.Data
{
    public class ApplicationDbContext2: DbContext
    {
        public ApplicationDbContext2(DbContextOptions<ApplicationDbContext2> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Akqia> Akqias { get; set; }
        public DbSet<Lek> Leks { get; set; }
        public DbSet<Proiz> Proizs { get; set; }
        public DbSet<VidZ> Vids { get; set; }
        public DbSet<Zakaz> Zakazs { get; set; }
        public DbSet<ZakLek> ZakLeks { get; set; }
    }

}
