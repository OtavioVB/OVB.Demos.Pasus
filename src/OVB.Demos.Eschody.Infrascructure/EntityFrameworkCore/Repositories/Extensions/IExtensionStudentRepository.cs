using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionStudentRepository
{
    public Task<bool> VerifyStudentExistsByEmailAsync(string email, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
}
