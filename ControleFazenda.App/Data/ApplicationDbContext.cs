using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ControleFazenda.App.ViewModels;

namespace ControleFazenda.App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ControleFazenda.App.ViewModels.ValeVM> ValeVM { get; set; } = default!;
        
    }
}
