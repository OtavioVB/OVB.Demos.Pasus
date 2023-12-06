namespace OVB.Demos.Eschody.Application.UseCases.CreateStudent.Outputs;

public readonly struct CreateStudentUseCaseResult
{
    public Guid StudentId { get; }

    private CreateStudentUseCaseResult(Guid studentId)
    {
        StudentId = studentId;
    }

    public static CreateStudentUseCaseResult Build(Guid studentId)
        => new CreateStudentUseCaseResult(studentId);
}
