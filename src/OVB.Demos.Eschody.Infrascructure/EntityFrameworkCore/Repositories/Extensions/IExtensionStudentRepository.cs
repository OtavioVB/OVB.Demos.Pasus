namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionStudentRepository
{
    public Task<bool> VerifyStudentExistsByEmailAsync(string email, CancellationToken cancellationToken);
}
