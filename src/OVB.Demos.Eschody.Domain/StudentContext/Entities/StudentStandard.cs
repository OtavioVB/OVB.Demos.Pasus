using OVB.Demos.Eschody.Domain.StudentContext.Entities.Base;
using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;

namespace OVB.Demos.Eschody.Domain.StudentContext.Entities;

public sealed class StudentStandard : StudentBase
{
    public StudentStandard() : base(TypeStudent.Standard)
    {
    }
}
