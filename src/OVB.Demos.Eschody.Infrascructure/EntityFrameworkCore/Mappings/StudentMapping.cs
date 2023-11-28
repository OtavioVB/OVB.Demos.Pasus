using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OVB.Demos.Eschody.Domain.StudentContext;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Mappings;

public sealed class StudentMapping : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        throw new NotImplementedException();
    }
}
