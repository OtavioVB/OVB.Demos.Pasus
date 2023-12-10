using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Domain.StudentContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Mappings;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore;

public sealed class DataContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StudentMapping());
        modelBuilder.ApplyConfiguration(new TenantMapping());
        base.OnModelCreating(modelBuilder);
    }
}
