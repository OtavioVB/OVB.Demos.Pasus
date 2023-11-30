using OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Inputs;
using OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Outputs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;

namespace OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Interfaces;

public interface ICreateStudentDomainFunction
{
    public Task<ProcessResult<Notification, CreateStudentDomainFunctionResult>> CreateStudentDomainFunctionAsync(
        CreateStudentDomainFunctionInput input,
        Func<string, CancellationToken, Task<bool>> verifyStudentExistsByEmail,
        CancellationToken cancellationToken);
}
