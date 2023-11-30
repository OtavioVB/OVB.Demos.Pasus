using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Inputs;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Outputs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;

namespace OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Interfaces;

public interface IStudentService
{
    public Task<ProcessResult<Notification, CreateStudentServiceResult>> CreateStudentServiceAsync(
        CreateStudentServiceInput input,
        CancellationToken cancellationToken);
}
