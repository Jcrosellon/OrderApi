using Microsoft.EntityFrameworkCore;

namespace OrderApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MessageRequest> Messages { get; set; }
    }
}
