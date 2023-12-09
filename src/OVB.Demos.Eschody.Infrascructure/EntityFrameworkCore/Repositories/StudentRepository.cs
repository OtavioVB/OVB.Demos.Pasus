using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Domain.StudentContext.DataTransferObject;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class StudentRepository : BaseRepository<Student>, IExtensionStudentRepository
{
    public StudentRepository(
            DataContext dbContext, ITraceManager traceManager) 
        : base(
            dbContext: dbContext,
            traceManager: traceManager)
    {
    }

    public Task<bool> VerifyStudentExistsByEmailAsync(string email, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(StudentRepository)}.{nameof(VerifyStudentExistsByEmailAsync)}",
            activityKind: ActivityKind.Internal,
            input: email,
            handler: (input, inputAuditableInfo, activity, cancellationToken)
                => _dbContext.Set<Student>().Where(p => p.Email == email).AnyAsync(cancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken,
            keyValuePairs: [
                KeyValuePair.Create(
                    key: "SearchEmail",
                    value: email)
            ]);
}
