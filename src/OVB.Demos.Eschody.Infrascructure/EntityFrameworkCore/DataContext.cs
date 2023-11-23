using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Domain.StudentContext;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore;

public sealed class DataContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
