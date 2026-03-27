using Microsoft.EntityFrameworkCore;

namespace BackendExam.Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MyOffice_ACPD> MyOffice_ACPD { get; set; }
    }
}