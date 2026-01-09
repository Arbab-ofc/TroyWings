using Microsoft.EntityFrameworkCore;
using TroyWingsApp.Models;

namespace TroyWingsApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Registration> Registrations => Set<Registration>();
}
