using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Domain.StudentContext.DataTransferObject;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class StudentRepository : BaseRepository<Student>, IExtensionStudentRepository
{
    public StudentRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> VerifyStudentExistsByEmailAsync(string email, CancellationToken cancellationToken)
        => _dbContext.Set<Student>().Where(p => p.Email == email).AnyAsync(cancellationToken);
}
